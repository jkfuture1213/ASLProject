using System;
using System.Windows.Forms;

namespace SmartAccountBook
{
    public partial class FindPasswordForm : Form
    {
        public FindPasswordForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("아이디를 입력하세요.");
                return;
            }

            string password = UsersStore.GetPassword(id);

            if (password == null)
            {
                MessageBox.Show("존재하지 않는 아이디입니다.");
            }
            else
            {
                MessageBox.Show(
                    $"비밀번호 : {password}",
                    "조회 결과",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}