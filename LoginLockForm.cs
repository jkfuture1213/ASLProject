using System;
using System.Windows.Forms;

namespace SmartAccountBook
{
    public partial class LoginLockForm : Form
    {
        private DateTime _lockUntil;

        public LoginLockForm(DateTime lockUntil)
        {
            InitializeComponent();

            _lockUntil = lockUntil;

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan remain = _lockUntil - DateTime.Now;

            if (remain.TotalSeconds <= 0)
            {
                timer1.Stop();

                this.Close();

                return;
            }

            lblRemainTime.Text =
                $"{remain.Minutes:D2}:{remain.Seconds:D2}";
        }
    }
}