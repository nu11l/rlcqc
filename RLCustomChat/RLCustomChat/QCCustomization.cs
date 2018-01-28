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

        public void Init_TextBoxes()
        {
            for (int i = 0; i < 4; i++)
            {
                textBoxes[i].Text = Chats[ChatsTarget, i];
            }
        }

        public void UpdateText(ref string[] chats, Dictionary<Microsoft.Xna.Framework.Input.Buttons, string> toChange = null)
        {
            

            for (int i = 0; i < 4; i++)
            {
                Chats[ChatsTarget, i] = textBoxes[i].Text;
                if(toChange != null)
                {
                    toChange[toChange.ElementAt(i).Key] = textBoxes[i].Text;
                }
                
            }
            int k = 0;
            foreach (string s in Chats)
            {
                chats[k] = s;
                k++;
            }
            for (int i = 0; i < chats.Length; i++)
            {
                Console.WriteLine(chats[i]);
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
            ChatsTarget = Convert.ToInt16(b.Text) - 1;  //the value displayed on the button is not a valid array index since it starts at 1, so to compensate, 1 is subtracted
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

                button.Text = Convert.ToString(defaultQCSize + 1);
                button.Location = new Point(buttons[defaultQCSize - 2].Location.X, (buttons[defaultQCSize - 2].Location.Y) + 22);
                //extracts default values from preexisting button
                button.Size = buttons[0].Size;              
                button.BackColor = buttons[0].BackColor;
                button.ForeColor = buttons[0].ForeColor;
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
