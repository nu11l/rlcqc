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

        public static int SendDelay = 0;
        public static bool RunThread = false;
        public static bool CheckForOverlayUpdate = true;
        Overlay overlay = new Overlay();
        QCCustomization chat = new QCCustomization();
        Thread MainThread = null;

        public Main()
        {
            InitializeComponent();

            MainThread = new Thread(ActivateChatFeature);
            MainThread.Start();

            //Below are predefined message sets. These may be altered/changed with the "Customize Chat" button.
            insultMessages.Add(buttons[0], chat.Chats[0, 0] = "I had it!");
            insultMessages.Add(buttons[1], chat.Chats[0, 1] = "You stole my boost.");
            insultMessages.Add(buttons[2], chat.Chats[0, 2] = "What are you doing?");
            insultMessages.Add(buttons[3], chat.Chats[0, 3] = "How are you in this rank?");
            for(int i = 0; i < chat.textBoxes.Length; i++)
            {
                chat.textBoxes[i].Text = chat.Chats[0, i];
            }
            complimentMessages.Add(buttons[0], chat.Chats[1, 0] = "Great shot!");
            complimentMessages.Add(buttons[1], chat.Chats[1, 1] = "Nice pass!");
            complimentMessages.Add(buttons[2], chat.Chats[1, 2] = "Good clear!");
            complimentMessages.Add(buttons[3], chat.Chats[1, 3] = "Good defense!");

            messageSets.Add(buttons[0], insultMessages);
            messageSets.Add(buttons[1], complimentMessages);
            //Below are empty chat sets that can also be changed.
            for(int i = 2; i < 4; i++)
            {
                messageSets.Add(buttons[i], new Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>());
                for (int j = 0; j < 4; j++)
                {
                    messageSets[buttons[i]].Add(buttons[j], chat.Chats[i, j]);
                    
                }
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
                            Thread.Sleep(75);
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
        private void CallTextUpdate()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(CallTextUpdate), new object[] { });
                return;
            }
            if(currentMessageSet != 0)
            {
                chat.UpdateText(messageSets[buttons[chat.ChatsTarget]]);
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

        
    }
}
