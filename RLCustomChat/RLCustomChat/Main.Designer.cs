namespace RLCustomChat
{
    partial class Main
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
            this.OverlayState = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.CustomizeChat = new System.Windows.Forms.Button();
            this.DelaySet = new System.Windows.Forms.RichTextBox();
            this.DelayButtonSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OverlayState
            // 
            this.OverlayState.AutoSize = true;
            this.OverlayState.Location = new System.Drawing.Point(8, 12);
            this.OverlayState.Name = "OverlayState";
            this.OverlayState.Size = new System.Drawing.Size(98, 17);
            this.OverlayState.TabIndex = 0;
            this.OverlayState.Text = "Enable Overlay";
            this.OverlayState.UseVisualStyleBackColor = true;
            this.OverlayState.CheckedChanged += new System.EventHandler(this.OverlayState_CheckedChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(8, 36);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(237, 22);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // CustomizeChat
            // 
            this.CustomizeChat.Location = new System.Drawing.Point(137, 7);
            this.CustomizeChat.Name = "CustomizeChat";
            this.CustomizeChat.Size = new System.Drawing.Size(108, 23);
            this.CustomizeChat.TabIndex = 2;
            this.CustomizeChat.Text = "Customize Chat";
            this.CustomizeChat.UseVisualStyleBackColor = true;
            this.CustomizeChat.Click += new System.EventHandler(this.CustomizeChat_Click);
            // 
            // DelaySet
            // 
            this.DelaySet.Location = new System.Drawing.Point(112, 64);
            this.DelaySet.Name = "DelaySet";
            this.DelaySet.Size = new System.Drawing.Size(48, 20);
            this.DelaySet.TabIndex = 3;
            this.DelaySet.Text = "";
            // 
            // DelayButtonSet
            // 
            this.DelayButtonSet.Location = new System.Drawing.Point(8, 64);
            this.DelayButtonSet.Name = "DelayButtonSet";
            this.DelayButtonSet.Size = new System.Drawing.Size(98, 20);
            this.DelayButtonSet.TabIndex = 5;
            this.DelayButtonSet.Text = "Sleep/Delay";
            this.DelayButtonSet.UseVisualStyleBackColor = true;
            this.DelayButtonSet.Click += new System.EventHandler(this.SetDelay);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(266, 87);
            this.Controls.Add(this.DelayButtonSet);
            this.Controls.Add(this.DelaySet);
            this.Controls.Add(this.CustomizeChat);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.OverlayState);
            this.ForeColor = System.Drawing.SystemColors.MenuText;
            this.Name = "Main";
            this.Text = "Custom RL Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox OverlayState;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button CustomizeChat;
        public System.Windows.Forms.RichTextBox DelaySet;
        private System.Windows.Forms.Button DelayButtonSet;
    }
}

