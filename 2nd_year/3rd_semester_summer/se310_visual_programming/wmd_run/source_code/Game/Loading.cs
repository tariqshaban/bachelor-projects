using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WMPLib;

namespace Game
{
    public partial class Loading : Form
    {
        int i,size=73;
        readonly WindowsMediaPlayer Main = new WindowsMediaPlayer { URL = "Loading.mp3" };
        readonly System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public string P1N, P2N, P3N, P4N, P1F, P2F, P3F, P4F;
        double pbUnit;
        int pbWIDTH, pbHEIGHT, pbComplete;

        Bitmap bmp;
        Graphics g;

        public Loading()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            Factions();
            string[] arr = { P1N, P2N, P3N, P4N, P1F, P2F, P3F, P4F };
            float f=this.Height / 2 - panel1.Height/1.5f;
            if (f < 0)
                f = 50;
            Cursor.Hide();
            Main.controls.play();
            Main.settings.setMode("loop", true);
            panel1.Location = new Point(this.Width / 2 - panel1.Width / 2, (int)f);
            Players.Location = new Point(Players.Location.X+panel1.Size.Width/2-Players.Size.Width/2,Players.Location.Y);
            panel1.BackColor = Color.FromArgb(100, Color.Black);
            pbWIDTH = picboxPB.Width;
            pbHEIGHT = picboxPB.Height;
            pbUnit = pbWIDTH / 100.0;
            pbComplete = 0;
            bmp = new Bitmap(pbWIDTH, pbHEIGHT);
            t.Interval = 150;
            t.Tick += new EventHandler(this.Timer1_Tick);
            t.Start();

            Players.SuspendLayout();
            for(int i=1;i<=4;i++)
            {
                if (P3N == "Closed" && i == 3 || P4N == "Closed" && i == 4)
                    continue;
                size += 73;
                Players.RowCount++;
                Players.Controls.Add(new LabelEx.LabelEx() { Text = arr[i - 1], ShadowDepth = 1, ShowTextShadow = true, Size = new Size(327, 66), BackColor = Color.Transparent, OutlineColor = Color.White, BorderColor = Color.Black, ForeColor = Color.Black, BorderStyle = LabelEx.LabelEx.BorderType.Squared, Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))), });
                Players.Controls.Add(new LabelEx.LabelEx() { BackgroundImage = Image.FromFile(arr[i + 3]), Size = new Size(327, 66), BackgroundImageLayout= ImageLayout.Stretch });
            }
            Players.Size = new Size(Players.Size.Width, size);
            Players.ResumeLayout();
        }

        private void Min(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.RWin || e.KeyCode == Keys.LWin)
            {
                Main.controls.pause();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void Restore(object sender, EventArgs e)
        {
            if (i++ > 3 && this.WindowState == FormWindowState.Maximized)
                Main.controls.play();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, (int)(pbComplete * pbUnit), pbHEIGHT));
            g.DrawString(pbComplete + "%", new Font("Arial", pbHEIGHT / 2), Brushes.White, new PointF(pbWIDTH / 2 - pbHEIGHT, pbHEIGHT / 10));
            picboxPB.Image = bmp;
            pbComplete++;
            if (pbComplete < 20)
                t.Interval = 100;
            else if (pbComplete < 50)
                t.Interval = 60;
            else if (pbComplete < 89)
                t.Interval = 175;
            else if (pbComplete < 90)
                t.Interval = 250;
            else if (pbComplete > 101)
            {
                Main.controls.stop();
                System.Threading.Thread.Sleep(1000);
                this.Dispose();
            }
        }
        void Factions()
        {
            if (P1F == "Rising Sun")
                P1F = "Images/Rising Sun.jpg";
            else if (P1F == "Soviets")
                P1F = "Images/Soviets.png";
            else if (P1F == "Allies")
                P1F = "Images/Allies.gif";
            else if (P1F == "NOD")
                P1F = "Images/NOD.png";

            if (P2F == "Rising Sun")
                P2F = "Images/Rising Sun.jpg";
            else if (P2F == "Soviets")
                P2F = "Images/Soviets.png";
            else if (P2F == "Allies")
                P2F = "Images/Allies.gif";
            else if (P2F == "NOD")
                P2F = "Images/NOD.png";

            if (P3F == "Rising Sun")
                P3F = "Images/Rising Sun.jpg";
            else if (P3F == "Soviets")
                P3F = "Images/Soviets.png";
            else if (P3F == "Allies")
                P3F = "Images/Allies.gif";
            else if (P3F == "NOD")
                P3F = "Images/NOD.png";

            if (P4F == "Rising Sun")
                P4F = "Images/Rising Sun.jpg";
            else if (P4F == "Soviets")
                P4F = "Images/Soviets.png";
            else if (P4F == "Allies")
                P4F = "Images/Allies.gif";
            else if (P4F == "NOD")
                P4F = "Images/NOD.png";
        }
    }
}
