namespace SmartAccountBook
{
    partial class LoginLockForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblRemainTime;
        private System.Windows.Forms.Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblRemainTime = new System.Windows.Forms.Label();

            this.timer1 = new System.Windows.Forms.Timer(this.components);

            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 20);
            this.lblTitle.Size = new System.Drawing.Size(376, 40);
            this.lblTitle.Text = "로그인 제한";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblMessage
            this.lblMessage.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblMessage.Location = new System.Drawing.Point(12, 80);
            this.lblMessage.Size = new System.Drawing.Size(376, 50);
            this.lblMessage.Text = "로그인이 제한되었습니다.";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblRemainTime
            this.lblRemainTime.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.lblRemainTime.ForeColor = System.Drawing.Color.Red;
            this.lblRemainTime.Location = new System.Drawing.Point(12, 150);
            this.lblRemainTime.Size = new System.Drawing.Size(376, 60);
            this.lblRemainTime.Text = "00:00";
            this.lblRemainTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // timer1
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            // LoginLockForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 250);

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblRemainTime);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "로그인 제한";

            this.ResumeLayout(false);
        }
    }
}