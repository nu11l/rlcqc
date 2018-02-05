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
using System.Diagnostics;

namespace RLCustomChat
{
    public partial class Main : Form
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // System.Windows.Forms.Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

 
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
        public string[] UpdatedChatSet;

        int selected = 0;

        VAMemory Mem;
        public const string PROCESS_NAME = "RocketLeague";
        int[] boost_offset = { 0xEC, 0x500, 0x3DC, 0x554, 0x54 };
        public static int BaseAddress;
        public static int TrainingAddress;


        int FindMultiLevelPointer(long Base, int[] offsets)
        {
            int prev = (int)Base;
            int cur;
            for (int i = 0; i < offsets.Length - 1; i++)
            {
                cur = Mem.ReadInt32((IntPtr)prev + offsets[i]);
                prev = cur;
            }
            int final_offset = offsets[offsets.Length - 1];
            int returned_address = prev + final_offset;
            if (returned_address <= 100)
            {
                return -1;
            }
            return (prev + final_offset);
        }

        public static int GetModule(String ModuleName)
        {
            try
            {
                Process[] p = Process.GetProcessesByName(PROCESS_NAME);
                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if (m.ModuleName.Equals(ModuleName))
                        {
                            Console.WriteLine("Found!");
                            return (int)m.BaseAddress;
                        }
                    }
                    return 0;
                }
                else
                {
                    Console.WriteLine("Did not find!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }



        public delegate void FuncThreadCall();

        public void CallFunc(FuncThreadCall func)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<FuncThreadCall>(CallFunc), new object[] { func });
                return;
            }
            func();
        }


        public void initChatSets(string filename)
        {

            string[] fileContents = File.ReadAllLines(filename);
            UpdatedChatSet = new string[fileContents.Length];

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

            DelaySet.Text = "" + SLEEP_TIME;

            Mem = new VAMemory(PROCESS_NAME);

            MainThread = new Thread(ActivateChatFeature);
            MainThread.Start();

            initChatSets("ChatSet1.txt");

            for (int i = 0; i < 4; i++)
            {
                messageSets.Add(buttons[i], new Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>());
                for (int j = 0; j < 4; j++)
                {
                    messageSets[buttons[i]].Add(buttons[j], chat.Chats[i, j] = chatSets[i, j]);

                }
            }
  
            chat.Init_TextBoxes();
        }

        private void ActivateChatFeature()
        {

            //RefreshChatCustomization();
            CallFunc(chat.Refresh);
            Dictionary<Microsoft.Xna.Framework.Input.Buttons, string> dict = new Dictionary<Microsoft.Xna.Framework.Input.Buttons, string>();
            BaseAddress = GetModule("RocketLeague.exe");
            int[] ChatEnabledOffsets = { 0xF8, 0x1C0, 0x10, 0x78, 0x414 };
            int[] InMenuOffsets = { 0x10, 0xC, 0x0, 0x614, 0x718 };
            if (BaseAddress != 0)
            {
                while (true)
                {
                    //checks whether or not the player is in a menu
                    uint InMenuBase = Mem.ReadUInteger((IntPtr)BaseAddress + 0x019FA7E8);
                    int InMenuAddress = FindMultiLevelPointer(InMenuBase, InMenuOffsets);
                    int InMenu = Mem.ReadInt32((IntPtr)InMenuAddress);

                    //checks whether or not the quick chat is enabled
                    uint ChatEnabledBase = Mem.ReadUInteger((IntPtr)BaseAddress + 0x01962610);
                    int ChatEnabledAddress = FindMultiLevelPointer(ChatEnabledBase, ChatEnabledOffsets);
                    int ChatEnabled = Mem.ReadInt32((IntPtr)ChatEnabledAddress);

                    if (CheckForOverlayUpdate)
                    {
                        if (chat.RefreshOverlayCue)
                        {
                            CallTextUpdate();
                            for (int i = 0; i < dict.Count; i++)
                            {
                                KeyValuePair<Microsoft.Xna.Framework.Input.Buttons, string> pair = dict.ElementAt(i);
                                overlay.StrToDraw[i] = pair.Value;
                            }
                            CallFunc(overlay.Refresh);
                            chat.RefreshOverlayCue = false;
                        }
                    }
                    if (RunThread && (InMenu == 0 && (ChatEnabled == 1 || ChatEnabled == 25 || ChatEnabled == 27)))
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
                            CallFunc(overlay.Refresh);
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
                                //checks for existence of [y] preceding message - if this yields true, it will send a 'y' to open team chat
                                bool TeamChatPrefixExists = (dict[key].Substring(0, 3).Equals("[y]"));

                                SendKeys.SendWait((TeamChatPrefixExists) ? "y" : "t");
                                Thread.Sleep(SLEEP_TIME);
                                SendKeys.SendWait(((TeamChatPrefixExists) ? dict[key].Substring(3) : dict[key]) + "{ENTER}"); //if [y] prefix exists, it is excluded from the main message being sent
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
                UpdateChatSetsFile("ChatSet1.txt");
            }
            
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

        private void SetDelay(object sender, EventArgs e)
        {
            SLEEP_TIME = Convert.ToInt16(DelaySet.Text);
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }
    }
}
