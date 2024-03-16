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
            mainDisplayLabel = new Label();
            cBox = new System.Windows.Forms.TextBox();
            PostListBtn = new System.Windows.Forms.Button();
            UIScaleLabel = new Label();
            UIScaleInput = new NumericUpDown();
            SetUIScaleBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)UIScaleInput).BeginInit();
            SuspendLayout();
            // 
            // mainDisplayLabel
            // 
            mainDisplayLabel.BackColor = SystemColors.Window;
            mainDisplayLabel.BorderStyle = BorderStyle.FixedSingle;
            mainDisplayLabel.Location = new Point(12, 9);
            mainDisplayLabel.Name = "mainDisplayLabel";
            mainDisplayLabel.Size = new Size(776, 314);
            mainDisplayLabel.TabIndex = 0;
            mainDisplayLabel.Text = "mainDisplayLabel";
            // 
            // cBox
            // 
            cBox.Location = new Point(12, 337);
            cBox.Multiline = true;
            cBox.Name = "cBox";
            cBox.Size = new Size(776, 79);
            cBox.TabIndex = 1;
            cBox.Text = "cBox";
            // 
            // PostListBtn
            // 
            PostListBtn.Location = new Point(12, 422);
            PostListBtn.Name = "PostListBtn";
            PostListBtn.Size = new Size(103, 23);
            PostListBtn.TabIndex = 2;
            PostListBtn.Text = "Post List (Home)";
            PostListBtn.UseVisualStyleBackColor = true;
            PostListBtn.Click += button1_Click;
            // 
            // UIScaleLabel
            // 
            UIScaleLabel.AutoSize = true;
            UIScaleLabel.Location = new Point(140, 426);
            UIScaleLabel.Name = "UIScaleLabel";
            UIScaleLabel.Size = new Size(48, 15);
            UIScaleLabel.TabIndex = 4;
            UIScaleLabel.Text = "UI Scale";
            // 
            // UIScaleInput
            // 
            UIScaleInput.Location = new Point(190, 422);
            UIScaleInput.Minimum = new decimal(new int[] { 80, 0, 0, 0 });
            UIScaleInput.Name = "UIScaleInput";
            UIScaleInput.Size = new Size(48, 23);
            UIScaleInput.TabIndex = 5;
            UIScaleInput.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // SetUIScaleBtn
            // 
            SetUIScaleBtn.Location = new Point(244, 422);
            SetUIScaleBtn.Name = "SetUIScaleBtn";
            SetUIScaleBtn.Size = new Size(35, 23);
            SetUIScaleBtn.TabIndex = 6;
            SetUIScaleBtn.Text = "Set";
            SetUIScaleBtn.UseVisualStyleBackColor = true;
            SetUIScaleBtn.Click += SetUIScaleBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 453);
            Controls.Add(SetUIScaleBtn);
            Controls.Add(UIScaleInput);
            Controls.Add(UIScaleLabel);
            Controls.Add(PostListBtn);
            Controls.Add(cBox);
            Controls.Add(mainDisplayLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Task Board";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)UIScaleInput).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label mainDisplayLabel;
        private Label label2;
        private System.Windows.Forms.TextBox cBox;
        private System.Windows.Forms.Button PostListBtn;
        private Label UIScaleLabel;
        private NumericUpDown UIScaleInput;
        private System.Windows.Forms.Button SetUIScaleBtn;
    }
}
