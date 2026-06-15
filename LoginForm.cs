using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartAccountBook
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            textBoxPW.UseSystemPasswordChar = true;
        }

        private void btnTogglePW_Click(object sender, EventArgs e)
        {
            // 토글: 현재 UseSystemPasswordChar 값을 반전
            try
            {
                textBoxPW.UseSystemPasswordChar = !textBoxPW.UseSystemPasswordChar;
            }
            catch
            {
                // 안전하게 무시
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string ID = textBoxID.Text;
            string password = textBoxPW.Text;

            bool success = UsersStore.Validate(ID, password);

            if (success)
            {
                MainForm main = new MainForm(ID);

                this.Hide();
                main.ShowDialog();
                // 로그아웃 시 다시 로그인 폼을 보여주기 위해 Close 대신 Show로 변경
                this.Show();
                // 돌아왔을 때 입력 필드 초기화
                try
                {
                    textBoxID.Clear();
                    textBoxPW.Clear();
                    textBoxID.Focus();
                    textBoxPW.UseSystemPasswordChar = true;
                }
                catch { }
            }
            else
            {
                MessageBox.Show("아이디 또는 비밀번호가 틀렸습니다.");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.ShowDialog();
        }

        private void btnRecoverPassword_Click(object sender, EventArgs e)
        {
            using (var form = new RecoverPasswordForm())
            {
                form.ShowDialog(this);
            }
        }
    }
}
