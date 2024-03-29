using SuperSimpleTcp;
using System.Text;
using System.Text.RegularExpressions;

namespace TaskBoardServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpServer? server;

        private List<User> users = [];
        private List<User> oldUsers = [];

        private List<string> reminderList = [];
        private string taskBoard = "";

        private class User(string IpPort)
        {
            public readonly string IpPort = IpPort;
            public string username = "NONAME";
            public string[] taskList = new string[8];
            public DateTime lastListUpdate = DateTime.Now;
            public DateTime lastActive = DateTime.Now;
        }

        private SimpleTcpServer CreateServer()
        {
            SimpleTcpServer s = new(IPTxt.Text);
            s.Events.ClientConnected += Events_ClientConnected;
            s.Events.ClientDisconnected += Events_ClientDisconnected;
            s.Events.DataReceived += Events_DataReceived;
            return s;
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                users.Add(new User(e.IpPort));
                infoTxt.Text += $"{e.IpPort} connected{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                oldUsers.Add(users.Single(x => x.IpPort == e.IpPort));
                users.Remove(users.Single(x => x.IpPort == e.IpPort));
                UpdateUserList();
                infoTxt.Text += $"{e.IpPort} disconnected{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string dataString = Encoding.UTF8.GetString(e.Data);

                Match userDataMatch = UserDataRegex().Match(dataString);
                if (userDataMatch.Success)
                {
                    string username = userDataMatch.Groups[1].Value;

                    User currentUser = users.Single(x => x.IpPort == e.IpPort);
                    currentUser.username = username;
                    if (oldUsers.Exists(x => x.username == username))
                    {
                        User oldUser = oldUsers.Single(x => x.username == username);
                        currentUser.taskList = oldUser.taskList;
                        currentUser.lastListUpdate = oldUser.lastListUpdate;
                        oldUsers.Remove(oldUser);
                    }

                    infoTxt.Text += $"{e.IpPort}: Username is {username}{Environment.NewLine}";
                    UpdateUserList();
                    SendTaskBoard();
                }

                Match taskListDataMatch = taskListDataRegex().Match(dataString);
                if (taskListDataMatch.Success)
                {
                    string[] strings = taskListDataMatch.Groups[1].Value.Split(Environment.NewLine);
                    User u = users.Single(x => x.IpPort == e.IpPort);
                    for (int i = 0; i < 8; i++)
                    {
                        u.taskList[i] = strings[i + 1].Trim();
                    }
                    u.lastListUpdate = DateTime.Now;
                    UpdateTaskBoard();
                    SendTaskBoard();
                }

                if (keepAliveDataRegex().Match(dataString).Success)
                {
                    users.Single(x => x.IpPort == e.IpPort).lastActive = DateTime.Now;
                    server?.Send(e.IpPort, "$keepAlive");
                }
            });
        }

        private async void RemoveInactiveUsersLoop()
        {
            while (true)
            {
                foreach (User user in users)
                {
                    try
                    {
                        if (DateTime.Now - user.lastActive > TimeSpan.FromMilliseconds(69999))
                        {
                            infoTxt.Text += $"{user.IpPort} removed for inactivity{Environment.NewLine}";
                            oldUsers.Add(user);
                            users.Remove(user);
                            UpdateUserList();
                        }
                    }
                    catch (Exception e)
                    {
                        infoTxt.Text += $"{e.Message}{Environment.NewLine}";
                    }
                }
                await Task.Delay(59999);
            }
        }

        private void UpdateUserList()
        {
            userLst.Items.Clear();
            users.ForEach(x => userLst.Items.Add(x.IpPort + "=" + x.username));
        }

        private void SendTaskBoard()
        {
            foreach (User u in users)
            {
                if (server != null)
                {
                    server.Send(u.IpPort, taskBoard);
                }
                else
                    infoTxt.Text += $"SendTaskBoard Error{Environment.NewLine}";
            }
        }

        private void UpdateTaskBoard()
        {
            users = [.. users.OrderBy(o => o.username)];
            StringBuilder mainBoard = new();

            foreach (User u in users)
            {
                string hour;
                if (u.lastListUpdate.Hour > 12)
                {
                    hour = (u.lastListUpdate.Hour - 12).ToString();
                }
                else
                    hour = u.lastListUpdate.Hour.ToString();
                if (hour == "0")
                    hour = "12";
                string minute;
                if (u.lastListUpdate.Minute < 10)
                    minute = "0" + u.lastListUpdate.Minute.ToString();
                else
                    minute = u.lastListUpdate.Minute.ToString();
                mainBoard.AppendLine($"{u.username} @ {hour}:{minute}");
                mainBoard.AppendLine("");

                reminderList = [];
                for (int i = 0; i < 8; i++)
                {
                    if (i % 2 == 0) //check task
                    {
                        mainBoard.Append(SimplifyTask(u.taskList[i]));
                    }
                    else // check score
                    {
                        if (!string.IsNullOrEmpty(u.taskList[i]))
                        {
                            Regex regex = ScoreRegex();

                            if (regex.IsMatch(u.taskList[i]))
                                mainBoard.AppendLine($" ({u.taskList[i]})");
                            else
                                mainBoard.AppendLine("");
                        }
                        else
                            mainBoard.AppendLine("");
                    }

                }
                mainBoard.AppendLine("");
            }

            mainBoard.AppendLine("===Reminders===");
            mainBoard.AppendLine("");

            foreach(string reminder in reminderList)
            {
                mainBoard.AppendLine(reminder);
            }

            taskBoard = mainBoard.ToString();
        }

        private void AddReminder(string reminder)
        {
            if (!reminderList.Contains(reminder))
            {
                reminderList.Add(reminder);
            }
        }

        private string SimplifyTask(string task)
        {
            string simp = "";

            if (task != null)
            {
                switch (task)
                {
                    case string t when t.Contains("Grunts"):
                        simp = "Grunts"; AddReminder(simp);
                        break;
                    case string t when t.Contains("Lantern Grunt"):
                        simp = "Lantern Grunts"; AddReminder(simp);
                        break;
                    case string t when t.Contains("Pistol Grunt"):
                        simp = "Pistol Grunts"; AddReminder(simp);
                        break;
                    case string t when t.Contains("Meatheads"):
                        simp = "Meatheads"; AddReminder(simp);
                        break;
                    case string t when t.Contains("Water Devils"):
                        simp = "Water Devils"; AddReminder(simp);
                        break;
                    case string t when t.Contains("Armoreds"):
                        simp = "Armoreds";
                        if (t.Contains("Fire Damage"))
                            simp = "Armoreds with Fire Damage";
                        else if (t.Contains("Poison Damage"))
                            simp = "Armoreds with Poison Damage";
                        AddReminder(simp);
                        break;
                    case string t when t.Contains("Immolators"):
                        simp = "Immolators";
                        if (t.Contains("Choke"))
                            simp = "Immolators with Choke";
                        else if (t.Contains("Dusters"))
                            simp = "Immolators with Dusters";
                        AddReminder(simp);
                        break;
                    case string t when t.Contains("Hellhounds"):
                        simp = "Hellhounds";
                        if (t.Contains("Fire Damage"))
                            simp = "Hellhounds with Fire Damage";
                        else if (t.Contains("Poison Damage"))
                            simp = "Hellhounds with Poison Damage";
                        AddReminder(simp);
                        break;
                    case string t when t.Contains("Hives"):
                        simp = "Hives";
                        if (t.Contains("Fire Damage"))
                            simp = "Hives with Fire Damage";
                        else if (t.Contains("Throwing"))
                            simp = "Hives with a Throwing Weapon";
                        AddReminder(simp);
                        break;
                    case string t when t.Contains("Destroy"):
                        simp = "Dog Cages"; AddReminder(simp);
                        break;
                    case string t when t.Contains("Collect Clues"):
                        simp = "Collect Clues";
                        break;
                    case string t when t.Contains("Banish Targets"):
                        simp = "Banish Targets";
                        break;
                    case string t when t.Contains("Extract"):
                        simp = "Extract with a Bounty";
                        break;
                    case string t when t.Contains("Trait Spurs"):
                        simp = "Trait Spurs";
                        break;
                    case string t when t.Contains("Hunters bleed"):
                        simp = "Bleed Hunters";
                        break;
                    case string t when t.Contains("Hunters on fire"):
                        simp = "Burn Hunters";
                        break;
                    case string t when t.Contains("Poison enemy"):
                        simp = "Poison Hunters";
                        break;
                    case string t when t.Contains("Melee Damage"):
                        simp = "Melee Hunters";
                        break;
                    case string t when t.Contains("damage to enemy Hunters") && !t.Contains("using"):
                        simp = "Damage Hunters";
                        break;
                    case string t when t.Contains("headshot") && t.Contains(':'):
                        simp = "Headshot Hunters with " + task.Split(':')[1].Trim();
                        break;
                    case string t when t.Contains("Hunters using") && t.Contains(':'):
                        simp = "Damage with " + task.Split(':')[1].Trim();
                        break;
                    case string t when t.Contains("headshot"):
                        simp = "Headshot Hunters";
                        break;
                    default:
                        simp = task;
                        break;
                }
            }

            return simp;
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            infoTxt.Text += $"Starting...{Environment.NewLine}";
            server = CreateServer();
            server.Start();
            RemoveInactiveUsersLoop();
            startBtn.Enabled = false;
        }

        [GeneratedRegex(@"^[0-9][0-9]*/[1-9][0-9]*$")]
        private static partial Regex ScoreRegex();
        [GeneratedRegex(@"\$user=(.*?)(?:\$|$)")]
        private static partial Regex UserDataRegex();
        [GeneratedRegex(@"\$keepAlive")]
        private static partial Regex keepAliveDataRegex();
        [GeneratedRegex(@"\$taskList=(.*?)(?:\$|$)", RegexOptions.Singleline)]
        private static partial Regex taskListDataRegex();
    }
}
