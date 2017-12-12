using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RLCustomChat
{
    public partial class QCCustomization : Form
    {
        public RichTextBox[] textBoxes = new RichTextBox[4];
        public Button[] buttons = new Button[4];

        public int ChatsTarget = 0;
        public string[,] Chats = new string[4,4];
        public int defaultQCSize = 1;
        public bool RefreshOverlayCue = false;
        public QCCustomization()
        {
            InitializeComponent();
           
            textBoxes[0] = richTextBox1;
            textBoxes[1] = richTextBox2;
            textBoxes[2] = richTextBox3;
            textBoxes[3] = richTextBox4;

            buttons[0] = button1;
            buttons[1] = button2;
            
            
        }

        public void UpdateText(Dictionary<Microsoft.Xna.Framework.Input.Buttons, string> toChange = null)
        {
            

            for (int i = 0; i < 4; i++)
            {
                Chats[ChatsTarget, i] = textBoxes[i].Text;
                if(toChange != null)
                {
                    toChange[toChange.ElementAt(i).Key] = textBoxes[i].Text;
                }
                
            }
           
        }

    
        private void buttons_pressed(object sender, EventArgs e)
        {
            Button b = new Button();
            try
            {
                b = (Button)sender;
            }catch(InvalidCastException)
            {
                return;
            }
            ChatsTarget = Convert.ToInt16(b.Text);
            for(int i = 0; i < 4; i++)
            {
                textBoxes[i].Text = Chats[ChatsTarget, i];
            }


        }

        private void entered(object sender, EventArgs e)
        {
            RefreshOverlayCue = true;
        }

        private void QCCustomization_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void AddChatSet_Click(object sender, EventArgs e)
        {
            if(!((defaultQCSize += 1) >= buttons.Length))
            {
                Button button = new Button();

                button.Name = Convert.ToString("button" + (defaultQCSize));

                button.Text = Convert.ToString(defaultQCSize);
                button.Location = new Point(buttons[defaultQCSize - 2].Location.X, (buttons[defaultQCSize - 2].Location.Y) + 22);
                button.Size = new Size(22, 22);
                button.Visible = true;

                buttons[defaultQCSize] = button;
              
                button.Click += new EventHandler(buttons_pressed);
                Controls.Add(button);
            }else
            {
                return;
            }
            
        }
    }
}
