using Microsoft.DirectX.AudioVideoPlayback;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WMPLib;

namespace Game
{
    public partial class Game : Form
    {
        readonly InGame Recorder=new InGame();
        readonly PictureBox ghost = new PictureBox();
        int i, turns = 1, time, dice_num, sec, min, y,x, p1station, p2station, p1stier, p2stier, p1rtier, p2rtier, p1health = 100, p2health = 100,
            p3station, p4station, p3stier, p4stier, p3rtier, p4rtier, p3health = 100, p4health = 100,steps=1,COM_Tick, Replay_Order=0;
        readonly int PlayerNum=4;
        readonly Random rand = new Random();
        readonly WindowsMediaPlayer HoverS = new WindowsMediaPlayer { URL = "Select.mp3" };
        readonly WindowsMediaPlayer SubOpen = new WindowsMediaPlayer { URL = "SubOpen.mp3" };
        readonly WindowsMediaPlayer Main = new WindowsMediaPlayer { URL = "Game.mp3" };
        readonly WindowsMediaPlayer Event = new WindowsMediaPlayer();
        readonly WindowsMediaPlayer Event1 = new WindowsMediaPlayer();
        readonly Cursor mycursor = new Cursor(Cursor.Current.Handle);
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string filename);
        Bitmap bmp;
        Graphics g;
        string ssec, smin;
        bool StandBy = true, rotated1 = false, rotated2 = false, rotated3 = false, rotated4 = false, Striker = false, WMD = false, Rotate_Chk = false;
        readonly Button[,] Tiles = new Button[4, 5];
        Button p1, p2,p3,p4;
        double p1S, p2S,p3S,p4S;

        private void Tutorial_Click(object sender, EventArgs e)
        {
            SubOpen.controls.stop();
            SubOpen.controls.play();
            Info.Visible = true;
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            ActiveControl = Squadron;
        }

        Video vid;
        public string P1N, P2N, P3N, P4N,P1F, P2F, P3F, P4F;
        public bool Override=false;
        public int GameNo;
        readonly List<string> BReplayScript = File.ReadAllLines("DB/InGame.txt").ToList();
        readonly List<string> ReplayScript = new List<string>();

        public Game()
        {
            InitializeComponent();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            foreach (string line in BReplayScript)
            {
                string[] partition = line.Split(',');
                if (GameNo == Convert.ToInt32(partition[0]))
                {
                    ReplayScript.Add(line);
                }
            }
            if (Override)
                Cursor.Hide();
            Thread.Sleep(15000);
            Main.controls.play();
            Main.settings.setMode("loop", true);
            Event.URL = "Voices/Start/Start" + rand.Next(1, 6) + ".mp3";
            Board.Size = this.Size;

            mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Cursor.cur") });
            this.Cursor = mycursor;
            tableLayoutPanel7.Size = tableLayoutPanel8.Size = P1S.Size = P2S.Size = P3S.Size = P4S.Size = Panel5.Size = Panel6.Size = Panel7.Size = Panel8.Size
           = S.Size = SU.Size = R.Size = RU.Size = Tile0.Size = D.Size = T.Size = P1.Size = P2.Size = P3.Size = P4.Size = P1HP.Size = P2HP.Size = P3HP.Size = P4HP.Size
           = Tile0.Size = Tile1.Size = Tile2.Size = Tile3.Size = Tile4.Size = Tile5.Size = Tile6.Size = Tile7.Size = Tile8.Size = Tile9.Size
           = Tile10.Size = Tile11.Size = Tile12.Size = Tile13.Size = Tile14.Size = Tile15.Size = Tile16.Size = Tile17.Size = Tile18.Size = Tile19.Size
           = panel1.Size = panel2.Size = panel3.Size = panel4.Size = Dice.Size
           = this.Size;

            //Fits the text in the table cell
            while (P1.Width < System.Windows.Forms.TextRenderer.MeasureText(P1.Text,
                new Font(P1.Font.FontFamily, P1.Font.Size, P1.Font.Style)).Width)
            {
                P1.Font = new Font(P1.Font.FontFamily, P1.Font.Size - 0.5f, P1.Font.Style);
            }
            while (P2.Width < System.Windows.Forms.TextRenderer.MeasureText(P2.Text,
                new Font(P2.Font.FontFamily, P2.Font.Size, P2.Font.Style)).Width)
            {
                P2.Font = new Font(P2.Font.FontFamily, P2.Font.Size - 0.5f, P2.Font.Style);
            } while (P3.Width < System.Windows.Forms.TextRenderer.MeasureText(P3.Text,
                 new Font(P3.Font.FontFamily, P3.Font.Size, P3.Font.Style)).Width)
            {
                P3.Font = new Font(P3.Font.FontFamily, P3.Font.Size - 0.5f, P3.Font.Style);
            } while (P4.Width < System.Windows.Forms.TextRenderer.MeasureText(P4.Text,
                 new Font(P4.Font.FontFamily, P4.Font.Size, P4.Font.Style)).Width)
            {
                P4.Font = new Font(P4.Font.FontFamily, P4.Font.Size - 0.5f, P4.Font.Style);
            }

            P1.Text = P1N; P2.Text = P2N; P3.Text = P3N; P4.Text = P4N;
            P1S.Text = "0"; P2S.Text = "0"; P3S.Text = "0"; P4S.Text = "0";
            Skins();
            Available();
            Sizer();
            P1.BackColor = Color.PaleGreen;

            //Paints the healthbars
            bmp = new Bitmap(P1HP.Width, P1HP.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p1health * P1HP.Width / 100.0), P1HP.Height));
            g.DrawString(p1health + "%", new Font("Arial", P1HP.Height / 2), Brushes.White, new PointF(P1HP.Width / 2 - P1HP.Height, P1HP.Height / 10));
            P1HP.Image = bmp;

            bmp = new Bitmap(P2HP.Width, P2HP.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p2health * P2HP.Width / 100.0), P2HP.Height));
            g.DrawString(p2health + "%", new Font("Arial", P2HP.Height / 2), Brushes.White, new PointF(P2HP.Width / 2 - P2HP.Height, P2HP.Height / 10));
            P2HP.Image = bmp;

            bmp = new Bitmap(P3HP.Width, P3HP.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p3health * P3HP.Width / 100.0), P3HP.Height));
            g.DrawString(p3health + "%", new Font("Arial", P3HP.Height / 2), Brushes.White, new PointF(P3HP.Width / 2 - P3HP.Height, P3HP.Height / 10));
            P3HP.Image = bmp;

            bmp = new Bitmap(P4HP.Width, P4HP.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p4health * P4HP.Width / 100.0), P4HP.Height));
            g.DrawString(p4health + "%", new Font("Arial", P4HP.Height / 2), Brushes.White, new PointF(P4HP.Width / 2 - P4HP.Height, P4HP.Height / 10));
            P4HP.Image = bmp;

            Dice.BackgroundImage = Image.FromFile("Images/Button/Released.png");
            R.BackgroundImage = Image.FromFile("Images/Tactical/Guerilla Repair.png");
            RU.BackgroundImage = Image.FromFile("Images/Tactical/Upgrade.png");
            SU.BackgroundImage = Image.FromFile("Images/Tactical/Upgrade.png");
            S.BackgroundImage = Image.FromFile("Images/Tactical/Strike.png");

            //Initializes the locations and the rotation
            Player1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            Player1.Location = new Point((int)(Tile0.Location.X + this.Size.Width * 0.01) - ((int)(Tile0.Location.X + this.Size.Width * 0.01)) % 5, (int)(Tile0.Location.Y + this.Size.Height * 0.01));

            Player2.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            Player2.Location = new Point((int)(Tile0.Location.X + this.Size.Width * 0.01), (int)(Tile0.Location.Y + Tile0.Size.Height - Player2.Size.Height - this.Size.Height * 0.01));

            Player3.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            Player3.Location = new Point((int)(Tile0.Location.X + this.Size.Width * 0.01 + Math.Max(Player3.Size.Width, Player4.Size.Width)), (int)(Tile0.Location.Y + this.Size.Height * 0.01));

            Player4.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            Player4.Location = new Point((int)(Tile0.Location.X + this.Size.Width * 0.01 + Math.Max(Player3.Size.Width, Player4.Size.Width)), (int)(Tile0.Location.Y + Tile0.Size.Height - Player2.Size.Height - this.Size.Height * 0.01));

            y = Player2.Location.Y - Player1.Location.Y;
            x = Math.Max(Player3.Location.X, Player4.Location.X) - Player1.Location.X;

            Menu_Container.Location = new Point(this.Location.X + Tile15.Location.X + Tile15.Size.Width, this.Location.Y + Tile15.Location.Y + Tile15.Size.Height);
            Menu_Container.Size = new Size(this.Width - (int)(Tile0.Width * 2.05), this.Height - (int)(Tile0.Height * 2));
            MenuS.Location = new Point(Menu_Container.Width / 2 - MenuS.Width / 2, Menu_Container.Height / 2 - MenuS.Height / 2);

            //Assigns the two dimensional array to the panel tiles
            Tiles[0, 0] = Tile0; Tiles[0, 1] = Tile1; Tiles[0, 2] = Tile2; Tiles[0, 3] = Tile3; Tiles[0, 4] = Tile4;
            Tiles[1, 0] = Tile5; Tiles[1, 1] = Tile6; Tiles[1, 2] = Tile7; Tiles[1, 3] = Tile8; Tiles[1, 4] = Tile9;
            Tiles[2, 0] = Tile10; Tiles[2, 1] = Tile11; Tiles[2, 2] = Tile12; Tiles[2, 3] = Tile13; Tiles[2, 4] = Tile14;
            Tiles[3, 0] = Tile15; Tiles[3, 1] = Tile16; Tiles[3, 2] = Tile17; Tiles[3, 3] = Tile18; Tiles[3, 4] = Tile19;

            //Sets the spawn locations for all the players
            p1 = Tiles[0, 0];
            p2 = Tiles[0, 0];
            p3 = Tiles[0, 0];
            p4 = Tiles[0, 0];

            //Initializes the video panel
            vid = new Video("Events/EMP.wmv") { Owner = Vid_Pnl };
            vid.Ending += new EventHandler(BackLoop);
            Vid_Container.Visible = false;
            Vid_Container.Location = new Point(this.Location.X + Tile15.Location.X + Tile15.Size.Width, this.Location.Y + Tile15.Location.Y + Tile15.Size.Height);
            Vid_Container.Size = new Size(this.Width - (int)(Tile0.Width * 2.05), this.Height - (int)(Tile0.Height * 2));
            Info.Location = new Point(this.Location.X + Tile15.Location.X + Tile15.Size.Width, this.Location.Y + Tile15.Location.Y + Tile15.Size.Height);
            Info.Size = new Size(this.Width - (int)(Tile0.Width * 2.05), this.Height - (int)(Tile0.Height * 2));

            Squadron.Location = new Point (this.Location.X - Squadron.Size.Width,(int)(this.Size.Height/2));

            COM_Move_Handler.Start();
        }

        private void BackLoop(object sender, EventArgs e)
        {
            //Hides the video when it finishes
            vid.Stop();
            Vid_Container.Visible = false;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            //Exit handler
            SubOpen.controls.stop();
            SubOpen.controls.play();
            Vid_Pnl.Enabled = true;
            vid.Dispose();
            Thread.Sleep(1000);
            Application.Exit();
        }

        private void Main_Menu_Click(object sender, EventArgs e)
        {
            //Mainmenu handler
            SubOpen.controls.stop();
            SubOpen.controls.play();
            Main.controls.stop();
            Thread Transition;
            Transition = new Thread(Runform);
            Transition.SetApartmentState(ApartmentState.STA);
            Transition.Start();
            vid.Dispose();
            Thread.Sleep(3000);
            this.Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //Timer handler
            D.Text = DateTime.Now.ToLongTimeString();
            sec++;
            if (sec == 59)
            {
                sec = 0;
                min++;
            }
            if (sec < 10)
                ssec = "0" + sec;
            else
                ssec = sec + "";
            if (min < 10)
                smin = "0" + min;
            else
                smin = min + "";
            T.Text = string.Format("{1} : {0}", ssec, smin);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            //Dice behaviours
            if (time < 25)
            {
                Dice.BackgroundImage = Image.FromFile("Images/Num/" + rand.Next(1, 7) + ".png");
            }
            if (time++ == 25)
            {
                if (!Override)
                    dice_num = rand.Next(1,7);
                else
                {
                    string[] partition = ReplayScript[Replay_Order].Split(',');
                    dice_num = Convert.ToInt32(partition[3]);
                    Replay_Order++;
                }
                Dice.BackgroundImage = Image.FromFile("Images/Num/" + dice_num + ".png");
                Dice.FlatAppearance.BorderColor = Color.PaleGreen;
                if(turns==1)
                    Recorder.Flush(P1.Text, "Manoeuvre", dice_num+"",0);
                else if (turns == 2)
                    Recorder.Flush(P2.Text, "Manoeuvre", dice_num + "",0);
                else if (turns == 3)
                    Recorder.Flush(P3.Text, "Manoeuvre", dice_num + "",0);
                else if (turns == 4)
                    Recorder.Flush(P4.Text, "Manoeuvre", dice_num + "",0);
                Moving.Start();
            }
            if (time == 35)
            {
                Dice.BackgroundImage = Image.FromFile("Images/Button/Released.png");
                timer2.Stop();
                StandBy = true;
                time = 0;
                Dice.FlatAppearance.BorderColor = Color.Black;
            }
        }

        private void ChkScr(object sender, EventArgs e)
        {
            //Changes the cursor when hovering on a support power depending on sets of conditions
            HoverS.controls.stop();
            HoverS.controls.play();
            if (!Striker && !WMD)
            {
                mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Select.ani") });
                if (turns == 1)
                {
                    if (sender == S && p1S < 700)
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == SU && (p1S < 1800 || p1stier==2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == R && (p1S < 300 || p1health==100))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == RU && (p1S < 1200 || p1rtier==2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                }
                else if (turns == 2)
                {
                    if (sender == S && p2S < 700)
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == SU && (p2S < 1800 || p2stier==2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == R && (p2S < 300 || p2health == 100))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == RU && (p2S < 1200 || p2rtier==2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                }
                else if (turns == 3)
                {
                    if (sender == S && p3S < 700)
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == SU && (p3S < 1800 || p3stier == 2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == R && (p3S < 300 || p3health == 100))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == RU && (p3S < 1200 || p3rtier == 2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                }
                else if (turns == 4)
                {
                    if (sender == S && p4S < 700)
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == SU && (p4S < 1800 || p4stier == 2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == R && (p4S < 300 || p4health == 100))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                    else if (sender == RU && (p4S < 1200 || p4rtier == 2))
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Deny.cur") });
                }
                this.Cursor = mycursor;
            }
        }

        private void Action(object sender, EventArgs e)
        {
            //Handles validated support power clicks
            SubOpen.controls.stop();
            SubOpen.controls.play();
            if (!Moving.Enabled && !Striker && !WMD && StandBy)
            {
                if (turns == 1 && P1.Text != "COM")
                {
                    if (sender == S && p1S >= 700)
                    {
                        p1S -= 700;
                        P1S.Text = "" + Convert.ToInt32(p1S);
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                        this.Cursor = mycursor;
                        Striker = true;
                    }
                    else if (sender == SU && p1S >= 1800 && p1stier!=2)
                    {
                        p1S -= 1800;
                        P1S.Text = "" + Convert.ToInt32(p1S);
                            Vid_Container.Visible = true;
                            vid.Open("Events/SUp.wmv");
                            vid.Play();
                            Event.URL = "Events/SUp.mp3";
                        if (p1stier == 0)
                        {
                            Event1.URL = "Voices/Strike/Strike2.mp3";
                        }
                        else if (p1stier == 1)
                        {
                            Event1.URL = "Voices/Strike/Strike3.mp3";
                        }
                        p1stier++;
                        p1station += 2;
                        Recorder.Flush(P1.Text, "Strike Upgrade",p1stier+1+"",0);
                        Iterator();
                    }
                    else if (sender == R && p1S >= 300 && p1health != 100)
                    {
                        p1health += (p1rtier + 1) * 20;
                        if (p1health > 100)
                            p1health = 100;
                        bmp = new Bitmap(P1HP.Width, P1HP.Height);
                        g = Graphics.FromImage(bmp);
                        g.Clear(Color.Black);
                        g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p1health * P1HP.Width / 100.0), P1HP.Height));
                        g.DrawString(p1health + "%", new Font("Arial", P1HP.Height / 2), Brushes.White, new PointF(P1HP.Width / 2 - P1HP.Height, P1HP.Height / 10));
                        P1HP.Image = bmp;

                        p1S -= 300;
                        P1S.Text = "" + Convert.ToInt32(p1S);
                        Event.URL = "Events/Repair_Ability.mp3";
                        Recorder.Flush(P1.Text, "Repair",(p1rtier + 1) * 20+"",0);
                        Iterator();
                    }
                    else if (sender == RU && p1S >= 1200 && p1rtier != 2)
                    {
                        p1S -= 1200;
                        P1S.Text = "" + Convert.ToInt32(p1S);
                        p1rtier++;
                        p1station++;
                        Recorder.Flush(P1.Text, "Repair Upgrade",(p1rtier+1)+"",0);
                        Iterator();
                    }
                }



                else if (turns == 2 && P2.Text != "COM")
                {
                    if (sender == S && p2S >= 700)
                    {
                        p2S -= 700;
                        P2S.Text = "" + Convert.ToInt32(p2S);
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                        this.Cursor = mycursor;
                        Striker = true;
                    }
                    else if (sender == SU && p2S >= 1800 && p2stier != 2)
                    {
                        p2S -= 1800;
                        P2S.Text = "" + Convert.ToInt32(p2S);
                        Vid_Container.Visible = true;
                        vid.Open("Events/SUp.wmv");
                        vid.Play();
                        Event.URL = "Events/SUp.mp3";
                        if (p2stier == 0)
                        {
                            Event1.URL = "Voices/Strike/Strike2.mp3";
                        }
                        else if (p2stier == 1)
                        {
                            Event1.URL = "Voices/Strike/Strike3.mp3";
                        }
                        p2stier++;
                        p2station += 2;
                        Recorder.Flush(P2.Text, "Strike Upgrade",p2stier+1+"",0);
                        Iterator();
                    }
                    else if (sender == R && p2S >= 300 && p2health != 100)
                    {
                        p2health += (p2rtier + 1) * 20;
                        if (p2health > 100)
                            p2health = 100;
                        bmp = new Bitmap(P2HP.Width, P2HP.Height);
                        g = Graphics.FromImage(bmp);
                        g.Clear(Color.Black);
                        g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p2health * P2HP.Width / 100.0), P2HP.Height));
                        g.DrawString(p2health + "%", new Font("Arial", P2HP.Height / 2), Brushes.White, new PointF(P2HP.Width / 2 - P2HP.Height, P2HP.Height / 10));
                        P2HP.Image = bmp;

                        p2S -= 300;
                        P2S.Text = "" + Convert.ToInt32(p2S);
                        Event.URL = "Events/Repair_Ability.mp3";
                        Recorder.Flush(P2.Text, "Repair",(p2rtier+1)*20+"",0);
                        Iterator();
                    }
                    else if (sender == RU && p2S >= 1200 && p2rtier != 2)
                    {
                        p2S -= 1200;
                        P2S.Text = "" + Convert.ToInt32(p2S);
                        p2rtier++;
                        p2station++;
                        Recorder.Flush(P2.Text, "Repair Upgrade",(p2rtier+1)+"",0);                        
                        Iterator();
                    }
                }



                else if (turns == 3 && P3.Text != "COM")
                {
                    if (sender == S && p3S >= 700)
                    {
                        p3S -= 700;
                        P3S.Text = "" + Convert.ToInt32(p3S);
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                        this.Cursor = mycursor;
                        Striker = true;
                    }
                    else if (sender == SU && p3S >= 1800 && p3stier != 2)
                    {
                        p3S -= 1800;
                        P3S.Text = "" + Convert.ToInt32(p3S);
                        Vid_Container.Visible = true;
                        vid.Open("Events/SUp.wmv");
                        vid.Play();
                        Event.URL = "Events/SUp.mp3";
                        if (p3stier == 0)
                        {
                            Event1.URL = "Voices/Strike/Strike2.mp3";
                        }
                        else if (p3stier == 1)
                        {
                            Event1.URL = "Voices/Strike/Strike3.mp3";
                        }
                        p3stier++;
                        p3station += 2;
                        Recorder.Flush(P3.Text, "Strike Upgrade",p3stier+1+"",0);
                        Iterator();
                    }
                    else if (sender == R && p3S >= 300 && p3health != 100)
                    {
                        p3health += (p3rtier + 1) * 20;
                        if (p3health > 100)
                            p3health = 100;
                        bmp = new Bitmap(P3HP.Width, P3HP.Height);
                        g = Graphics.FromImage(bmp);
                        g.Clear(Color.Black);
                        g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p3health * P3HP.Width / 100.0), P3HP.Height));
                        g.DrawString(p3health + "%", new Font("Arial", P3HP.Height / 2), Brushes.White, new PointF(P3HP.Width / 2 - P3HP.Height, P3HP.Height / 10));
                        P3HP.Image = bmp;

                        p3S -= 300;
                        P3S.Text = "" + Convert.ToInt32(p3S);
                        Event.URL = "Events/Repair_Ability.mp3";
                        Recorder.Flush(P3.Text, "Repair",(p3rtier+1)*20+"",0);
                        Iterator();
                    }
                    else if (sender == RU && p3S >= 1200 && p3rtier != 2)
                    {
                        p3S -= 1200;
                        P3S.Text = "" + Convert.ToInt32(p3S);
                        p3rtier++;
                        p3station++;
                        Recorder.Flush(P3.Text, "Repair Upgrade",(p3rtier+1)+"",0);
                        Iterator();
                    }
                }



                else if (turns == 4 && P4.Text != "COM")
                {
                    if (sender == S && p4S >= 700)
                    {
                        p4S -= 700;
                        P4S.Text = "" + Convert.ToInt32(p4S);
                        mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                        this.Cursor = mycursor;
                        Striker = true;
                    }
                    else if (sender == SU && p4S >= 1800 && p4stier != 2)
                    {
                        p4S -= 1800;
                        P4S.Text = "" + Convert.ToInt32(p4S);
                        Vid_Container.Visible = true;
                        vid.Open("Events/SUp.wmv");
                        vid.Play();
                        Event.URL = "Events/SUp.mp3";
                        if (p4stier == 0)
                        {
                            Event1.URL = "Voices/Strike/Strike2.mp3";
                        }
                        else if (p4stier == 1)
                        {
                            Event1.URL = "Voices/Strike/Strike3.mp3";
                        }
                        p4stier++;
                        p4station += 2;
                        Recorder.Flush(P4.Text, "Strike Upgrade",p4stier+1+"",0);
                        Iterator();
                    }
                    else if (sender == R && p4S >= 300 && p4health != 100)
                    {
                        p4health += (p4rtier + 1) * 20;
                        if (p4health > 100)
                            p4health = 100;
                        bmp = new Bitmap(P4HP.Width, P4HP.Height);
                        g = Graphics.FromImage(bmp);
                        g.Clear(Color.Black);
                        g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p4health * P4HP.Width / 100.0), P4HP.Height));
                        g.DrawString(p4health + "%", new Font("Arial", P4HP.Height / 2), Brushes.White, new PointF(P4HP.Width / 2 - P4HP.Height, P4HP.Height / 10));
                        P4HP.Image = bmp;

                        p4S -= 300;
                        P4S.Text = "" + Convert.ToInt32(p4S);
                        Event.URL = "Events/Repair_Ability.mp3";
                        Recorder.Flush(P4.Text, "Repair",(p4rtier+ 1)*20+"",0);
                        Iterator();
                    }
                    else if (sender == RU && p4S >= 1200 && p4rtier != 2)
                    {
                        p4S -= 1200;
                        P4S.Text = "" + Convert.ToInt32(p4S);
                        p4rtier++;
                        p4station++;
                        Recorder.Flush(P4.Text, "Repair Upgrade",(p4rtier+1)+"",0);
                        Iterator();
                    }
                }
            }
        }

        private void Strike(object sender, EventArgs e)
        {
            //Handles clicking on players
            PictureBox Sender_ = (PictureBox)sender;
            if (Striker)
            {
                if (turns == 1 && Sender_.Name == Player1.Name || turns == 2 && Sender_.Name == Player2.Name || turns == 3 && Sender_.Name == Player3.Name || turns == 4 && Sender_.Name == Player4.Name)
                    Event.URL = "Voices/Revoke/Deny" + rand.Next(1, 6) + ".mp3";
                else
                {
                    mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Cursor.cur") });
                    this.Cursor = mycursor;
                    Squadron.Location = new Point(this.Location.X - Squadron.Size.Width, (int)(this.Size.Height / 2));
                    Raid.Start();
                    if (turns == 1)
                    {
                        Prey(sender, (p1stier * 15) + 30);
                        Event.URL = "Events/Strike" + (p1stier + 1) + ".mp3";
                        vid.Open("Events/Strike" + (p1stier + 1) + ".wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        p1S += 300;
                        P1S.Text = "" + Convert.ToInt32(p1S);
                        int SRecord=0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if(sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P1.Text, "Surgical Strike", -(p1stier * 15 + 30) + "",SRecord);
                    }
                    else if (turns == 2)
                    {
                        Prey(sender, (p2stier * 15) + 30);
                        Event.URL = "Events/Strike" + (p2stier + 1) + ".mp3";
                        vid.Open("Events/Strike" + (p2stier + 1) + ".wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        p2S += 300;
                        P2S.Text = "" + Convert.ToInt32(p2S);
                        int SRecord = 0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if (sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P2.Text, "Surgical Strike",-(p2stier*15+30)+"",SRecord);
                    }
                    else if (turns == 3)
                    {
                        Prey(sender, (p3stier * 15) + 30);
                        Event.URL = "Events/Strike" + (p3stier + 1) + ".mp3";
                        vid.Open("Events/Strike" + (p3stier + 1) + ".wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        p3S += 300;
                        P3S.Text = "" + Convert.ToInt32(p3S);
                        int SRecord = 0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if (sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P3.Text, "Surgical Strike",-(p3stier*15+30)+"",SRecord);
                    }
                    else if (turns == 4)
                    {
                        Prey(sender, (p4stier * 15) + 30);
                        Event.URL = "Events/Strike" + (p4stier + 1) + ".mp3";
                        vid.Open("Events/Strike" + (p4stier + 1) + ".wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        p4S += 300;
                        P4S.Text = "" + Convert.ToInt32(p4S);
                        int SRecord = 0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if (sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P3.Text, "Surgical Strike",-(p3stier*15+30)+"",SRecord);
                    }
                    CheckWin();
                    Iterator();
                    Striker = false;
                }
            }

            if (WMD && !vid.Playing)
            {
                if (turns == 1 && Sender_.Name == Player1.Name || turns == 2 && Sender_.Name == Player2.Name || turns == 3 && Sender_.Name == Player3.Name || turns == 4 && Sender_.Name == Player4.Name)
                    Event.URL = "Voices/Revoke/Deny" + rand.Next(1, 6) + ".mp3";
                else
                {
                    mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Cursor.cur") });
                    this.Cursor = mycursor;
                    if (turns == 1)
                    {
                        Prey(sender, 100);
                        Event.URL = "Voices/WMD/Fire.mp3";
                        Event1.URL = "Events/Bay.mp3";
                        vid.Open("Events/Bay.wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        P1S.Text = "" + Convert.ToInt32(p1S);
                        int SRecord = 0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if (sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P1.Text, "WMD Strike",-100+"",SRecord);
                    }
                    else if (turns == 2)
                    {
                        Prey(sender, 100);
                        Event.URL = "Voices/WMD/Fire.mp3";
                        Event1.URL = "Events/Bay.mp3";
                        vid.Open("Events/Bay.wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        P2S.Text = "" + Convert.ToInt32(p2S);
                        int SRecord = 0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if (sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P2.Text, "WMD Strike",-100+"",SRecord);
                    }
                    else if (turns == 3)
                    {
                        Prey(sender, 100);
                        Event.URL = "Voices/WMD/Fire.mp3";
                        Event1.URL = "Events/Bay.mp3";
                        vid.Open("Events/Bay.wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        P3S.Text = "" + Convert.ToInt32(p3S);
                        int SRecord = 0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if (sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P3.Text, "WMD Strike",-100+"",SRecord);
                    }
                    else if (turns == 4)
                    {
                        Prey(sender, 100);
                        Event.URL = "Voices/WMD/Fire.mp3";
                        Event1.URL = "Events/Bay.mp3";
                        vid.Open("Events/Bay.wmv");
                        Vid_Container.Visible = true;
                        vid.Play();
                        P4S.Text = "" + Convert.ToInt32(p4S);
                        int SRecord = 0;
                        if (sender == Player1)
                            SRecord = 1;
                        else if (sender == Player2)
                            SRecord = 2;
                        else if (sender == Player3)
                            SRecord = 3;
                        else if (sender == Player4)
                            SRecord = 4;
                        Recorder.Flush(P4.Text, "WMD Strike",-100+"",SRecord);
                    }
                    WMD = false;
                    CheckWin();
                    Iterator();
                }
            }
        }

        private void Raid_Tick(object sender, EventArgs e)
        {
            //A puny animation of a squadron
            if (!vid.Playing)
            {
                Squadron.Location = new Point(Squadron.Location.X + 40, Squadron.Location.Y);
                if (Squadron.Location.X > this.Size.Width)
                {
                    Squadron.Location = new Point(this.Location.X - Squadron.Size.Width, (int)(this.Size.Height / 2));
                    Raid.Stop();
                }
            }
        }

        private void Moving_Tick(object sender, EventArgs e)
        {
            //Manages moving and ghost location
            Dice.Enabled = false;
            if (turns == 1)
            {
                ghost.Location = Player1.Location;
                ghost.Size = Player1.Size;
                ghost.Visible = false;
                MoveHelper(Player1, 0, 0, p1);
            }
            else if (turns == 2)
            {
                ghost.Location = new Point(Player2.Location.X, Player2.Location.Y - y);
                ghost.Size = Player2.Size;
                ghost.Visible = false;
                MoveHelper(Player2, 0, y, p2);
            }
            else if (turns == 3)
            {
                ghost.Location = new Point(Player3.Location.X - x, Player3.Location.Y);
                ghost.Size = Player3.Size;
                ghost.Visible = false;
                MoveHelper(Player3, x, 0, p3);
            }
            else if (turns == 4)
            {
                ghost.Location = new Point(Player4.Location.X - x, Player4.Location.Y - y);
                ghost.Size = Player4.Size;
                ghost.Visible = false;
                MoveHelper(Player4, x, y, p4);
            }
        }

        private void Dice_Click(object sender, EventArgs e)
        {
            if (StandBy && !Moving.Enabled && !Striker && !WMD)
            {
                SubOpen.controls.stop();
                SubOpen.controls.play();
                Thread.Sleep(200);
                timer2.Start();
                StandBy = false;
            }
        }

        private void Dice_MouseDown(object sender, MouseEventArgs e)
        {
            if (StandBy)
                Dice.BackgroundImage = Image.FromFile("Images/Button/Pressed.png");
        }

        private void Dice_MouseUp(object sender, MouseEventArgs e)
        {
            if (StandBy)
                Dice.BackgroundImage = Image.FromFile("Images/Button/Released.png");
        }

        void Runform(object obj)
        {
            //Return from the game to mainmenu
            if (((turns == 1 && p2health == 0 && p3health == 0 && p4health == 0) || (turns == 2 && p1health == 0 && p3health == 0 && p4health == 0)
             || (turns == 3 && p1health == 0 && p2health == 0 && p4health == 0) || (turns == 4 && p1health == 0 && p2health == 0 && p3health == 0)) && !Override)
                Application.Run(new Menu(){isWin = true});
            else
                Application.Run(new Menu());
        }

        private void Hover(object sender, EventArgs e)
        {
            if (!Striker && !WMD)
            {
            HoverS.controls.stop();
            HoverS.controls.play();
            mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Select.ani") });
            this.Cursor = mycursor;
            }
        }

        private void Default_Cursor(object sender, EventArgs e)
        {
            if(!Striker && !WMD)
            mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Cursor.cur") });
            this.Cursor = mycursor;
        }

        private void Min(object sender, KeyEventArgs e)
        {
            //Hanles minimizing the game
            if (e.KeyCode == Keys.RWin || e.KeyCode == Keys.LWin)
            {
                Main.controls.pause();
                this.WindowState = FormWindowState.Minimized;
            }
            if (e.KeyCode == Keys.Escape)
            {
                SubOpen.controls.stop();
                SubOpen.controls.play();
                if (!Info.Visible)
                {
                    if (Menu_Container.Visible == false)
                    {
                        if (Override)
                            Cursor.Show();
                        Menu_Container.Visible = true;
                        S.Enabled = false;
                        SU.Enabled = false;
                        R.Enabled = false;
                        RU.Enabled = false;
                        Dice.Enabled = false;
                    }
                    else
                    {
                        if (Override)
                            Cursor.Hide();
                        Menu_Container.Visible = false;
                        S.Enabled = true;
                        SU.Enabled = true;
                        R.Enabled = true;
                        RU.Enabled = true;
                        Dice.Enabled = true;
                    }
                }
                else
                    Info.Visible = false;
            }
        }

        private void Restore(object sender, EventArgs e)
        {
            //Handles maximizing the game
            if (i++ > 3 && this.WindowState == FormWindowState.Maximized)
                Main.controls.play();
        }

        public void MoveHelper(PictureBox a, int x, int y, Button p)
        {
            //Moves the ghost (which is the player is referenced to it) and increment the player score
            if (p == Tiles[0, 0] || p == Tiles[0, 1] || p == Tiles[0, 2] || p == Tiles[0, 3] || p == Tiles[0, 4])
            {
                ghost.Location = new Point(ghost.Location.X + 5, ghost.Location.Y);
                a.Location = new Point(ghost.Location.X + x, ghost.Location.Y + y);

                if (turns == 1)
                    p1S += 2000.0 / this.Size.Width;
                else if (turns == 2)
                    p2S += 2000.0 / this.Size.Width;
                else if (turns == 3)
                    p3S += 2000.0 / this.Size.Width;
                else if (turns == 4)
                    p4S += 2000.0 / this.Size.Width;
            }
            else
            if (p == Tiles[1, 0] || p == Tiles[1, 1] || p == Tiles[1, 2] || p == Tiles[1, 3] || p == Tiles[1, 4])
            {
                ghost.Location = new Point(ghost.Location.X, ghost.Location.Y - 5);
                a.Location = new Point(ghost.Location.X + x, ghost.Location.Y + y);

                if (turns == 1)
                    p1S += 2000.0 / this.Size.Height;
                else if (turns == 2)
                    p2S += 2000.0 / this.Size.Height;
                else if (turns == 3)
                    p3S += 2000.0 / this.Size.Height;
                else if (turns == 4)
                    p4S += 2000.0 / this.Size.Height;
            }
            else
            if (p == Tiles[2, 0] || p == Tiles[2, 1] || p == Tiles[2, 2] || p == Tiles[2, 3] || p == Tiles[2, 4])
            {
                ghost.Location = new Point(ghost.Location.X - 5, ghost.Location.Y);
                a.Location = new Point(ghost.Location.X + x, ghost.Location.Y + y);

                if (turns == 1)
                    p1S += 2000.0 / this.Size.Width;
                else if (turns == 2)
                    p2S += 2000.0 / this.Size.Width;
                else if (turns == 3)
                    p3S += 2000.0 / this.Size.Width;
                else if (turns == 4)
                    p4S += 2000.0 / this.Size.Width;
            }
            else
            if (p == Tiles[3, 0] || p == Tiles[3, 1] || p == Tiles[3, 2] || p == Tiles[3, 3] || p == Tiles[3, 4])
            {
                ghost.Location = new Point(ghost.Location.X, ghost.Location.Y + 5);
                a.Location = new Point(ghost.Location.X + x, ghost.Location.Y + y);

                if (turns == 1)
                    p1S += 2000.0 / this.Size.Height;
                else if (turns == 2)
                    p2S += 2000.0 / this.Size.Height;
                else if (turns == 3)
                    p3S += 2000.0 / this.Size.Height;
                else if (turns == 4)
                    p4S += 2000.0 / this.Size.Height;
            }

            //Displays the updated player score
            P1S.Text = "" + Convert.ToInt32(p1S);
            P2S.Text = "" + Convert.ToInt32(p2S);
            P3S.Text = "" + Convert.ToInt32(p3S);
            P4S.Text = "" + Convert.ToInt32(p4S);

            Decrementer_rotator(Tile0, a); Decrementer(Tile1, a); Decrementer(Tile2, a); Decrementer(Tile3, a); Decrementer(Tile4, a);
            Decrementer_rotator(Tile5, a); Decrementer(Tile6, a); Decrementer(Tile7, a); Decrementer(Tile8, a); Decrementer(Tile9, a);
            Decrementer_rotator(Tile10, a); Decrementer(Tile11, a); Decrementer(Tile12, a); Decrementer(Tile13, a); Decrementer(Tile14, a);
            Decrementer_rotator(Tile15, a); Decrementer(Tile16, a); Decrementer(Tile17, a); Decrementer(Tile18, a); Decrementer(Tile19, a);

            if (dice_num == 0)
            {
                //Detect if the player stopped at a special tile
                EventDetector(a);
                Moving.Stop();
                Dice.Enabled = true;

                //The iterator will not be called at the start point for now
                if ((turns==1 && p1 != Tiles[0,0]) || (turns == 2 && p2 != Tiles[0, 0]) || (turns == 3 && p3 != Tiles[0, 0]) || (turns == 4 && p4 != Tiles[0, 0]))
                Iterator();
            }
        }

        public void Decrementer(Button b, PictureBox a)
        {
            //Detects if the player have completed crossing a tile to decremet a the dice number
            if ((int)(ghost.Location.X - (this.Size.Width * 0.01)) - (int)(ghost.Location.X - (this.Size.Width * 0.01)) % 5 == (int)(b.Location.X) - ((int)(b.Location.X)) % 5 && (int)(ghost.Location.Y - (this.Size.Height * 0.01)) - (int)(ghost.Location.Y - (this.Size.Height * 0.01)) % 5 == (int)(b.Location.Y) - ((int)(b.Location.Y)) % 5)
            {
                dice_num--;
                if (a.Name == Player1.Name)
                {
                    p1 = b;
                    rotated1 = false;
                }
                else
                    if (a.Name == Player2.Name)
                {
                    p2 = b;
                    rotated2 = false;
                }
                else
                    if (a.Name == Player3.Name)
                {
                    p3 = b;
                    rotated3 = false;
                }
                else
                    if (a.Name == Player4.Name)
                {
                    p4 = b;
                    rotated4 = false;
                }
                Rotate_Chk = true;
            }
        }

        public void Decrementer_rotator(Button b, PictureBox a)
        {
            //Detects if the player have completed crossing a cornered tile to decremet a the dice number and rotate the player
            if ((int)(ghost.Location.X - (this.Size.Width * 0.01)) - (int)(ghost.Location.X - (this.Size.Width * 0.01)) % 5 == (int)(b.Location.X) - ((int)(b.Location.X)) % 5 && (int)(ghost.Location.Y - (this.Size.Height * 0.01)) - (int)(ghost.Location.Y - (this.Size.Height * 0.01)) % 5 == (int)(b.Location.Y) - ((int)(b.Location.Y)) % 5)
            {
                if (Rotate_Chk)
                {
                    dice_num--;
                }
                if (a.Name == Player1.Name)
                {
                    p1 = b;
                    if (rotated1 == false && Rotate_Chk)
                    {
                        a.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        a.Size = new Size(a.Size.Height, a.Size.Width);
                        rotated1 = true;
                    }
                }
                else
                if (a.Name == Player2.Name)
                {
                    p2 = b;
                    if (rotated2 == false)
                    {
                        a.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        a.Size = new Size(a.Size.Height, a.Size.Width);
                        rotated2 = true;
                    }
                }
                if (a.Name == Player3.Name)
                {
                    p3 = b;
                    if (rotated3 == false)
                    {
                        a.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        a.Size = new Size(a.Size.Height, a.Size.Width);
                        rotated3 = true;
                    }
                }
                if (a.Name == Player4.Name)
                {
                    p4 = b;
                    if (rotated4 == false)
                    {
                        a.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        a.Size = new Size(a.Size.Height, a.Size.Width);
                        rotated4 = true;
                    }
                }
            }
        }

        public void EventDetector(PictureBox a)
        {
            //Handles event tiles
            if ((p1 == Tiles[0, 1] || p1 == Tiles[1, 1] || p1 == Tiles[2, 2] || p1 == Tiles[3, 2]) && a.Name == Player1.Name)
            {

                Vid_Container.Visible = true;
                vid.Open("Events/EMP.wmv");
                vid.Play();
                Event1.URL = "Events/EMP.mp3";


                if (p1 == Tiles[0, 1])
                    Event.URL = "Voices/EMP/EMP1" + rand.Next(1, 3) + ".mp3";
                else if (p1 == Tiles[1, 1])
                    Event.URL = "Voices/EMP/EMP2" + rand.Next(1, 3) + ".mp3";
                else if (p1 == Tiles[2, 2])
                    Event.URL = "Voices/EMP/EMP3" + rand.Next(1, 3) + ".mp3";
                else if (p1 == Tiles[3, 2])
                    Event.URL = "Voices/EMP/EMP4" + rand.Next(1, 3) + ".mp3";

                p1station++;
            }
            else if ((p2 == Tiles[0, 1] || p2 == Tiles[1, 1] || p2 == Tiles[2, 2] || p2 == Tiles[3, 2]) && a.Name == Player2.Name)
            {
                Vid_Container.Visible = true;
                vid.Open("Events/EMP.wmv");
                vid.Play();
                Event1.URL = "Events/EMP.mp3";


                if (p2 == Tiles[0, 1])
                    Event.URL = "Voices/EMP/EMP1" + rand.Next(1, 3) + ".mp3";
                else if (p2 == Tiles[1, 1])
                    Event.URL = "Voices/EMP/EMP2" + rand.Next(1, 3) + ".mp3";
                else if (p2 == Tiles[2, 2])
                    Event.URL = "Voices/EMP/EMP3" + rand.Next(1, 3) + ".mp3";
                else if (p2 == Tiles[3, 2])
                    Event.URL = "Voices/EMP/EMP4" + rand.Next(1, 3) + ".mp3";


                p2station++;
            }
            else if ((p3 == Tiles[0, 1] || p3 == Tiles[1, 1] || p3 == Tiles[2, 2] || p3 == Tiles[3, 2]) && a.Name == Player3.Name)
            {
                Vid_Container.Visible = true;
                vid.Open("Events/EMP.wmv");
                vid.Play();
                Event1.URL = "Events/EMP.mp3";


                if (p3 == Tiles[0, 1])
                    Event.URL = "Voices/EMP/EMP1" + rand.Next(1, 3) + ".mp3";
                else if (p3 == Tiles[1, 1])
                    Event.URL = "Voices/EMP/EMP2" + rand.Next(1, 3) + ".mp3";
                else if (p3 == Tiles[2, 2])
                    Event.URL = "Voices/EMP/EMP3" + rand.Next(1, 3) + ".mp3";
                else if (p3 == Tiles[3, 2])
                    Event.URL = "Voices/EMP/EMP4" + rand.Next(1, 3) + ".mp3";


                p3station++;
            }
            else if ((p4 == Tiles[0, 1] || p4 == Tiles[1, 1] || p4 == Tiles[2, 2] || p4 == Tiles[3, 2]) && a.Name == Player4.Name)
            {
                Vid_Container.Visible = true;
                vid.Open("Events/EMP.wmv");
                vid.Play();
                Event1.URL = "Events/EMP.mp3";


                if (p4 == Tiles[0, 1])
                    Event.URL = "Voices/EMP/EMP1" + rand.Next(1, 3) + ".mp3";
                else if (p4 == Tiles[1, 1])
                    Event.URL = "Voices/EMP/EMP2" + rand.Next(1, 3) + ".mp3";
                else if (p4 == Tiles[2, 2])
                    Event.URL = "Voices/EMP/EMP3" + rand.Next(1, 3) + ".mp3";
                else if (p4 == Tiles[3, 2])
                    Event.URL = "Voices/EMP/EMP4" + rand.Next(1, 3) + ".mp3";


                p4station++;
            }


            if (a.Location.X > Tile5.Location.X && a.Location.Y > Tile5.Location.Y ||
                a.Location.X < Tile15.Location.X + Tile15.Size.Width && a.Location.Y < Tile15.Location.Y + Tile15.Size.Height)
            {
                if (a.Name == Player1.Name)
                {
                    Vid_Container.Visible = true;
                    vid.Open("Events/Repair.wmv");
                    vid.Play();
                    Event.URL = "Events/Repair.mp3";
                    Event.controls.play();

                    p1health += 40;
                    if (p1health > 100)
                        p1health = 100;
                    bmp = new Bitmap(P1HP.Width, P1HP.Height);
                    g = Graphics.FromImage(bmp);
                    g.Clear(Color.Black);
                    g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p1health * P1HP.Width / 100.0), P1HP.Height));
                    g.DrawString(p1health + "%", new Font("Arial", P1HP.Height / 2), Brushes.White, new PointF(P1HP.Width / 2 - P1HP.Height, P1HP.Height / 10));
                    P1HP.Image = bmp;
                }
                else if (a.Name == Player2.Name)
                {
                    Vid_Container.Visible = true;
                    vid.Open("Events/Repair.wmv");
                    vid.Play();
                    Event.URL = "Events/Repair.mp3";

                    p2health += 40;
                    if (p2health > 100)
                        p2health = 100;
                    bmp = new Bitmap(P2HP.Width, P2HP.Height);
                    g = Graphics.FromImage(bmp);
                    g.Clear(Color.Black);
                    g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p2health * P2HP.Width / 100.0), P2HP.Height));
                    g.DrawString(p2health + "%", new Font("Arial", P2HP.Height / 2), Brushes.White, new PointF(P2HP.Width / 2 - P2HP.Height, P2HP.Height / 10));
                    P2HP.Image = bmp;
                }
                else if (a.Name == Player3.Name)
                {
                    Vid_Container.Visible = true;
                    vid.Open("Events/Repair.wmv");
                    vid.Play();
                    Event.URL = "Events/Repair.mp3";

                    p3health += 40;
                    if (p3health > 100)
                        p3health = 100;
                    bmp = new Bitmap(P3HP.Width, P3HP.Height);
                    g = Graphics.FromImage(bmp);
                    g.Clear(Color.Black);
                    g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p3health * P3HP.Width / 100.0), P3HP.Height));
                    g.DrawString(p3health + "%", new Font("Arial", P3HP.Height / 2), Brushes.White, new PointF(P3HP.Width / 2 - P3HP.Height, P3HP.Height / 10));
                    P3HP.Image = bmp;
                }
                else if (a.Name == Player4.Name)
                {
                    Vid_Container.Visible = true;
                    vid.Open("Events/Repair.wmv");
                    vid.Play();
                    Event.URL = "Events/Repair.mp3";

                    p4health += 40;
                    if (p4health > 100)
                        p4health = 100;
                    bmp = new Bitmap(P4HP.Width, P4HP.Height);
                    g = Graphics.FromImage(bmp);
                    g.Clear(Color.Black);
                    g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p4health * P4HP.Width / 100.0), P4HP.Height));
                    g.DrawString(p4health + "%", new Font("Arial", P4HP.Height / 2), Brushes.White, new PointF(P4HP.Width / 2 - P4HP.Height, P4HP.Height / 10));
                    P4HP.Image = bmp;
                }
            }

            if (p1 == Tiles[0,0] && a.Name==Player1.Name)
            {
                mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                this.Cursor = mycursor;
                Vid_Container.Visible = true;
                vid.Open("Events/Authorised.wmv");
                vid.Play();
                Event1.URL = "Events/Authorised.mp3";
                Event.URL = "Voices/WMD/WMD"+rand.Next(1,4)+".mp3";
                WMD = true;
                COM_WMD_Handler.Start();

            }
            else if (p2 == Tiles[0, 0] && a.Name == Player2.Name)
            {
                mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                this.Cursor = mycursor;
                Vid_Container.Visible = true;
                vid.Open("Events/Authorised.wmv");
                vid.Play();
                Event1.URL = "Events/Authorised.mp3";
                Event.URL = "Voices/WMD/WMD" + rand.Next(1, 4) + ".mp3";
                WMD = true;
                COM_WMD_Handler.Start();
            }
            else if (p3 == Tiles[0, 0] && a.Name == Player3.Name)
            {
                mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                this.Cursor = mycursor;
                Vid_Container.Visible = true;
                vid.Open("Events/Authorised.wmv");
                vid.Play();
                Event1.URL = "Events/Authorised.mp3";
                Event.URL = "Voices/WMD/WMD" + rand.Next(1, 4) + ".mp3";
                WMD = true;
                COM_WMD_Handler.Start();
            }
            else if (p4 == Tiles[0, 0] && a.Name == Player4.Name)
            {
                mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Attack.ani") });
                this.Cursor = mycursor;
                Vid_Container.Visible = true;
                vid.Open("Events/Authorised.wmv");
                vid.Play();
                Event1.URL = "Events/Authorised.mp3";
                Event.URL = "Voices/WMD/WMD" + rand.Next(1, 4) + ".mp3";
                WMD = true;
                COM_WMD_Handler.Start();
            }

        }

        public void Iterator() {
            //Determines what player turn it is (increment the turn, if the players turn station value is not zero it will decrement it and move to the next turn)
            if (p1station <= 2 && p2station <= 2 && p3station <= 2 && p4station <= 2)
            {
                turns++;
                if (turns > PlayerNum)
                    turns = 1;
            }

            while (true)
            {
                if (turns == 1 && p1station == 0 && p1health != 0)
                    break;
                if (turns == 2 && p2station == 0 && p2health != 0)
                    break;
                if (turns == 3 && p3station == 0 && p3health != 0)
                    break;
                if (turns == 4 && p4station == 0 && p4health != 0)
                    break;
                if (turns == 1 && p1station != 0)
                {
                    p1station--;
                    turns++;
                }
                else if (turns == 2 && p2station != 0)
                {
                    p2station--;
                    turns++;
                }
                else if (turns == 3 && p3station != 0)
                {
                    p3station--;
                    turns++;
                }
                else if (turns == 4 && p4station != 0)
                {
                    p4station--;
                    turns++;
                }
                else if (turns > PlayerNum)
                    turns = 1;
                else turns++;
            }

            if (turns == 1)
            {
                P1.BackColor = Color.PaleGreen;
                P2.BackColor = Color.Black;
                P3.BackColor = Color.Black;
                P4.BackColor = Color.Black;
            }
            if (turns == 2)
            {
                P2.BackColor = Color.PaleGreen;
                P1.BackColor = Color.Black;
                P3.BackColor = Color.Black;
                P4.BackColor = Color.Black;
            }
            if (turns == 3)
            {
                P3.BackColor = Color.PaleGreen;
                P1.BackColor = Color.Black;
                P2.BackColor = Color.Black;
                P4.BackColor = Color.Black;
            }
            if (turns == 4)
            {
                P4.BackColor = Color.PaleGreen;
                P1.BackColor = Color.Black;
                P2.BackColor = Color.Black;
                P3.BackColor = Color.Black;
            }
            COM_Move_Handler.Start();
            steps++;
        }

        private void COM_WMD_Handler_Tick_1(object sender, EventArgs e)
        {
            //WMD handler
            if (!Override)
            {
                if ((turns == 1 && P1.Text == "COM" || turns == 2 && P2.Text == "COM" || turns == 3 && P3.Text == "COM" || turns == 4 && P4.Text == "COM") && !vid.Playing)
                {
                    if (turns != 4 && p4health != 0)
                        Strike(Player4, new EventArgs());
                    else if (turns != 3 && p3health != 0)
                        Strike(Player3, new EventArgs());
                    else if (turns != 2 && p2health != 0)
                        Strike(Player2, new EventArgs());
                    else if (turns != 1 && p1health != 0)
                        Strike(Player1, new EventArgs());
                    COM_WMD_Handler.Stop();
                }
            }
            else
            {
                string[] partition = ReplayScript[Replay_Order].Split(',');
                if (partition[2] == "WMD Strike" && !vid.Playing)
                {
                    if (partition[4] == "4")
                        Strike(Player4, new EventArgs());
                    else if (partition[4] == "3")
                        Strike(Player3, new EventArgs());
                    else if (partition[4] == "2")
                        Strike(Player2, new EventArgs());
                    else if (partition[4] == "1")
                        Strike(Player1, new EventArgs());
                    COM_WMD_Handler.Stop();
                    Replay_Order++;
                }
            }
        }

        private void COM_Move_Handler_Tick(object sender, EventArgs e)
        {
            //COM move handler
            if (!vid.Playing && COM_Tick> 8 && !MenuS.Visible)
            {
                bool checkm = false;
                bool checks = false;
                bool checksu = false;
                bool checkr = false;
                bool checkru = false;
                if (Override)
                {
                    string[] partition= new string[0];
                    if (ReplayScript.Count != Replay_Order)
                    {
                        partition = ReplayScript[Replay_Order].Split(',');
                        checkm = Override && partition[2] == "Manoeuvre";
                        checks = Override && partition[2] == "Surgical Strike";
                        checksu = Override && partition[2] == "Strike Upgrade";
                        checkr = Override && partition[2] == "Repair";
                        checkru = Override && partition[2] == "Repair Upgrade";
                    }
                    if (checks)
                    {
                        S.PerformClick();
                        if (partition[4] == "1")
                        {
                            Strike(Player1, new EventArgs());
                        }
                        else if (partition[4] == "2")
                        {
                            Strike(Player2, new EventArgs());
                        }
                        else if (partition[4] == "3")
                        {
                            Strike(Player3, new EventArgs());
                        }
                        else if (partition[4] == "4")
                        {
                            Strike(Player4, new EventArgs());
                        }
                        Replay_Order++;
                        return;
                    }
                    if (checksu)
                    {
                        SU.PerformClick();
                        Replay_Order++;
                        return;
                    }
                    if (checkr)
                    {
                        R.PerformClick();
                        Replay_Order++;
                        return;
                    }
                    if (checkru)
                    {
                        RU.PerformClick();
                        Replay_Order++;
                        return;
                    }
                    COM_Move_Handler.Stop();
                    COM_Tick = 0;
                }

                if (turns == 1 && P1.Text == "COM" || turns == 2 && P2.Text == "COM" || turns == 3 && P3.Text == "COM" || turns == 4 && P4.Text == "COM" || checkm)
                {
                    Dice.PerformClick();
                    COM_Move_Handler.Stop();
                    COM_Tick = 0;
                }
                else
                {
                    COM_Tick = 0;
                    COM_Move_Handler.Stop();
                }
            }
            COM_Tick++;
        }

        public void CheckWin()
        {
            //Checks if winning conditions are met
            if ((turns == 1 && p2health == 0 && p3health == 0 && p4health == 0) || (turns == 2 && p1health == 0 && p3health == 0 && p4health == 0)
             || (turns == 3 && p1health == 0 && p2health == 0 && p4health == 0) || (turns == 4 && p1health == 0 && p2health == 0 && p3health == 0))
            {
                if (turns == 1)
                {
                    p1S += 2000;
                    P1S.Text = "" + Convert.ToInt32(p1S);
                }
                else if (turns == 2)
                {
                    p2S += 2000;
                    P2S.Text = "" + Convert.ToInt32(p2S);
                }
                else if (turns == 3)
                {
                    p3S += 2000;
                    P3S.Text = "" + Convert.ToInt32(p3S);
                }
                else if (turns == 4)
                {
                    p4S += 2000;
                    P4S.Text = "" + Convert.ToInt32(p4S);
                }

                int MaxS = (int)Math.Max(p1S, Math.Max(p2S, Math.Max(p3S, p4S)));
                int MinS;
                if (P3.Text == "Closed" && P4.Text == "Closed")
                    MinS = (int)Math.Min(p1S,p2S);
                else if (P3.Text=="Closed")
                    MinS = (int)Math.Min(p1S, Math.Min(p2S, p4S));
                else if (P4.Text == "Closed")
                    MinS = (int)Math.Min(p1S, Math.Min(p2S,p3S));
                else
                    MinS = (int)Math.Min(p1S, Math.Min(p2S,Math.Min(p3S,p4S)));
                string[] plyrs = { P1.Text, P2.Text, P3.Text, P4.Text };
                History history = new History(T.Text,steps,MaxS,MinS,plyrs);
                time = 0;
                StandBy = false;
                Quit.Start();
                if (MaxS == (int)p1S)
                    history.Setwinner(P1.Text);
                else if (MaxS == (int)p2S)
                    history.Setwinner(P2.Text);
                else if (MaxS == (int)p3S)
                    history.Setwinner(P3.Text);
                else if (MaxS == (int)p4S)
                    history.Setwinner(P4.Text);
                if (!Override)
                {
                    Recorder.Apply();
                    history.Apply();
                }
            }
        }

        private void Quit_Tick(object sender, EventArgs e)
        {
            //Quit countdown
            if (!vid.Playing)
            {
                if (StandBy==false)
                {
                    Squadron.Visible = false;
                    Vid_Container.Visible = true;
                    vid.Open("Events/Win.wmv");
                    vid.Play();
                    Event1.URL = "Events/Win.mp3";
                    StandBy = true;
                    Main.controls.stop();
                }

                if (!vid.Playing)
                {
                    SubOpen.controls.stop();
                    SubOpen.controls.play();
                    Main.controls.stop();
                    Thread Transition;
                    Transition = new Thread(Runform);
                    Transition.SetApartmentState(ApartmentState.STA);
                    Transition.Start();
                    vid.Dispose();
                    Thread.Sleep(3000);
                    this.Close();
                }
            }
        }



        private void Prey(object x,int y)
        {
            //Triggered by selecting an opponent that is targeted by either a strike or a WMD
            if (x == Player1)
            {
                p1health -= y;
                if (p1health < 0)
                    p1health = 0;
                bmp = new Bitmap(P1HP.Width, P1HP.Height);
                g = Graphics.FromImage(bmp);
                g.Clear(Color.Black);
                g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p1health * P1HP.Width / 100.0), P1HP.Height));
                g.DrawString(p1health + "%", new Font("Arial", P1HP.Height / 2), Brushes.White, new PointF(P1HP.Width / 2 - P1HP.Height, P1HP.Height / 10));
                P1HP.Image = bmp;
                if (p1health == 0)
                    Player1.Visible = false;
            }
            else if (x == Player2)
            {
                p2health -= y;
                if (p2health < 0)
                    p2health = 0;
                bmp = new Bitmap(P2HP.Width, P2HP.Height);
                g = Graphics.FromImage(bmp);
                g.Clear(Color.Black);
                g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p2health * P2HP.Width / 100.0), P2HP.Height));
                g.DrawString(p2health + "%", new Font("Arial", P2HP.Height / 2), Brushes.White, new PointF(P2HP.Width / 2 - P2HP.Height, P2HP.Height / 10));
                P2HP.Image = bmp;
                if (p2health == 0)
                    Player2.Visible = false;
            }
            else if (x == Player3)
            {
                p3health -= y;
                if (p3health < 0)
                    p3health = 0;
                bmp = new Bitmap(P3HP.Width, P3HP.Height);
                g = Graphics.FromImage(bmp);
                g.Clear(Color.Black);
                g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p3health * P3HP.Width / 100.0), P3HP.Height));
                g.DrawString(p3health + "%", new Font("Arial", P3HP.Height / 2), Brushes.White, new PointF(P3HP.Width / 2 - P2HP.Height, P3HP.Height / 10));
                P3HP.Image = bmp;
                if (p3health == 0)
                    Player3.Visible = false;
            }
            else if (x == Player4)
            {
                p4health -= y;
                if (p4health < 0)
                    p4health = 0;
                bmp = new Bitmap(P4HP.Width, P4HP.Height);
                g = Graphics.FromImage(bmp);
                g.Clear(Color.Black);
                g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(p4health * P4HP.Width / 100.0), P4HP.Height));
                g.DrawString(p4health + "%", new Font("Arial", P4HP.Height / 2), Brushes.White, new PointF(P4HP.Width / 2 - P2HP.Height, P4HP.Height / 10));
                P4HP.Image = bmp;
                if (p4health == 0)
                    Player4.Visible = false;
            }
            if (x == Player1 && p1health == 0 || x == Player2 && p2health == 0 || x == Player3 && p3health == 0 || x == Player4 && p4health == 0)
            {
                if (turns == 1)
                    p1S += 1000;
                else if (turns == 2)
                    p2S += 1000;
                else if (turns == 3)
                    p3S += 1000;
                else if (turns == 4)
                    p4S += 1000;
            }
        }

        void Skins()
        {
            if (P1F == "Rising Sun")
                Player1.Image = Image.FromFile("Images/Factions/Tsunami.png");
            else if (P1F == "Soviets")
                Player1.Image = Image.FromFile("Images/Factions/Hammer.png");
            else if (P1F == "Allies")
                Player1.Image = Image.FromFile("Images/Factions/Guardian.png");
            else if (P1F == "NOD")
                Player1.Image = Image.FromFile("Images/Factions/Scorpion.png");

            if (P2F == "Rising Sun")
                Player2.Image = Image.FromFile("Images/Factions/Tsunami.png");
            else if (P2F == "Soviets")
                Player2.Image = Image.FromFile("Images/Factions/Hammer.png");
            else if (P2F == "Allies")
                Player2.Image = Image.FromFile("Images/Factions/Guardian.png");
            else if (P2F == "NOD")
                Player2.Image = Image.FromFile("Images/Factions/Scorpion.png");

            if (P3F == "Rising Sun")
                Player3.Image = Image.FromFile("Images/Factions/Tsunami.png");
            else if (P3F == "Soviets")
                Player3.Image = Image.FromFile("Images/Factions/Hammer.png");
            else if (P3F == "Allies")
                Player3.Image = Image.FromFile("Images/Factions/Guardian.png");
            else if (P3F == "NOD")
                Player3.Image = Image.FromFile("Images/Factions/Scorpion.png");

            if (P4F == "Rising Sun")
                Player4.Image = Image.FromFile("Images/Factions/Tsunami.png");
            else if (P4F == "Soviets")
                Player4.Image = Image.FromFile("Images/Factions/Hammer.png");
            else if (P4F == "Allies")
                Player4.Image = Image.FromFile("Images/Factions/Guardian.png");
            else if (P4F == "NOD")
                Player4.Image = Image.FromFile("Images/Factions/Scorpion.png");
        }

        void Available()
        {
            if (P3.Text == "Closed")
            {
                P3.Visible = false;
                Player3.Visible = false;
                P3HP.Visible = false;
                P3S.Visible = false;
                p3health = 0;
                p3station = -1;
            }
            if (P4.Text == "Closed")
            {
                P4.Visible = false;
                Player4.Visible = false;
                P4HP.Visible = false;
                P4S.Visible = false;
                p4health = 0;
                p4station = -1;
            }
        }

        void Sizer()
        {
            //Size calibrator
            Size Scorpion = new Size((int)(Tile0.Size.Width / 2.3 / 1.625), (int)(Tile0.Size.Height / 2.3));
            Size Tsunami = new Size((int)(Tile0.Size.Width / 2.3 / 1.2), (int)(Tile0.Size.Height / 2.3 / 1.2));
            Size Guardian = new Size((int)(Tile0.Size.Width / 2.3), (int)(Tile0.Size.Height / 2.3));
            Size Hammer = new Size((int)(Tile0.Size.Width / 2.3 / 1.2), (int)(Tile0.Size.Height / 2.3 / 1.2));

            if (P1F == "NOD")
                Player1.Size = Scorpion;
            else if (P1F == "Rising Sun")
                Player1.Size = Tsunami;
            else if (P1F == "Allies")
                Player1.Size = Guardian;
            else if (P1F == "Soviets")
                Player1.Size = Hammer;

            if (P2F == "NOD")
                Player2.Size = Scorpion;
            else if (P2F == "Rising Sun")
                Player2.Size = Tsunami;
            else if (P2F == "Allies")
                Player2.Size = Guardian;
            else if (P2F == "Soviets")
                Player2.Size = Hammer;

            if (P3F == "NOD")
                Player3.Size = Scorpion;
            else if (P3F == "Rising Sun")
                Player3.Size = Tsunami;
            else if (P3F == "Allies")
                Player3.Size = Guardian;
            else if (P3F == "Soviets")
                Player3.Size = Hammer;

            if (P4F == "NOD")
                Player4.Size = Scorpion;
            else if (P4F == "Rising Sun")
                Player4.Size = Tsunami;
            else if (P4F == "Allies")
                Player4.Size = Guardian;
            else if (P4F == "Soviets")
                Player4.Size = Hammer;
        }
    }
}
 