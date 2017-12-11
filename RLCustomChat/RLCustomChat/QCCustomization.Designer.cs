namespace RLCustomChat
{
    partial class QCCustomization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.AddChatSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(96, 150);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(176, 40);
            this.richTextBox4.TabIndex = 0;
            this.richTextBox4.Text = "";
            this.richTextBox4.Leave += new System.EventHandler(this.entered);
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(96, 106);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(176, 40);
            this.richTextBox3.TabIndex = 1;
            this.richTextBox3.Text = "";
            this.richTextBox3.Leave += new System.EventHandler(this.entered);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(96, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(176, 40);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            this.richTextBox1.Leave += new System.EventHandler(this.entered);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(96, 59);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(176, 40);
            this.richTextBox2.TabIndex = 3;
            this.richTextBox2.Text = "";
            this.richTextBox2.Leave += new System.EventHandler(this.entered);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 22);
            this.button1.TabIndex = 4;
            this.button1.Text = "0";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttons_pressed);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(41, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 22);
            this.button2.TabIndex = 5;
            this.button2.Text = "1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttons_pressed);
            // 
            // AddChatSet
            // 
            this.AddChatSet.Location = new System.Drawing.Point(13, 166);
            this.AddChatSet.Name = "AddChatSet";
            this.AddChatSet.Size = new System.Drawing.Size(22, 23);
            this.AddChatSet.TabIndex = 6;
            this.AddChatSet.Text = "+";
            this.AddChatSet.UseVisualStyleBackColor = true;
            this.AddChatSet.Click += new System.EventHandler(this.AddChatSet_Click);
            // 
            // QCCustomization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 199);
            this.Controls.Add(this.AddChatSet);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox4);
            this.Name = "QCCustomization";
            this.Text = "QCCustomization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QCCustomization_Closing);
            this.Click += new System.EventHandler(this.buttons_pressed);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox4;
        public System.Windows.Forms.RichTextBox richTextBox3;
        public System.Windows.Forms.RichTextBox richTextBox1;
        public System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button AddChatSet;
        public System.Windows.Forms.Button button1;
    }
}