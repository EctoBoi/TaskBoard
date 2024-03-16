using System.Drawing.Imaging;
using Tesseract;
using System.Text;
using System.Runtime.InteropServices;

namespace TaskBoard
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Int32 vKey);
        readonly int VK_HOME = 0x24;

        readonly int defaultUIScale = 100;
        readonly string configPath = ".\\config.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UIScaleInput.Value = GetUIScale();
            CheckKeypressLoop();
        }

        async void CheckKeypressLoop()
        {
            while (true)
            {
                await Task.Delay(100);

                short keyStatus = GetAsyncKeyState(VK_HOME);

                if ((keyStatus & 1) == 1)
                    PostList();
            }
        }

        private void PostList()
        {
            Bitmap bmp = GetTaskListImage();
            //Bitmap bmp = new Bitmap(".\\testimages\\Capture90-3.png");

            if (bmp == null)
                mainDisplayLabel.Text += "Image Error";
            else
            {
                bmp.Save(@".\Capture.png", System.Drawing.Imaging.ImageFormat.Png);

                string[]? taskList = ReadTaskListImage(bmp);
                //string[] taskList = ReadImage(bmp);

                if (taskList != null)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < taskList.Length; i++)
                    {
                        sb.AppendLine(taskList[i].ToString());
                    }
                    mainDisplayLabel.Text = sb.ToString();
                }
                else
                {
                    mainDisplayLabel.Text = "taskList null";
                }

            }
        }

        private void SetUIScale()
        {
            if (File.Exists(configPath))
            {
                String[] lines = File.ReadAllLines(configPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("UIScale"))
                    {
                        lines[i] = $"UIScale={UIScaleInput.Value}";
                    }
                }
                File.WriteAllLines(configPath, lines);
            }
            else
            {
                CreateConfig();
                UIScaleInput.Value = defaultUIScale;
            }
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
                return defaultUIScale;
            }
            else
            {
                CreateConfig();
                return defaultUIScale;
            }
        }

        private void CreateConfig()
        {
            File.Create(configPath).Close();
            String[] lines = [$"UIScale={defaultUIScale}"];
            File.WriteAllLines(configPath, lines);
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

            return ScreenshotTaskList(cX, cY, cWidth, cHeight);
        }

        private static Bitmap ScreenshotTaskList(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            Bitmap captureBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            Size size = new Size(width, height);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(x, y, 0, 0, size, CopyPixelOperation.SourceCopy);

            //Returns fixed size capture of region
            Bitmap resized = new Bitmap(captureBitmap, new Size(1256, 1612));

            return resized;
        }

        private static string[] ReadImage(Bitmap bmp)
        {
            TesseractEngine engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
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

            TesseractEngine engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default);

            for (int i = 0; i < 8; i++)
            {
                Bitmap tempbmp = bmp.Clone(taskPartsRects[i], bmp.PixelFormat);
                Tesseract.Page page = engine.Process(tempbmp, PageSegMode.SparseText);

                //tempbmp.Save($@".\testimages\CapturePart{i}.png", System.Drawing.Imaging.ImageFormat.Png);
                string[] strings = page.GetText().Trim().Split(null);
                taskList[i] = String.Join(" ", strings).Replace("  ", " ");

                if (taskList[i].Length > 0)
                    notEmptyCount++;
                page.Dispose();
            }

            engine.Dispose();

            if (notEmptyCount == 8)
                return taskList;
            else
                return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostList();
        }

        private void SetUIScaleBtn_Click(object sender, EventArgs e)
        {
            SetUIScale();
        }
    }
}
