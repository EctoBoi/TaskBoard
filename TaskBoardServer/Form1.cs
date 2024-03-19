using SuperSimpleTcp;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private class User(string IpPort)
        {
            public readonly string IpPort = IpPort;
            public string username = "NONAME";
            public string[] taskList = new string[8];
            public DateTime lastListUpdate = new();
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
            users.Add(new User(e.IpPort));
            UpdateUserList();
            infoTxt.Text += $"{e.IpPort} connected.{Environment.NewLine}";
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                users.Remove(users.Single(x => x.IpPort == e.IpPort));
                UpdateUserList();
                infoTxt.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string dataString = Encoding.UTF8.GetString(e.Data);

                if (dataString.Contains("$taskList="))
                {
                    string[] strings = dataString.Split(Environment.NewLine);
                    User u = users.Single(x => x.IpPort == e.IpPort);
                    for (int i = 0; i < 8; i++)
                    {
                        u.taskList[i] = strings[i + 1];
                    }
                    u.lastListUpdate = DateTime.Now;
                    SendTaskBoard();
                }
                if (dataString.Contains("$user="))
                {
                    string username = dataString.Split('=')[1];
                    users.Single(x => x.IpPort == e.IpPort).username = username;
                    UpdateUserList();
                    infoTxt.Text += $"{e.IpPort}: Username is {username}{Environment.NewLine}";
                }
            });
        }

        private void UpdateUserList()
        {
            userLst.Items.Clear();
            users.ForEach(x => userLst.Items.Add(x.IpPort + "=" + x.username));
        }

        private void SendTaskBoard()
        {
            StringBuilder sb = new();
            foreach (User u in users)
            {
                int hour;
                if(u.lastListUpdate.Hour>12)
                    hour = u.lastListUpdate.Hour-12;
                else 
                    hour = u.lastListUpdate.Hour;
                sb.AppendLine(u.username + " @ " + hour + ":" + u.lastListUpdate.Minute);
                for (int i = 0; i < 8; i++)
                {
                    sb.AppendLine(u.taskList[i]);
                }
                sb.AppendLine("");
            }

            foreach (User u in users)
            {
                if (server != null)
                {
                    server.Send(u.IpPort, sb.ToString());
                    infoTxt.Text += $"Task Board sent to {u.username}{Environment.NewLine}";
                }
                else
                    infoTxt.Text += $"SendTaskBoard Error{Environment.NewLine}";
            }

        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            server = CreateServer();
            server.Start();
            infoTxt.Text += $"Starting...{Environment.NewLine}";
            startBtn.Enabled = false;
        }
    }
}
