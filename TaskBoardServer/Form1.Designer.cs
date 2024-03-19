namespace TaskBoardServer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            startBtn = new Button();
            label1 = new Label();
            IPTxt = new TextBox();
            infoTxt = new TextBox();
            userLst = new ListBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // startBtn
            // 
            startBtn.Location = new Point(526, 11);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(75, 23);
            startBtn.TabIndex = 9;
            startBtn.Text = "Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += startBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 14);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 8;
            label1.Text = "Server: ";
            // 
            // IPTxt
            // 
            IPTxt.Location = new Point(58, 11);
            IPTxt.Name = "IPTxt";
            IPTxt.Size = new Size(462, 23);
            IPTxt.TabIndex = 7;
            IPTxt.Text = "127.0.0.1:4545";
            // 
            // infoTxt
            // 
            infoTxt.Location = new Point(58, 40);
            infoTxt.Multiline = true;
            infoTxt.Name = "infoTxt";
            infoTxt.ReadOnly = true;
            infoTxt.ScrollBars = ScrollBars.Both;
            infoTxt.Size = new Size(543, 304);
            infoTxt.TabIndex = 10;
            // 
            // userLst
            // 
            userLst.FormattingEnabled = true;
            userLst.ItemHeight = 15;
            userLst.Location = new Point(607, 40);
            userLst.Name = "userLst";
            userLst.Size = new Size(162, 304);
            userLst.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(607, 14);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 12;
            label2.Text = "Users:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 355);
            Controls.Add(label2);
            Controls.Add(userLst);
            Controls.Add(infoTxt);
            Controls.Add(startBtn);
            Controls.Add(label1);
            Controls.Add(IPTxt);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Task Board Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button startBtn;
        private Label label1;
        private TextBox IPTxt;
        private TextBox infoTxt;
        private ListBox userLst;
        private Label label2;
    }
}
