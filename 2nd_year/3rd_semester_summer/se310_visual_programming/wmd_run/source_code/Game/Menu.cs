using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.DirectX.AudioVideoPlayback;
using WMPLib;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Game
{
    public partial class Menu : Form
    {
        Profile ProfileDB;
        int i, replay;
        readonly WindowsMediaPlayer Main = new WindowsMediaPlayer { URL = "Main.mp3" };
        readonly WindowsMediaPlayer HoverS = new WindowsMediaPlayer { URL = "Select.mp3" };
        readonly WindowsMediaPlayer Open = new WindowsMediaPlayer { URL = "Open.mp3" };
        readonly WindowsMediaPlayer SubOpen = new WindowsMediaPlayer { URL = "SubOpen.mp3" };
        readonly Cursor mycursor = new Cursor(Cursor.Current.Handle);
        Video vid;
        int ticks;
        public bool isWin = false, edit_override = false;
        string p1n, p2n, p3n, p4n, p1f, p2f, p3f, p4f;
        readonly List<LabelEx.LabelEx> ID = new List<LabelEx.LabelEx>();
        bool Override = false;
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string filename);

        public Menu()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Checks if the player have returned from a game to change the theme
            if (!isWin)
                vid = new Video("vid.wmv") { Owner = Vid_Pnl };
            else
            {
                vid = new Video("Victory.wmv") { Owner = Vid_Pnl };
                Main.URL = "Victory.mp3";
                Title.BorderColor = Color.Red;
                Title.OutlineColor = Color.OrangeRed;

                Main_Menu.Visible = false;
                Hitory_Stat_Detailed.Visible = true;
                Title.Text = "Game Details";
                List<string> file2 = File.ReadAllLines("DB/InGame.txt").ToList();

                //Shows the recent game activities
                history_t.AutoScroll = false;
                history_t.Controls.Clear();
                history_t.RowStyles.Clear();
                history_t.RowCount = 1;
                history_t.RowStyles.Add(new RowStyle(SizeType.AutoSize, 20F));
                history_t.Controls.Add(new LabelEx.LabelEx() { Text = "Player", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                history_t.Controls.Add(new LabelEx.LabelEx() { Text = "Procedure", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                history_t.Controls.Add(new LabelEx.LabelEx() { Text = "Value", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                history_t.AutoScroll = true;
                history_t.HorizontalScroll.Visible = false;
                history_t.SuspendLayout();//Saves processing time by rendering the table when it's filled
                foreach (string line in file2)
                {
                    string[] partition = line.Split(',');
                    if (File.ReadLines("DB/History.txt").Count() == Convert.ToInt32(partition[0]))
                    {
                        history_t.RowCount++;
                        history_t.Controls.Add(new LabelEx.LabelEx() { Text = partition[1], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                        history_t.Controls.Add(new LabelEx.LabelEx() { Text = partition[2], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                        history_t.Controls.Add(new LabelEx.LabelEx() { Text = partition[3], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                    }
                }
                history_t.ResumeLayout();//Render the table
                replay = File.ReadAllLines("DB/History.txt").Count();
            }
            //Background video (DirectX)
            Vid_Pnl.Size = this.Size;
            vid.Play();

            //Changes the default cursor
            mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Cursor.cur") });
            this.Cursor = mycursor;
            Main.controls.play();
            Main.settings.setMode("loop", true);
            vid.Ending += new EventHandler(BackLoop);

            //Changes the combobox style
            this.Player1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Player2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Player3.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Player4.DropDownStyle = ComboBoxStyle.DropDownList;

            //Initializes the locations of all panels
            Main_Menu.Location = new Point((int)(0.5f * this.Size.Width), (int)(0.33f * this.Size.Height));
            New_Game_Menu.Location = new Point((int)(0.5f * this.Size.Width), (int)(0.33f * this.Size.Height));
            Title.Location = new Point(Main_Menu.Location.X + Main_Menu.Size.Width / 5, (int)(Main_Menu.Location.Y - this.Size.Height * 0.20f));
            Stat_Report.Location = new Point((int)(Title.Location.X - this.Size.Height * 0.005f), Main_Menu.Location.Y);
            History_Report.Location = new Point((int)(Title.Location.X - this.Size.Height * 0.19f), Main_Menu.Location.Y);
            Hitory_Stat_Detailed.Location = new Point((int)(Title.Location.X - this.Size.Height * 0.178f), Main_Menu.Location.Y);
            Profile.Location = new Point((int)(Title.Location.X - this.Size.Height * 0.16f), Main_Menu.Location.Y);
            Preview.Location = new Point((int)(Title.Location.X - this.Size.Height * 0.16f), Main_Menu.Location.Y);

            //The textbox of the create and edit profile
            name.Text = "Player";
            name.Leave += new System.EventHandler(Leave);
            name.Enter += new System.EventHandler(Enter);

            //Recalibrate panel locations 'Referentially'
            Calibrator();

            //Fills the comboboxes with profiles
            List<string> file = File.ReadAllLines("DB/Profiles.txt").ToList();
            foreach (string line in file)
            {
                string[] partition = line.Split(',');
                Player1.Items.Add(partition[0]);
                Player2.Items.Add(partition[0]);
                Player3.Items.Add(partition[0]);
                Player4.Items.Add(partition[0]);
            }
            Player1.Items.Add("COM");
            Player2.Items.Add("COM");
            Player3.Items.Add("COM");
            Player4.Items.Add("COM");
            Player3.Items.Add("Closed");
            Player4.Items.Add("Closed");

            //Assign a label with the version
            string version = System.Windows.Forms.Application.ProductVersion;
            Version.Text = "V" + version + " experimental";

            //Fills the history table
            List<string> file1 = File.ReadAllLines("DB/History.txt").ToList();
            List<int> scores = new List<int>();
            int position = 0;
            tableLayoutPanel1.SuspendLayout();
            foreach (string line in file1)
            {
                string[] partition = line.Split(',');
                ID.Add(new LabelEx.LabelEx() { Text = partition[0], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, (byte)(0)) });
                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.Controls.Add(new LabelEx.LabelEx() { Text = partition[3], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) }, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(new LabelEx.LabelEx() { Text = partition[4], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) }, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(new LabelEx.LabelEx() { Text = partition[2], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) }, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(ID[position], 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(new LabelEx.LabelEx() { Text = partition[1], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) }, 0, tableLayoutPanel1.RowCount - 1);
                position++;
                scores.Add(Convert.ToInt32(partition[5]));
                scores.Add(Convert.ToInt32(partition[6]));
            }
            //Applies linq to find the min and max scrore
            IEnumerable<int> linq = from x in scores
                                  orderby x
                                  select x;
            scores = linq.ToList();

            //Fills the stats table
            tableLayoutPanel1.ResumeLayout();
            Stat stat = new Stat();
            if (!stat.IsEmpty)
            {
                Table.Controls.Add(new LabelEx.LabelEx() { Text = stat.Games + "", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                Table.Controls.Add(new LabelEx.LabelEx() { Text = stat.Profiles + "", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                Table.Controls.Add(new LabelEx.LabelEx() { Text = scores[scores.Count - 1] + "", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))), });
                Table.Controls.Add(new LabelEx.LabelEx() { Text = scores[0] + "", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))), });
                Table.Controls.Add(new LabelEx.LabelEx() { Text = stat.LD, ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))), });
                Table.Controls.Add(new LabelEx.LabelEx() { Text = stat.HD, ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))), });
                Table.Controls.Add(new LabelEx.LabelEx() { Text = stat.AD, ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))), });
            }

            //Handels mouse events on the game number in the history table
            foreach (var x in ID)
            {
                x.MouseEnter += new EventHandler(Hover);
                x.MouseLeave += new EventHandler(Default_Cursor);
                x.Click += new EventHandler(Clicking);
            }
        }

        //Replays the video
        private void BackLoop(object sender, EventArgs e)
        {
            vid.CurrentPosition = 0;
        }

        //Mouse enter event that changes the cursor
        private void Hover(object sender, EventArgs e)
        {
            HoverS.controls.stop();
            HoverS.controls.play();
            mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Select.ani") });
            this.Cursor = mycursor;
        }

        //Handles click events
        private void Clicking(object sender, EventArgs e)
        {
            Open.controls.stop();
            Open.controls.play();
            if (sender == New_Game)
            {
                bunifuTransition1.HideSync(Main_Menu);
                bunifuTransition1.ShowSync(New_Game_Menu);
                Create.Visible = false;
                Edit.Visible = false;
                Title.Text = "New Game";
            }
            else if (sender == New_Game_Back)
            {
                bunifuTransition1.HideSync(New_Game_Menu);
                bunifuTransition1.ShowSync(Main_Menu);
                Title.Text = "WMD RUN";
                //Hides the warning message
                Duplicate.Visible = false;
            }
            else if (sender == Stats)
            {
                bunifuTransition1.HideSync(Main_Menu);
                bunifuTransition1.ShowSync(Stat_Report);
                Create.Visible = false;
                Edit.Visible = false;
                System.Threading.Thread.Sleep(100);
                Title.Text = "Stats";
            }
            else if (sender == Stats_Back)
            {
                bunifuTransition1.HideSync(Stat_Report);
                bunifuTransition1.ShowSync(Main_Menu);
                Title.Text = "WMD RUN";
            }
            else if (sender == History)
            {
                bunifuTransition1.HideSync(Main_Menu);
                bunifuTransition1.ShowSync(History_Report);
                Create.Visible = false;
                Edit.Visible = false;
                System.Threading.Thread.Sleep(100);
                Title.Text = "History";
            }
            else if (sender == Create)
            {
                bunifuTransition1.HideSync(Main_Menu);
                bunifuTransition1.ShowSync(Profile);
                Create.Visible = false;
                Edit.Visible = false;
                System.Threading.Thread.Sleep(100);
                Title.Text = "Profile";
            }
            else if (sender == Edit)
            {
                bunifuTransition1.HideSync(Main_Menu);
                bunifuTransition1.ShowSync(Profile);
                Create.Visible = false;
                Edit.Visible = false;
                System.Threading.Thread.Sleep(100);
                Title.Text = "Profile Editor";
                edit_override = true;
            }
            else if (sender == History_Back)
            {
                bunifuTransition1.HideSync(History_Report);
                bunifuTransition1.ShowSync(Main_Menu);
                Title.Text = "WMD RUN";
            }
            else if (sender == History_Detailed_Back)
            {
                bunifuTransition1.HideSync(Hitory_Stat_Detailed);
                bunifuTransition1.ShowSync(History_Report);
                Title.Text = "History";
            }
            else if (sender == Profile_Back)
            {
                bunifuTransition1.HideSync(Profile);
                bunifuTransition1.ShowSync(Main_Menu);
                Title.Text = "Title";
                //Hides the warning message
                Warning.Visible = false;
                Warning.Text = "All Fields Are Required";
                edit_override = false;
            }
            else if (sender == Preview_Back)
            {
                GIFPreview.Image = null;
                bunifuTransition1.HideSync(Preview);
                bunifuTransition1.ShowSync(Profile);
                Title.Text = "Profile";
                if (edit_override)
                    Title.Text = "Profile Editor";
            }
            else if (sender.GetType().Name == "LabelEx")
            {
                //Handles the game number label click in the history table
                replay = Convert.ToInt32(((LabelEx.LabelEx)sender).Text);//Sets which game to be replayed (int)
                bunifuTransition1.HideSync(History_Report);
                bunifuTransition1.ShowSync(Hitory_Stat_Detailed);
                Title.Text = "Game Details";
                List<string> file2 = File.ReadAllLines("DB/InGame.txt").ToList();

                //Fills the replay table
                history_t.AutoScroll = false;
                history_t.Controls.Clear();
                history_t.RowStyles.Clear();
                history_t.RowCount = 1;
                history_t.RowStyles.Add(new RowStyle(SizeType.AutoSize, 20F));
                history_t.Controls.Add(new LabelEx.LabelEx() { Text = "Player", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                history_t.Controls.Add(new LabelEx.LabelEx() { Text = "Procedure", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                history_t.Controls.Add(new LabelEx.LabelEx() { Text = "Value", ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.DimGray, OutlineColor = Color.LightSkyBlue, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                history_t.AutoScroll = true;
                history_t.HorizontalScroll.Visible = false;
                history_t.SuspendLayout();
                foreach (string line in file2)
                {
                    string[] partition = line.Split(',');
                    if (((LabelEx.LabelEx)sender).Text == partition[0])
                    {
                        history_t.RowCount++;
                        history_t.Controls.Add(new LabelEx.LabelEx() { Text = partition[1], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                        history_t.Controls.Add(new LabelEx.LabelEx() { Text = partition[2], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                        history_t.Controls.Add(new LabelEx.LabelEx() { Text = partition[3], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) });
                    }
                }
                history_t.ResumeLayout();
            }

        }

        //Handles click sounds on main elements
        private void SubClick(object sender, EventArgs e)
        {
            SubOpen.controls.stop();
            SubOpen.controls.play();
            if (sender == Quit)
                timer1.Start();
            else if (sender == Profiles)
            {
                if (Create.Visible == true)
                {
                    Create.Visible = false;
                    Edit.Visible = false;
                }
                else
                {
                    Create.Visible = true;
                    Edit.Visible = true;
                }
            }
            else if (sender == New_Game_Start)
            {
                //Validate players that are participating in a game
                if (Player1.Text == Player2.Text && Player1.Text != "COM" || Player1.Text == Player3.Text && Player3.Text != "COM"
                    || Player1.Text == Player4.Text && Player4.Text != "COM" ||
                    Player2.Text == Player3.Text && Player2.Text != "COM" || Player2.Text == Player4.Text && Player2.Text != "COM" ||
                    Player3.Text == Player4.Text && (Player3.Text != "COM" && Player3.Text != "Closed") || Player3.Text == "" || Player4.Text == "")
                    Duplicate.Visible = true;
                else
                {
                    //Fetches the faction based on the profile name by reading profile.txt file
                    ProfileDB = new Profile(Player1.Text);
                    p1f = ProfileDB.FindFaction();
                    ProfileDB = new Profile(Player2.Text);
                    p2f = ProfileDB.FindFaction();
                    ProfileDB = new Profile(Player3.Text);
                    p3f = ProfileDB.FindFaction();
                    ProfileDB = new Profile(Player4.Text);
                    p4f = ProfileDB.FindFaction();
                    p1n = Player1.Text;
                    p2n = Player2.Text;
                    p3n = Player3.Text;
                    p4n = Player4.Text;
                    System.Threading.Thread.Sleep(400);
                    Thread Transition;
                    Transition = new Thread(Runform);
                    Transition.SetApartmentState(ApartmentState.STA);
                    Transition.Start();
                    Thread Transition2;
                    Transition2 = new Thread(Runform2);
                    Transition2.SetApartmentState(ApartmentState.STA);
                    Transition2.Start();
                    Main.controls.stop();
                    vid.Dispose();
                    this.Close();
                }
            }
            else if (sender == Save)
            {
                //Validates profile creation
                if (Title.Text == "Profile")
                {
                    if (name.Text == "" || name.Text == "Player" || !(Tsunami_Panel.BackColor == Color.Cyan || Hammer_Panel.BackColor == Color.Cyan || Guardian_Panel.BackColor == Color.Cyan || Scorpion_Panel.BackColor == Color.Cyan))
                        Warning.Visible = true;
                    else if (name.Text.Length > 6)
                    {
                        Warning.Text = "Atmost Six Characters Are Allowed";
                        Warning.Visible = true;
                    }
                    else
                    {
                        //Reads the selected faction
                        ProfileDB = new Profile(name.Text);
                        if (Tsunami_Panel.BackColor == Color.Cyan)
                            ProfileDB.Faction = "Rising Sun";
                        else if (Hammer.BackColor == Color.Cyan)
                            ProfileDB.Faction = "Soviets";
                        else if (Guardian.BackColor == Color.Cyan)
                            ProfileDB.Faction = "Allies";
                        else
                            ProfileDB.Faction = "NOD";
                        //Reserved profile names
                        if (ProfileDB.Exist || name.Text == "COM" || name.Text == "Closed")
                        {
                            Warning.Text = "Name Already Exist";
                            Warning.Visible = true;
                        }
                        else
                        {
                            //Flushes the profile
                            ProfileDB.Insert();

                            //Re-reads from the profile name to fill the comboboxes with the updated profile list
                            Player1.Items.Clear();
                            Player2.Items.Clear();
                            Player3.Items.Clear();
                            Player4.Items.Clear();
                            List<string> file = File.ReadAllLines("DB/Profiles.txt").ToList();
                            foreach (string line in file)
                            {
                                string[] partition = line.Split(',');
                                Player1.Items.Add(partition[0]);
                                Player2.Items.Add(partition[0]);
                                Player3.Items.Add(partition[0]);
                                Player4.Items.Add(partition[0]);
                            }
                            Player1.Items.Add("COM");
                            Player2.Items.Add("COM");
                            Player3.Items.Add("COM");
                            Player4.Items.Add("COM");
                            Player3.Items.Add("Closed");
                            Player4.Items.Add("Closed");

                            bunifuTransition1.HideSync(Profile);
                            bunifuTransition1.ShowSync(Main_Menu);
                        }
                    }
                }
                else if (Title.Text == "Profile Editor")
                {
                    if (name.Text == "" || name.Text == "Player" || !(Tsunami_Panel.BackColor == Color.Cyan || Hammer_Panel.BackColor == Color.Cyan || Guardian_Panel.BackColor == Color.Cyan || Scorpion_Panel.BackColor == Color.Cyan))
                        Warning.Visible = true;
                    else
                    {
                        ProfileDB = new Profile(name.Text);
                        if (Tsunami_Panel.BackColor == Color.Cyan)
                            ProfileDB.Faction = "Rising Sun";
                        else if (Hammer.BackColor == Color.Cyan)
                            ProfileDB.Faction = "Soviets";
                        else if (Guardian.BackColor == Color.Cyan)
                            ProfileDB.Faction = "Allies";
                        else
                            ProfileDB.Faction = "NOD";
                        if (ProfileDB.Exist)
                        {
                            ProfileDB.Editor();
                            bunifuTransition1.HideSync(Profile);
                            bunifuTransition1.ShowSync(Main_Menu);
                        }
                        else
                        {
                            Warning.Text = "Name Not Found";
                            Warning.Visible = true;
                        }
                    }
                }
            }
            else if (sender == Replay)
            {
                List<string> file2 = File.ReadAllLines("DB/History.txt").ToList();
                foreach (string line in file2)
                {
                    Override = true;
                    string[] partition = line.Split(',');
                    if (replay == Convert.ToInt32(partition[0]))
                    {
                        //Retrieve the saved profiles to start the replay
                        ProfileDB = new Profile(partition[7]);
                        p1f = ProfileDB.FindFaction();
                        ProfileDB = new Profile(partition[8]);
                        p2f = ProfileDB.FindFaction();
                        ProfileDB = new Profile(partition[9]);
                        p3f = ProfileDB.FindFaction();
                        ProfileDB = new Profile(partition[10]);
                        p4f = ProfileDB.FindFaction();
                        p1n = partition[7];
                        p2n = partition[8];
                        p3n = partition[9];
                        p4n = partition[10];
                        System.Threading.Thread.Sleep(400);
                        Thread Transition;
                        Transition = new Thread(Runform);
                        Transition.SetApartmentState(ApartmentState.STA);
                        Transition.Start();
                        Thread Transition2;
                        Transition2 = new Thread(Runform2);
                        Transition2.SetApartmentState(ApartmentState.STA);
                        Transition2.Start();
                        Main.controls.stop();
                        vid.Dispose();
                        this.Close();
                    }
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //Exit countdown
            if (ticks++ > 8)
                Application.Exit();
        }

        private void Min(object sender, KeyEventArgs e)
        {
            //Minimize handler
            if (e.KeyCode == Keys.RWin || e.KeyCode == Keys.LWin)
            {
                Main.controls.pause();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void Restore(object sender, EventArgs e)
        {
            //Restore Handler
            if (i++ > 3 && this.WindowState == FormWindowState.Maximized)
                Main.controls.play();
        }

        private new void Leave(object sender, EventArgs e)
        {
            //Refills the textbox
            if (name.Text == "")
            {
                name.Text = "Player";
            }
        }

        private new void Enter(object sender, EventArgs e)
        {
            //Empty the textbox when the user attemps to enter the name
            if (name.Text == "Player")
            {
                name.Text = "";
            }
        }

        private void Faction(object sender, EventArgs e)
        {
            //Detailed factions description
            SubOpen.controls.stop();
            SubOpen.controls.play();

            Tsunami_Panel.BackColor = Color.Transparent;
            Hammer_Panel.BackColor = Color.Transparent;
            Guardian_Panel.BackColor = Color.Transparent;
            Scorpion_Panel.BackColor = Color.Transparent;

            if (sender == Tsunami)
            {
                bunifuTransition1.HideSync(Profile);
                bunifuTransition1.ShowSync(Preview);
                Tsunami_Panel.BackColor = Color.Cyan;
                GIFPreview.Image = Image.FromFile("GIF/Tsunami.gif");
                Title.Text = "Tsunami Tank";
                Spec.Text = "High     : Speed, Regeneration\nLow      : Health\n\nAbitlities:\nFinal Squadron, Nanodeflectors";
                Desc.Text = " The Tsunami tank is yet another ambitious breakthrough in the Empire arsenal, it can travel both land and sea at exceptional speeds";
            }
            else if (sender == Hammer)
            {
                bunifuTransition1.HideSync(Profile);
                bunifuTransition1.ShowSync(Preview);
                Hammer_Panel.BackColor = Color.Cyan;
                GIFPreview.Image = Image.FromFile("GIF/Hammer.gif");
                Title.Text = "Hammer Tank";
                Spec.Text = "High     : Health, Support Powers\nLow      : Speed\n\nAbitlities:\nSurgical Strike, Leech Beam";
                Desc.Text = " The Hammer tank is the Soviets main front line battle tank, it's considerably cheap due to mass productions";
            }
            else if (sender == Guardian)
            {
                bunifuTransition1.HideSync(Profile);
                bunifuTransition1.ShowSync(Preview);
                Guardian_Panel.BackColor = Color.Cyan;
                GIFPreview.Image = null;
                Title.Text = "Guardian Tank";
                Spec.Text = "High     : Health\nLow      : Regeneration\n\nAbitlities:\nRocket Artillery, Repair Drones";
                Desc.Text = " The Guardian tank was the Allies last hope during the liberation of europe. The state of the art composite armor made it handle alot of pounding ";
            }
            else if (sender == Scorpion)
            {
                bunifuTransition1.HideSync(Profile);
                bunifuTransition1.ShowSync(Preview);
                Scorpion_Panel.BackColor = Color.Cyan;
                GIFPreview.Image = null;
                Title.Text = "Scorpion Tank";
                Spec.Text = "High     : Speed, Regeneration\nLow      : Health\n\nAbitlities:\nLand Mines, Guerilla Repair";
                Desc.Text = " The Scorpian tank is Nod's standard combat armor, it's usually used to guard convoys due to it's fast maneuverability but it's reliability is questionable";
            }
        }

        private void Default_Cursor(object sender, EventArgs e)
        {
            //Return to the default cursor on mouse leave event
            mycursor.GetType().InvokeMember("handle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, mycursor, new object[] { LoadCursorFromFile("Cursor.cur") });
            this.Cursor = mycursor;
        }
        void Runform(object obj)
        {
            //Launches the loading screen, passing the player names and factions
            System.Threading.Thread.Sleep(100);
            Application.Run(new Loading() { P1N = p1n, P2N = p2n, P3N = p3n, P4N = p4n, P1F = p1f, P2F = p2f, P3F = p3f, P4F = p4f });
        }
        void Runform2(object obj)
        {
            //Launches the game screen, passing the player names, factions,if it was a replay and the game number
            if (!Override)
                Application.Run(new Game() { P1N = p1n, P2N = p2n, P3N = p3n, P4N = p4n, P1F = p1f, P2F = p2f, P3F = p3f, P4F = p4f, Override = false });
            else
                Application.Run(new Game() { P1N = p1n, P2N = p2n, P3N = p3n, P4N = p4n, P1F = p1f, P2F = p2f, P3F = p3f, P4F = p4f, Override = true, GameNo = replay });
        }

        void Calibrator()
        {
            if (Profile.Location.X + Profile.Size.Width > this.Size.Width)
            {
                Main_Menu.Location = new Point((int)(0.45f * this.Size.Width), (int)(0.3f * this.Size.Height));
                New_Game_Menu.Location = new Point((int)(0.45f * this.Size.Width), (int)(0.3f * this.Size.Height));
                Title.Location = new Point(Main_Menu.Location.X + Main_Menu.Size.Width / 5, (int)(Main_Menu.Location.Y - this.Size.Height * 0.20f));
                Stat_Report.Location = new Point((int)(Title.Location.X - this.Size.Height * 0.005f), Main_Menu.Location.Y);
                History_Report.Location = new Point((int)(Title.Location.X + Title.Size.Width / 2 - History_Report.Size.Width / 2), Main_Menu.Location.Y);
                Hitory_Stat_Detailed.Location = new Point((int)(Title.Location.X + Title.Size.Width / 2 - Hitory_Stat_Detailed.Size.Width / 2), Main_Menu.Location.Y);
                Profile.Location = new Point((int)(Title.Location.X + Title.Size.Width / 2 - Profile.Size.Width / 2), Main_Menu.Location.Y);
                Preview.Location = new Point((int)(Title.Location.X + Title.Size.Width / 2 - Preview.Size.Width / 2), Main_Menu.Location.Y);
            }
        }
    }
}
