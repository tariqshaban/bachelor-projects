using System;
using System.Windows.Forms;
using System.Threading;

namespace Game
{
    public partial class Splash : Form
    {
        Thread Transition;
        int ticks;

        public Splash()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (ticks++ == 25)
            {
                Transition = new Thread(Runform);
                Transition.SetApartmentState(ApartmentState.STA);
                Transition.Start();
                this.Close();
            }
        }

        void Runform(object obj)
        {
            Application.Run(new Menu());
        }
    }
}
