using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;
using System.Threading;
namespace RLCustomChat
{
    public partial class Overlay : Form
    {
        public const string WINDOW_NAME = "Rocket League (32-bit, DX9)";

        IntPtr handle = FindWindow(null, WINDOW_NAME);
        RECT rect;

        public bool Draw = false;
        
        public string[] StrToDraw = new string[4];

        public struct RECT
        {
            public int left, top, right, bottom;
        }

        public System.Timers.Timer Timer;

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        public Overlay()
        {
            InitializeComponent();
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
            this.AllowTransparency = true;
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;


            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            GetWindowRect(handle, out rect);


            //this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            this.Size = new Size(1920, 1080);
            this.Top = rect.top;
            this.Left = rect.left;
            
        }

        public void TimeDrawing()
        {
            Timer = new System.Timers.Timer(2000);
            Timer.Elapsed += ((Object source, ElapsedEventArgs e) => { Draw = false; RefreshCurrent(); });
            Timer.AutoReset = false;
            Timer.Enabled = true;
        }

        private void RefreshCurrent()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(RefreshCurrent), new object[] { });
                return;
            }
            Refresh();
        }

        private void Overlay_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            Font DrawFont = new Font("Arial", 16);
            SolidBrush DrawBrush = new SolidBrush(Color.White);
            SolidBrush FillBrush = new SolidBrush(Color.Red);

            Image chatBox = Properties.Resources.ChatBox;

            g = e.Graphics;

            if(Draw)
            {
                g.DrawImage(chatBox, 10, 420);

                int loopCounterStr = 0;
                foreach (string s in StrToDraw)
                {
                    //Resets necessary variables before prefix check
                    string ToDraw = s;
                    DrawBrush.Color = Color.White;
                    DrawFont = new Font("Arial", 16);

                    int DrawX = 30;
                    int DrawY = 440 + loopCounterStr * 52;

                    //prefix check, modifies string and font color depending on presence of prefix
                    if(s.Length >= 3) {
                        if (s.Substring(0, 3).Equals("[y]"))
                        {
                            ToDraw = s.Replace("[y]", "(TEAM) ");
                            
                            DrawBrush.Color = Color.Green;
                        }
                    }


                    if (ToDraw.Length >= 28)
                    {
                        string TempToDraw = ToDraw.Substring(0, 28);
                        ToDraw = ToDraw.Substring(28);

                        int LastSpacePos = TempToDraw.LastIndexOf(' ');
                        TempToDraw = TempToDraw.Remove(LastSpacePos, 1).Insert(LastSpacePos, "\n");

                        ToDraw = TempToDraw + ToDraw;

                        DrawFont = new Font("Arial", 12);
                    }

                    g.DrawString(ToDraw, DrawFont, DrawBrush, DrawX, DrawY);
                    loopCounterStr++;
                }
                loopCounterStr = 0;
                TimeDrawing();
            }
            


            DrawFont.Dispose();
            DrawBrush.Dispose();
            g.Dispose();
        }
    }
}
