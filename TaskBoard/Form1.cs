using System.Drawing.Imaging;
using Tesseract;
using System.Text;
using System.Runtime.InteropServices;
using SuperSimpleTcp;
using Microsoft.VisualBasic.ApplicationServices;

namespace TaskBoard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [LibraryImport("user32.dll")]
        private static partial short GetAsyncKeyState(Int32 vKey);
        readonly int VK_HOME = 0x24;

        readonly static int defaultUIScale = 100;
        readonly static string configPath = @".\config.txt";
        readonly static string localIP = "127.0.0.1:4545";

        int UIScale = defaultUIScale;

        int reconnects = 0;

        SimpleTcpClient? client;

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateConfig();
            UIScale = GetUIScale();
            UIScaleLbl.Text = "UI Scale: " + UIScale;
            IPTxt.Text = GetIP();
            userTxt.Text = GetUser();
            CheckKeypressLoop();
        }

        private SimpleTcpClient CreateClient()
        {
            SimpleTcpClient c = new(IPTxt.Text);
            c.Events.Connected += Events_Connected;
            c.Events.Disconnected += Events_Disconnected;
            c.Events.DataReceived += Events_DataReceived;
            return c;
        }

        private void Events_Connected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                KeepAliveLoop();
                connectBtn.Enabled = false;
                IPTxt.Enabled = false;
                userTxt.Enabled = false;
                postListBtn.Enabled = true;
                clearBtn.Enabled = true;
                SetStatus("Connected");
                if (client != null)
                    client.Send($"$user={userTxt.Text}");
                else
                    SetStatus("Client Error");
            });
        }

        private void Events_Disconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                connectBtn.Enabled = true;
                IPTxt.Enabled = true;
                userTxt.Enabled = true;
                postListBtn.Enabled = false;
                clearBtn.Enabled = false;
                SetStatus("Disconnected");
                if (reconnects < 3)
                {
                    reconnects++;
                    SetStatus("Reconnecting...");
                    Task.Delay(1000).ContinueWith(t => Connect());
                }
            });
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                reconnects = 0;
                string dataString = Encoding.UTF8.GetString(e.Data);
                if (dataString != "$keepAlive")
                    infoLbl.Text = dataString;
            });
        }

        private async void KeepAliveLoop()
        {
            while (true)
            {
                if (client != null && client.IsConnected)
                {
                    client.Send("$keepAlive");
                    await Task.Delay(59999);
                }
            }
        }

        private void SetStatus(string status)
        {
            statusLbl.Text = "Status: " + status;
        }

        private async void CheckKeypressLoop()
        {
            while (true)
            {
                await Task.Delay(100);

                short keyStatus = GetAsyncKeyState(VK_HOME);

                if ((keyStatus & 1) == 1 && client != null && client.IsConnected)
                    PostList();
            }
        }

        private void PostList()
        {
            Bitmap bmp = GetTaskListImage();

            if (bmp == null)
                SetStatus("Image Error");
            else
            {
                bmp.Save(@".\Capture.png", System.Drawing.Imaging.ImageFormat.Png);

                string[]? taskList = ReadTaskListImage(bmp);
                //string[] taskList = ReadImage(bmp);

                if (taskList != null)
                {
                    StringBuilder sb = new();
                    sb.AppendLine("$taskList=");
                    for (int i = 0; i < 8; i++)
                    {
                        sb.AppendLine(taskList[i].ToString());
                    }
                    if (client != null)
                    {
                        client.Send(sb.ToString());
                        SetStatus("Posted");
                    }
                    else
                        SetStatus("Client Error");
                }
                else
                {
                    SetStatus("Task List Error");
                }

            }
        }

        private void ClearList()
        {
            StringBuilder sb = new();
            sb.AppendLine("$taskList=");
            for (int i = 0; i < 8; i++)
            {
                sb.AppendLine("");
            }
            if (client != null)
            {
                client.Send(sb.ToString());
                SetStatus("Cleared");
            }
            else
                SetStatus("Client Error");
        }

        private int GetUIScale()
        {
            if (File.Exists(configPath))
            {
                String[] lines = File.ReadAllLines(configPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("UIScale"))
                    {
                        string[] UISacle = lines[i].Split("=");
                        return Int32.Parse(UISacle[1]);
                    }
                }
                //Add line if not found
                File.AppendAllText(configPath, $"UIScale={defaultUIScale}" + Environment.NewLine);
                return defaultUIScale;
            }
            else
            {
                CreateConfig();
                return defaultUIScale;
            }
        }

        private void SetIP()
        {
            if (File.Exists(configPath))
            {
                bool lineEdited = false;
                String[] lines = File.ReadAllLines(configPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("IP"))
                    {
                        lines[i] = $"IP={IPTxt.Text}";
                        lineEdited = true;
                    }
                }
                File.WriteAllLines(configPath, lines);

                if (!lineEdited)
                {
                    File.AppendAllText(configPath, $"IP={IPTxt.Text}" + Environment.NewLine);
                }
            }
            else
            {
                CreateConfig();
            }
        }

        private string GetIP()
        {
            if (File.Exists(configPath))
            {
                String[] lines = File.ReadAllLines(configPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("IP"))
                    {
                        string[] IP = lines[i].Split("=");
                        return IP[1];
                    }
                }
                //Add line if not found
                File.AppendAllText(configPath, $"IP={localIP}" + Environment.NewLine);
                return localIP;
            }
            else
            {
                CreateConfig();
                return localIP;
            }
        }

        private void SetUser()
        {
            if (File.Exists(configPath))
            {
                bool lineEdited = false;
                String[] lines = File.ReadAllLines(configPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("User"))
                    {
                        lines[i] = $"User={userTxt.Text}";
                        lineEdited = true;
                    }
                }
                File.WriteAllLines(configPath, lines);

                if (!lineEdited)
                {
                    File.AppendAllText(configPath, $"User={userTxt.Text}" + Environment.NewLine);
                }
            }
            else
            {
                CreateConfig();
            }
        }

        private string GetUser()
        {
            if (File.Exists(configPath))
            {
                String[] lines = File.ReadAllLines(configPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("User"))
                    {
                        string[] user = lines[i].Split("=");
                        return user[1];
                    }
                }
                //Add line if not found
                File.AppendAllText(configPath, $"User={userTxt.Text}" + Environment.NewLine);
                return "";
            }
            else
            {
                CreateConfig();
                return "";
            }
        }

        private void CreateConfig()
        {
            if (!File.Exists(configPath))
            {
                File.Create(configPath).Close();
                String[] lines = [
                    $"UIScale={defaultUIScale}",
                    $"IP={localIP}",
                    $"User="
                ];
                File.WriteAllLines(configPath, lines);
            }
        }

        private Bitmap GetTaskListImage()
        {
            //Creates a region based on current UIScale and 1080p screen
            int maxX = 1089, maxY = 262, maxWidth = 349, maxHeight = 448;
            int minX = 1063, minY = 318, minWidth = 279, minHeight = 358;

            float scale = 1 - (((100 - (float)GetUIScale()) * 5) / 100);

            int cX = (Int32)(((maxX - minX) * scale) + minX);
            int cY = (Int32)(((maxY - minY) * scale) + minY);
            int cWidth = (Int32)(((maxWidth - minWidth) * scale) + minWidth);
            int cHeight = (Int32)(((maxHeight - minHeight) * scale) + minHeight);


            Bitmap screenshot = ScreenshotTaskList(cX, cY, cWidth, cHeight);
            //Bitmap screenshot = new(@".\testimages\Capture3.png");

            return screenshot;
        }

        private static Bitmap ScreenshotTaskList(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            Bitmap captureBitmap = new(width, height, PixelFormat.Format32bppArgb);

            Size size = new(width, height);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(x, y, 0, 0, size, CopyPixelOperation.SourceCopy);

            //Returns fixed size capture of region
            Bitmap resized = new(captureBitmap, new Size(1256, 1612));

            return resized;
        }

        private static string[] ReadImage(Bitmap bmp)
        {
            TesseractEngine engine = new("./tessdata", "eng", EngineMode.Default);
            Tesseract.Page page = engine.Process(bmp, PageSegMode.SparseText);
            ResultIterator iter = page.GetIterator();

            string preSplit = "";

            iter.Begin();
            do
            {
                string lineTxt = iter.GetText(PageIteratorLevel.TextLine);
                if (lineTxt != null)
                    preSplit += lineTxt.Trim() + "$%";

            } while (iter.Next(PageIteratorLevel.TextLine));

            engine.Dispose();

            return preSplit.Split("$%");
        }

        private static string[]? ReadTaskListImage(Bitmap bmp)
        {
            string[] taskList = new string[8];

            //Reads specific regions in image of size 1256x1612
            int nameX = 25, nameW = 1200, nameH = 176, valueX = 950, valueW = 277, valueH = 57;
            Rectangle[] taskPartsRects =
                [
                    new Rectangle(nameX, 46, nameW, nameH),
                    new Rectangle(valueX, 342, valueW, valueH),
                    new Rectangle(nameX, 450, nameW, nameH),
                    new Rectangle(valueX, 742, valueW, valueH),
                    new Rectangle(nameX, 855, nameW, nameH),
                    new Rectangle(valueX, 1146, valueW, valueH),
                    new Rectangle(nameX, 1252, nameW, nameH),
                    new Rectangle(valueX, 1548, valueW, valueH)
                ];

            int notEmptyCount = 0;

            using (StreamWriter writer = new(@".\list.txt"))
            {
                using TesseractEngine engine = new("./tessdata", "eng", EngineMode.Default);
                for (int i = 0; i < 8; i++)
                {
                    Bitmap tempbmp = bmp.Clone(taskPartsRects[i], bmp.PixelFormat);
                    if (i % 2 == 0)
                    {
                        engine.SetVariable("tessedit_char_whitelist", "");
                        tempbmp = OCRPreprocessor.PreprocessForOCR(tempbmp, 85);
                    }
                    else
                    {
                        engine.SetVariable("tessedit_char_whitelist", "/0123456789");
                        tempbmp = OCRPreprocessor.PreprocessForOCR(tempbmp, 100);
                    }
                        
                    using Tesseract.Page page = engine.Process(tempbmp, PageSegMode.SingleBlock);
                    //tempbmp.Save($@".\testimages\CapturePart{i}.png", System.Drawing.Imaging.ImageFormat.Png);
                    string[] strings = page.GetText().Trim().Split(null);
                    taskList[i] = String.Join(" ", strings).Replace("  ", " ");

                    writer.WriteLine(taskList[i]);

                    if (taskList[i].Length > 0)
                        notEmptyCount++;
                }
            }

            if (notEmptyCount > 3)
                return taskList;
            else
                return null;
        }

        private void Connect()
        {
            if (!string.IsNullOrEmpty(IPTxt.Text))
            {
                if (!string.IsNullOrEmpty(userTxt.Text))
                {
                    if (!userTxt.Text.Contains('='))
                    {
                        SetIP();
                        SetUser();
                        try
                        {
                            client = CreateClient();
                            client.Connect();
                        }
                        catch (Exception ex) { SetStatus("Connection Error: " + ex.Message); }
                    }
                    else { MessageBox.Show("Username can't contain \"=\"!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else { MessageBox.Show("Username empty!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else { MessageBox.Show("Server IP empty!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void postListBtn_Click(object sender, EventArgs e)
        {
            PostList();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            ClearList();
        }
    }
}
