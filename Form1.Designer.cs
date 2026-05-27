// ===============================
// Form1.Designer.cs
// 기존 코드 유지 + 추가 기능 포함 버전
// ===============================

namespace SmartAccountBook
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing &&
                (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form 디자이너 코드

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Panel panelRight;

        private System.Windows.Forms.DataGridView dgvEntries;

        private System.Windows.Forms.DateTimePicker dtpDate;

        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.ComboBox cbCategory;

        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtAnalysis;

        private System.Windows.Forms.NumericUpDown nudAmount;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnResearch;

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblAmount;

        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPass;

        private System.Windows.Forms.Label lblAnalysisTitle;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.CheckBox chkDarkMode;

        // ==========================
        // 추가 기능
        // ==========================

        private System.Windows.Forms.Button btnShowPassword;

        private System.Windows.Forms.LinkLabel linkFindId;
        private System.Windows.Forms.LinkLabel linkFindPassword;

        private System.Windows.Forms.Label lblWarning;

        private System.Windows.Forms.Timer timerLoginLock;

        private void InitializeComponent()
        {
            this.components =
                new System.ComponentModel.Container();

            this.panelLeft =
                new System.Windows.Forms.Panel();

            this.panelMiddle =
                new System.Windows.Forms.Panel();

            this.panelRight =
                new System.Windows.Forms.Panel();

            this.dgvEntries =
                new System.Windows.Forms.DataGridView();

            this.dtpDate =
                new System.Windows.Forms.DateTimePicker();

            this.cbType =
                new System.Windows.Forms.ComboBox();

            this.cbCategory =
                new System.Windows.Forms.ComboBox();

            this.txtDescription =
                new System.Windows.Forms.TextBox();

            this.txtUsername =
                new System.Windows.Forms.TextBox();

            this.txtPassword =
                new System.Windows.Forms.TextBox();

            this.txtAnalysis =
                new System.Windows.Forms.TextBox();

            this.nudAmount =
                new System.Windows.Forms.NumericUpDown();

            this.btnAdd =
                new System.Windows.Forms.Button();

            this.btnDelete =
                new System.Windows.Forms.Button();

            this.btnLogin =
                new System.Windows.Forms.Button();

            this.btnRegister =
                new System.Windows.Forms.Button();

            this.btnResearch =
                new System.Windows.Forms.Button();

            this.lblTotal =
                new System.Windows.Forms.Label();

            this.lblDate =
                new System.Windows.Forms.Label();

            this.lblType =
                new System.Windows.Forms.Label();

            this.lblCategory =
                new System.Windows.Forms.Label();

            this.lblDescription =
                new System.Windows.Forms.Label();

            this.lblAmount =
                new System.Windows.Forms.Label();

            this.lblUser =
                new System.Windows.Forms.Label();

            this.lblPass =
                new System.Windows.Forms.Label();

            this.lblAnalysisTitle =
                new System.Windows.Forms.Label();

            this.label1 =
                new System.Windows.Forms.Label();

            this.chkDarkMode =
                new System.Windows.Forms.CheckBox();

            // ==========================
            // 추가 객체 생성
            // ==========================

            this.btnShowPassword =
                new System.Windows.Forms.Button();

            this.linkFindId =
                new System.Windows.Forms.LinkLabel();

            this.linkFindPassword =
                new System.Windows.Forms.LinkLabel();

            this.lblWarning =
                new System.Windows.Forms.Label();

            this.timerLoginLock =
                new System.Windows.Forms.Timer(this.components);

            // =====================================
            // Form1
            // =====================================

            this.ClientSize =
                new System.Drawing.Size(1726, 878);

            this.Name = "Form1";

            this.Text = "가계부";

            // =====================================
            // panelLeft
            // =====================================

            this.panelLeft.Dock =
                System.Windows.Forms.DockStyle.Left;

            this.panelLeft.Size =
                new System.Drawing.Size(280, 878);

            // =====================================
            // 로그인 제목
            // =====================================

            this.label1.AutoSize = true;

            this.label1.Font =
                new System.Drawing.Font(
                    "맑은 고딕",
                    18F,
                    System.Drawing.FontStyle.Bold);

            this.label1.Location =
                new System.Drawing.Point(20, 20);

            this.label1.Text =
                "로그인";

            // =====================================
            // 아이디
            // =====================================

            this.lblUser.AutoSize = true;

            this.lblUser.Font =
                new System.Drawing.Font(
                    "맑은 고딕",
                    12F);

            this.lblUser.Location =
                new System.Drawing.Point(20, 80);

            this.lblUser.Text =
                "아이디:";

            this.txtUsername.Location =
                new System.Drawing.Point(20, 110);

            this.txtUsername.Size =
                new System.Drawing.Size(220, 30);

            // =====================================
            // 비밀번호
            // =====================================

            this.lblPass.AutoSize = true;

            this.lblPass.Font =
                new System.Drawing.Font(
                    "맑은 고딕",
                    12F);

            this.lblPass.Location =
                new System.Drawing.Point(20, 165);

            this.lblPass.Text =
                "비밀번호:";

            this.txtPassword.Location =
                new System.Drawing.Point(20, 195);

            this.txtPassword.Size =
                new System.Drawing.Size(180, 30);

            this.txtPassword.UseSystemPasswordChar =
                true;

            // =====================================
            // 비밀번호 보기 버튼
            // =====================================

            this.btnShowPassword.Location =
                new System.Drawing.Point(205, 195);

            this.btnShowPassword.Size =
                new System.Drawing.Size(35, 30);

            this.btnShowPassword.Text =
                "👁";

            this.btnShowPassword.UseVisualStyleBackColor =
                true;

            this.btnShowPassword.Click +=
                new System.EventHandler(
                    this.btnShowPassword_Click);

            // =====================================
            // 로그인 버튼
            // =====================================

            this.btnLogin.Location =
                new System.Drawing.Point(20, 245);

            this.btnLogin.Size =
                new System.Drawing.Size(220, 40);

            this.btnLogin.Text =
                "로그인";

            // =====================================
            // 회원가입 버튼
            // =====================================

            this.btnRegister.Location =
                new System.Drawing.Point(20, 295);

            this.btnRegister.Size =
                new System.Drawing.Size(220, 40);

            this.btnRegister.Text =
                "회원가입";

            // =====================================
            // 아이디 찾기
            // =====================================

            this.linkFindId.AutoSize = true;

            this.linkFindId.Location =
                new System.Drawing.Point(20, 350);

            this.linkFindId.Text =
                "아이디 찾기";

            // =====================================
            // 비밀번호 찾기
            // =====================================

            this.linkFindPassword.AutoSize = true;

            this.linkFindPassword.Location =
                new System.Drawing.Point(120, 350);

            this.linkFindPassword.Text =
                "비밀번호 찾기";

            // =====================================
            // 로그인 경고문
            // =====================================

            this.lblWarning.AutoSize = true;

            this.lblWarning.ForeColor =
                System.Drawing.Color.Red;

            this.lblWarning.Font =
                new System.Drawing.Font(
                    "맑은 고딕",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblWarning.Location =
                new System.Drawing.Point(20, 380);

            this.lblWarning.Size =
                new System.Drawing.Size(220, 40);

            // ==========================
            // 새로 수정한 부분
            // ==========================

            this.lblWarning.Text =
                "⚠ 로그인 실패 : 0 / 5\r\n" +
                "5회 이상 실패 시 로그인 제한";

            // 항상 보이게 설정
            this.lblWarning.Visible = true;

            // ==========================
            // 여기까지 수정
            // ==========================

            // =====================================
            // 다크모드
            // =====================================

            this.chkDarkMode.AutoSize = true;

            this.chkDarkMode.Location =
                new System.Drawing.Point(20, 820);

            this.chkDarkMode.Text =
                "다크 모드";

            // =====================================
            // panelMiddle
            // =====================================

            this.panelMiddle.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.dgvEntries.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.panelMiddle.Controls.Add(
                this.dgvEntries);

            // =====================================
            // panelRight
            // =====================================

            this.panelRight.Dock =
                System.Windows.Forms.DockStyle.Right;

            this.panelRight.Size =
                new System.Drawing.Size(300, 878);

            this.lblAnalysisTitle.AutoSize = true;

            this.lblAnalysisTitle.Font =
                new System.Drawing.Font(
                    "맑은 고딕",
                    14F,
                    System.Drawing.FontStyle.Bold);

            this.lblAnalysisTitle.Location =
                new System.Drawing.Point(20, 20);

            this.lblAnalysisTitle.Text =
                "지출 분석 결과";

            this.txtAnalysis.Multiline = true;

            this.txtAnalysis.Location =
                new System.Drawing.Point(20, 60);

            this.txtAnalysis.Size =
                new System.Drawing.Size(250, 760);

            this.panelRight.Controls.Add(
                this.lblAnalysisTitle);

            this.panelRight.Controls.Add(
                this.txtAnalysis);

            // =====================================
            // panelLeft Controls
            // =====================================

            this.panelLeft.Controls.Add(
                this.label1);

            this.panelLeft.Controls.Add(
                this.lblUser);

            this.panelLeft.Controls.Add(
                this.txtUsername);

            this.panelLeft.Controls.Add(
                this.lblPass);

            this.panelLeft.Controls.Add(
                this.txtPassword);

            this.panelLeft.Controls.Add(
                this.btnShowPassword);

            this.panelLeft.Controls.Add(
                this.btnLogin);

            this.panelLeft.Controls.Add(
                this.btnRegister);

            this.panelLeft.Controls.Add(
                this.linkFindId);

            this.panelLeft.Controls.Add(
                this.linkFindPassword);

            this.panelLeft.Controls.Add(
                this.lblWarning);

            this.panelLeft.Controls.Add(
                this.chkDarkMode);

            // =====================================
            // Form Controls
            // =====================================

            this.Controls.Add(this.panelMiddle);

            this.Controls.Add(this.panelRight);

            this.Controls.Add(this.panelLeft);

            this.ResumeLayout(false);
        }

        #endregion
    }
}