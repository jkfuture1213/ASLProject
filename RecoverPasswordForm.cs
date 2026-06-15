using System;
using System.Windows.Forms;

namespace SmartAccountBook
{
    public class RecoverPasswordForm : Form
    {
        private Label label1;
        private TextBox textBoxID;
        private TextBox textBoxNewPw;
        private TextBox textBoxConfirmPw;
        private Button btnReset;
        private Label label2;
        private Label label3;
        private Label lblResult;

        public RecoverPasswordForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.textBoxNewPw = new System.Windows.Forms.TextBox();
            this.textBoxConfirmPw = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(9, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "찾을 아이디";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(87, 12);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(185, 21);
            this.textBoxID.TabIndex = 1;
            this.textBoxID.TextChanged += new System.EventHandler(this.textBoxID_TextChanged);
            // 
            // textBoxNewPw
            // 
            this.textBoxNewPw.Location = new System.Drawing.Point(87, 49);
            this.textBoxNewPw.Name = "textBoxNewPw";
            this.textBoxNewPw.Size = new System.Drawing.Size(185, 21);
            this.textBoxNewPw.TabIndex = 2;
            this.textBoxNewPw.UseSystemPasswordChar = true;
            // 
            // textBoxConfirmPw
            // 
            this.textBoxConfirmPw.Location = new System.Drawing.Point(87, 87);
            this.textBoxConfirmPw.Name = "textBoxConfirmPw";
            this.textBoxConfirmPw.Size = new System.Drawing.Size(185, 21);
            this.textBoxConfirmPw.TabIndex = 3;
            this.textBoxConfirmPw.UseSystemPasswordChar = true;
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReset.Location = new System.Drawing.Point(197, 123);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "재설정";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 165);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 12);
            this.lblResult.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(9, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "새 비밀번호";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(9, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "비밀번호 확인";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // RecoverPasswordForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 191);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.textBoxConfirmPw);
            this.Controls.Add(this.textBoxNewPw);
            this.Controls.Add(this.textBoxID);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecoverPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "비밀번호 찾기";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string id = textBoxID.Text?.Trim();
            string newPw = textBoxNewPw.Text ?? "";
            string confirmPw = textBoxConfirmPw.Text ?? "";

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("아이디를 입력하세요.");
                return;
            }

            if (string.IsNullOrEmpty(newPw))
            {
                MessageBox.Show("새 비밀번호를 입력하세요.");
                return;
            }

            if (newPw != confirmPw)
            {
                MessageBox.Show("새 비밀번호와 확인이 일치하지 않습니다.");
                return;
            }

            bool ok = UsersStore.UpdatePassword(id, newPw);
            if (!ok)
            {
                lblResult.Text = "해당 아이디가 존재하지 않거나 비밀번호를 변경할 수 없습니다.";
            }
            else
            {
                lblResult.Text = "비밀번호가 재설정되었습니다.";
            }
        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
