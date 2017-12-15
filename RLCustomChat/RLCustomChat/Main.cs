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
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
namespace RLCustomChat
{
    public partial class Main : Form
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // System.Windows.Forms.Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        
        public static Dictionary<Microsoft.Xna.Framework.Input.Buttons, string> insultMessages = new Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>();
        public static Dictionary<Microsoft.Xna.Framework.Input.Buttons, string> complimentMessages = new Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>();
        public static Dictionary<Microsoft.Xna.Framework.Input.Buttons, Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>> messageSets = new Dictionary<Microsoft.Xna.Framework.Input.Buttons, Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>>();
        public Microsoft.Xna.Framework.Input.Buttons currentMessageSet = Microsoft.Xna.Framework.Input.Buttons.DPadUp;
        Microsoft.Xna.Framework.Input.Buttons key = new Microsoft.Xna.Framework.Input.Buttons();

        Microsoft.Xna.Framework.Input.Buttons[] buttons =
        {Microsoft.Xna.Framework.Input.Buttons.DPadUp,
        Microsoft.Xna.Framework.Input.Buttons.DPadRight,
        Microsoft.Xna.Framework.Input.Buttons.DPadDown,
        Microsoft.Xna.Framework.Input.Buttons.DPadLeft};
        public string[,] chatSets = new string[4, 4];
        public int SLEEP_TIME = 75;

        public static int SendDelay = 0;
        public static bool RunThread = false;
        public static bool CheckForOverlayUpdate = true;
        Overlay overlay = new Overlay();
        QCCustomization chat = new QCCustomization();
        Thread MainThread = null;
        public string[] fileContentsOnLoad;
        public string[] UpdatedChatSet = null;
        int selected = 0;
        public void initChatSets(string filename)
        {

            string[] fileContents = File.ReadAllLines(filename);
            UpdatedChatSet = new string[20];
            fileContentsOnLoad = fileContents;
            int j = 0;
            for (int i = 0; i < fileContents.Length; i++)
            {
                if (j >= 4)
                {
                    j = 0;
                }

                if (fileContents[i].Length > 0)
                {
                    if (fileContents[i][0] == '#')
                    {
                        selected = Convert.ToInt16(Convert.ToInt16(fileContents[i].Substring(1)) - 1);
                        continue;
                    }
                }
                chatSets[selected, j] = fileContents[i];
                j++;
            }
        }

        public Main()
        {
            InitializeComponent();
            richTextBox2.Text = "" + SLEEP_TIME;
            MainThread = new Thread(ActivateChatFeature);
            MainThread.Start();

            initChatSets("C:/Users/sraze/Desktop/ChatSet1.txt");

            for (int i = 0; i < 4; i++)
            {
                messageSets.Add(buttons[i], new Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>());
                for (int j = 0; j < 4; j++)
                {
                    messageSets[buttons[i]].Add(buttons[j], chat.Chats[i, j] = chatSets[i, j]);

                }
            }
            for (int i = 0; i < chat.textBoxes.Length; i++)
            {
                chat.textBoxes[i].Text = chat.Chats[0, i];
            }
        }

        private void ActivateChatFeature()
        {
            
            RefreshChatCustomization();
            Dictionary<Microsoft.Xna.Framework.Input.Buttons, string> dict = new Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>();
            while (true)
            {
                
                
                if(CheckForOverlayUpdate)
                {
                    if (chat.RefreshOverlayCue)
                    {
                        CallTextUpdate();
                        for (int i = 0; i < dict.Count; i++)
                        {
                            KeyValuePair<Microsoft.Xna.Framework.Input.Buttons, string> pair = dict.ElementAt(i);
                            overlay.StrToDraw[i] = pair.Value;
                        }
                        RefreshOverlay();
                        chat.RefreshOverlayCue = false;
                    }
                }
                if(RunThread)
                {
                    GamePadState currentState = GamePad.GetState(PlayerIndex.One);
                    foreach (var i in messageSets)
                    {
                        if (currentState.IsConnected && currentState.IsButtonDown(i.Key))
                        {
                            if (!overlay.Draw)
                            {
                                currentMessageSet = i.Key;
                            }
                            else
                            {
                                overlay.Timer.Stop();
                                overlay.Timer.Start();
                                continue;
                            }


                        }
                        else
                        {
                            continue;
                        }



                        dict = messageSets[currentMessageSet];

                        overlay.StrToDraw = new string[4];

                        for (int j = 0; j < dict.Count; j++)
                        {
                            KeyValuePair<Microsoft.Xna.Framework.Input.Buttons, string> pair = dict.ElementAt(j);
                            overlay.StrToDraw[j] = pair.Value;
                        }
                        overlay.Draw = true;
                        RefreshOverlay();
                        Thread.Sleep(300);

                    }
                    currentState = GamePad.GetState(PlayerIndex.One);
                    foreach (var i in dict)
                    {
                        if (overlay.Draw)
                        {
                            Microsoft.Xna.Framework.Input.Buttons iterKey = i.Key;
                            if (currentState.IsConnected && currentState.IsButtonDown(iterKey))
                            {
                                AlterTextBox("" + i.Key);
                                key = i.Key;
                                break;
                            }
                            key = 0;
                        }



                    }
                    if (key != 0)
                    {
                        SendDelay++;
                        if (SendDelay == 1)
                        {
                            SendKeys.SendWait("t");
                            Thread.Sleep(SLEEP_TIME);
                            SendKeys.SendWait(dict[key] + "{ENTER}");
                        }
                    }
                    else
                    {
                        SendDelay = 0;
                    }


                }
                Thread.Sleep(1);
            }
                


        }
        public void UpdateChatSetsFile(string filename)
        {
            if (UpdatedChatSet != null)
            {
                int j = 0;
                for (int i = 0; i < fileContentsOnLoad.Length; i++)
                {
                    if (fileContentsOnLoad[i].Length > 0)
                    {
                        if (fileContentsOnLoad[i][0] == '#')
                        {
                            continue;
                        }
                    }
                    fileContentsOnLoad[i] = UpdatedChatSet[j];
                    j++;
                }

            }

            File.WriteAllLines(filename, fileContentsOnLoad);

        }
        private void CallTextUpdate()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(CallTextUpdate), new object[] { });
                return;
            }
            if(currentMessageSet != 0)
            {
                chat.UpdateText(ref UpdatedChatSet, messageSets[buttons[chat.ChatsTarget]]);
                UpdateChatSetsFile("C:/Users/sraze/Desktop/ChatSet1.txt");
            }
            
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
        private void RefreshOverlay()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(RefreshOverlay), new object[] { });
                return;
            }
            overlay.Refresh();
        }
        private void RefreshChatCustomization()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(RefreshChatCustomization), new object[] { });
                return;
            }
            chat.Refresh();
        }
        private void AlterTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AlterTextBox), new object[] { value });
                return;
            }
            richTextBox1.Clear();
            richTextBox1.Text += value;
        }

        private void OverlayState_CheckedChanged(object sender, EventArgs e)
        {
            if (OverlayState.Checked)
            {
                if (!RunThread)
                {
                    RunThread = true;
                }
                overlay.Show();

            }
            else
            {
                richTextBox1.Clear();
                RunThread = false;
                overlay.Hide();
            }
        }

        private void CustomizeChat_Click(object sender, EventArgs e)
        {
            chat.Show();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MainThread != null)
            {
                MainThread.Abort();
            }
            
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SLEEP_TIME = Convert.ToInt16(richTextBox2.Text);
        }
    }
}
