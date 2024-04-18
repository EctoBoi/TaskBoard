using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaskBoard
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
            infoLbl = new Label();
            postListBtn = new System.Windows.Forms.Button();
            UIScaleLbl = new Label();
            IPTxt = new System.Windows.Forms.TextBox();
            label1 = new Label();
            connectBtn = new System.Windows.Forms.Button();
            statusLbl = new Label();
            label3 = new Label();
            userTxt = new System.Windows.Forms.TextBox();
            clearBtn = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // infoLbl
            // 
            infoLbl.BackColor = Color.FromArgb(43, 45, 49);
            infoLbl.BorderStyle = BorderStyle.FixedSingle;
            infoLbl.ForeColor = Color.FromArgb(215, 215, 220);
            infoLbl.Location = new Point(12, 50);
            infoLbl.Name = "infoLbl";
            infoLbl.Size = new Size(776, 589);
            infoLbl.TabIndex = 0;
            // 
            // postListBtn
            // 
            postListBtn.BackColor = Color.White;
            postListBtn.Enabled = false;
            postListBtn.ForeColor = Color.Black;
            postListBtn.Location = new Point(12, 651);
            postListBtn.Name = "postListBtn";
            postListBtn.Size = new Size(103, 23);
            postListBtn.TabIndex = 2;
            postListBtn.Text = "Post List (Home)";
            postListBtn.UseVisualStyleBackColor = false;
            postListBtn.Click += postListBtn_Click;
            // 
            // UIScaleLbl
            // 
            UIScaleLbl.AutoSize = true;
            UIScaleLbl.Location = new Point(716, 655);
            UIScaleLbl.Name = "UIScaleLbl";
            UIScaleLbl.Size = new Size(72, 15);
            UIScaleLbl.TabIndex = 3;
            UIScaleLbl.Text = "UI Scale: 100";
            // 
            // IPTxt
            // 
            IPTxt.BackColor = Color.FromArgb(43, 45, 49);
            IPTxt.ForeColor = SystemColors.MenuBar;
            IPTxt.Location = new Point(60, 12);
            IPTxt.Name = "IPTxt";
            IPTxt.Size = new Size(172, 23);
            IPTxt.TabIndex = 4;
            IPTxt.Text = "127.0.0.1:4545";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 5;
            label1.Text = "Server:";
            // 
            // connectBtn
            // 
            connectBtn.BackColor = Color.White;
            connectBtn.ForeColor = Color.Black;
            connectBtn.Location = new Point(452, 12);
            connectBtn.Name = "connectBtn";
            connectBtn.Size = new Size(75, 23);
            connectBtn.TabIndex = 6;
            connectBtn.Text = "Connect";
            connectBtn.UseVisualStyleBackColor = false;
            connectBtn.Click += connectBtn_Click;
            // 
            // statusLbl
            // 
            statusLbl.AutoSize = true;
            statusLbl.Location = new Point(184, 655);
            statusLbl.Name = "statusLbl";
            statusLbl.Size = new Size(42, 15);
            statusLbl.TabIndex = 7;
            statusLbl.Text = "Status:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(238, 15);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 9;
            label3.Text = "User:";
            // 
            // userTxt
            // 
            userTxt.BackColor = Color.FromArgb(43, 45, 49);
            userTxt.ForeColor = SystemColors.MenuBar;
            userTxt.Location = new Point(277, 12);
            userTxt.Name = "userTxt";
            userTxt.Size = new Size(169, 23);
            userTxt.TabIndex = 8;
            // 
            // clearBtn
            // 
            clearBtn.BackColor = Color.White;
            clearBtn.Enabled = false;
            clearBtn.ForeColor = Color.Black;
            clearBtn.Location = new Point(121, 651);
            clearBtn.Name = "clearBtn";
            clearBtn.Size = new Size(57, 23);
            clearBtn.TabIndex = 10;
            clearBtn.Text = "Clear";
            clearBtn.UseVisualStyleBackColor = false;
            clearBtn.Click += clearBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(49, 51, 56);
            ClientSize = new Size(800, 686);
            Controls.Add(clearBtn);
            Controls.Add(label3);
            Controls.Add(userTxt);
            Controls.Add(statusLbl);
            Controls.Add(connectBtn);
            Controls.Add(label1);
            Controls.Add(IPTxt);
            Controls.Add(UIScaleLbl);
            Controls.Add(postListBtn);
            Controls.Add(infoLbl);
            ForeColor = Color.FromArgb(215, 215, 220);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Task Board";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label infoLbl;
        private Label label2;
        private System.Windows.Forms.Button postListBtn;
        private Label UIScaleLbl;
        private System.Windows.Forms.TextBox IPTxt;
        private Label label1;
        private System.Windows.Forms.Button connectBtn;
        private Label statusLbl;
        private Label label3;
        private System.Windows.Forms.TextBox userTxt;
        private System.Windows.Forms.Button clearBtn;
    }
}
