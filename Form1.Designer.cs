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
            this.components = new System.ComponentModel.Container();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnShowPassword = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.linkFindId = new System.Windows.Forms.LinkLabel();
            this.linkFindPassword = new System.Windows.Forms.LinkLabel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.chkDarkMode = new System.Windows.Forms.CheckBox();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.dgvEntries = new System.Windows.Forms.DataGridView();
            this.panelRight = new System.Windows.Forms.Panel();
            this.lblAnalysisTitle = new System.Windows.Forms.Label();
            this.txtAnalysis = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.nudAmount = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnResearch = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.timerLoginLock = new System.Windows.Forms.Timer(this.components);
            this.panelLeft.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntries)).BeginInit();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.label1);
            this.panelLeft.Controls.Add(this.lblUser);
            this.panelLeft.Controls.Add(this.txtUsername);
            this.panelLeft.Controls.Add(this.lblPass);
            this.panelLeft.Controls.Add(this.txtPassword);
            this.panelLeft.Controls.Add(this.btnShowPassword);
            this.panelLeft.Controls.Add(this.btnLogin);
            this.panelLeft.Controls.Add(this.btnRegister);
            this.panelLeft.Controls.Add(this.linkFindId);
            this.panelLeft.Controls.Add(this.linkFindPassword);
            this.panelLeft.Controls.Add(this.lblWarning);
            this.panelLeft.Controls.Add(this.chkDarkMode);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(280, 878);
            this.panelLeft.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 65);
            this.label1.TabIndex = 0;
            this.label1.Text = "로그인";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.lblUser.Location = new System.Drawing.Point(20, 80);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(123, 45);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "아이디:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(20, 110);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(220, 35);
            this.txtUsername.TabIndex = 2;
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.lblPass.Location = new System.Drawing.Point(20, 165);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(155, 45);
            this.lblPass.TabIndex = 3;
            this.lblPass.Text = "비밀번호:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(20, 195);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(180, 35);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnShowPassword
            // 
            this.btnShowPassword.Location = new System.Drawing.Point(205, 195);
            this.btnShowPassword.Name = "btnShowPassword";
            this.btnShowPassword.Size = new System.Drawing.Size(35, 30);
            this.btnShowPassword.TabIndex = 5;
            this.btnShowPassword.Text = "👁";
            this.btnShowPassword.UseVisualStyleBackColor = true;
            this.btnShowPassword.Click += new System.EventHandler(this.btnShowPassword_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(20, 245);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(220, 40);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "로그인";
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(20, 295);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(220, 40);
            this.btnRegister.TabIndex = 7;
            this.btnRegister.Text = "회원가입";
            // 
            // linkFindId
            // 
            this.linkFindId.AutoSize = true;
            this.linkFindId.Location = new System.Drawing.Point(20, 350);
            this.linkFindId.Name = "linkFindId";
            this.linkFindId.Size = new System.Drawing.Size(138, 24);
            this.linkFindId.TabIndex = 8;
            this.linkFindId.TabStop = true;
            this.linkFindId.Text = "아이디 찾기";
            // 
            // linkFindPassword
            // 
            this.linkFindPassword.AutoSize = true;
            this.linkFindPassword.Location = new System.Drawing.Point(120, 350);
            this.linkFindPassword.Name = "linkFindPassword";
            this.linkFindPassword.Size = new System.Drawing.Size(162, 24);
            this.linkFindPassword.TabIndex = 9;
            this.linkFindPassword.TabStop = true;
            this.linkFindPassword.Text = "비밀번호 찾기";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(20, 380);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(332, 64);
            this.lblWarning.TabIndex = 10;
            this.lblWarning.Text = "⚠ 로그인 실패 : 0 / 5\r\n5회 이상 실패 시 로그인 제한";
            // 
            // chkDarkMode
            // 
            this.chkDarkMode.AutoSize = true;
            this.chkDarkMode.Location = new System.Drawing.Point(20, 820);
            this.chkDarkMode.Name = "chkDarkMode";
            this.chkDarkMode.Size = new System.Drawing.Size(146, 28);
            this.chkDarkMode.TabIndex = 11;
            this.chkDarkMode.Text = "다크 모드";

            this.panelMiddle.Controls.Add(this.dtpDate);
            this.panelMiddle.Controls.Add(this.cbType);
            this.panelMiddle.Controls.Add(this.cbCategory);
            this.panelMiddle.Controls.Add(this.txtDescription);
            this.panelMiddle.Controls.Add(this.nudAmount);

            this.panelMiddle.Controls.Add(this.btnAdd);
            this.panelMiddle.Controls.Add(this.btnDelete);

            this.panelMiddle.Controls.Add(this.lblDate);
            this.panelMiddle.Controls.Add(this.lblType);
            this.panelMiddle.Controls.Add(this.lblCategory);
            this.panelMiddle.Controls.Add(this.lblDescription);
            this.panelMiddle.Controls.Add(this.lblAmount);
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.dgvEntries);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(280, 0);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(1146, 878);
            this.panelMiddle.TabIndex = 0;
            // 
            // dgvEntries
            // 
            this.dgvEntries.ColumnHeadersHeight = 46;
            this.dgvEntries.Dock = System.Windows.Forms.DockStyle.None;
            this.dgvEntries.Location = new System.Drawing.Point(0, 110);
            this.dgvEntries.Size = new System.Drawing.Size(1146, 768);
            this.dgvEntries.Location = new System.Drawing.Point(0, 0);
            this.dgvEntries.Name = "dgvEntries";
            this.dgvEntries.RowHeadersWidth = 82;
            this.dgvEntries.Size = new System.Drawing.Size(1146, 878);
            this.dgvEntries.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.lblAnalysisTitle);
            this.panelRight.Controls.Add(this.txtAnalysis);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1426, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(300, 878);
            this.panelRight.TabIndex = 1;
            // 
            // lblAnalysisTitle
            // 
            this.lblAnalysisTitle.AutoSize = true;
            this.lblAnalysisTitle.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblAnalysisTitle.Location = new System.Drawing.Point(20, 20);
            this.lblAnalysisTitle.Name = "lblAnalysisTitle";
            this.lblAnalysisTitle.Size = new System.Drawing.Size(276, 51);
            this.lblAnalysisTitle.TabIndex = 0;
            this.lblAnalysisTitle.Text = "지출 분석 결과";
            // 
            // txtAnalysis
            // 
            this.txtAnalysis.Location = new System.Drawing.Point(20, 60);
            this.txtAnalysis.Multiline = true;
            this.txtAnalysis.Name = "txtAnalysis";
            this.txtAnalysis.Size = new System.Drawing.Size(250, 760);
            this.txtAnalysis.TabIndex = 1;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(0, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 35);
            this.dtpDate.TabIndex = 0;
            // 
            // cbType
            // 
            this.cbType.Location = new System.Drawing.Point(0, 0);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 32);
            this.cbType.TabIndex = 0;
            // 
            // cbCategory
            // 
            this.cbCategory.Location = new System.Drawing.Point(0, 0);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(121, 32);
            this.cbCategory.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(0, 0);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(100, 35);
            this.txtDescription.TabIndex = 0;
            // 
            // nudAmount
            // 
            this.nudAmount.Location = new System.Drawing.Point(0, 0);
            this.nudAmount.Name = "nudAmount";
            this.nudAmount.Size = new System.Drawing.Size(120, 35);
            this.nudAmount.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 0;
            // 
            // btnResearch
            // 
            this.btnResearch.Location = new System.Drawing.Point(0, 0);
            this.btnResearch.Name = "btnResearch";
            this.btnResearch.Size = new System.Drawing.Size(75, 23);
            this.btnResearch.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(0, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(100, 23);
            this.lblTotal.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(100, 23);
            this.lblDate.TabIndex = 0;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(0, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(100, 23);
            this.lblType.TabIndex = 0;
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(0, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(100, 23);
            this.lblCategory.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(0, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 23);
            this.lblDescription.TabIndex = 0;
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new System.Drawing.Point(0, 0);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(100, 23);
            this.lblAmount.TabIndex = 0;

            // ===========================
            // 가계부 입력 영역 위치 설정
            // ===========================

            // 날짜
            this.lblDate.Text = "날짜:";
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(20, 25);

            this.dtpDate.Location = new System.Drawing.Point(80, 20);
            this.dtpDate.Size = new System.Drawing.Size(280, 35);

            // 구분
            this.lblType.Text = "구분:";
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(390, 25);

            this.cbType.Location = new System.Drawing.Point(450, 20);
            this.cbType.Size = new System.Drawing.Size(120, 35);

            // 카테고리
            this.lblCategory.Text = "카테고리:";
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(600, 25);

            this.cbCategory.Location = new System.Drawing.Point(700, 20);
            this.cbCategory.Size = new System.Drawing.Size(170, 35);

            // 메모
            this.lblDescription.Text = "메모:";
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 75);

            this.txtDescription.Location = new System.Drawing.Point(80, 70);
            this.txtDescription.Size = new System.Drawing.Size(280, 35);

            // 금액
            this.lblAmount.Text = "금액:";
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(390, 75);

            this.nudAmount.Location = new System.Drawing.Point(450, 70);
            this.nudAmount.Size = new System.Drawing.Size(140, 35);

            // 버튼
            this.btnAdd.Text = "추가";
            this.btnAdd.Location = new System.Drawing.Point(670, 65);
            this.btnAdd.Size = new System.Drawing.Size(90, 40);

            this.btnDelete.Text = "삭제";
            this.btnDelete.Location = new System.Drawing.Point(780, 65);
            this.btnDelete.Size = new System.Drawing.Size(90, 40);

            // DataGridView 아래로 내리기
            this.dgvEntries.Dock = System.Windows.Forms.DockStyle.None;
            this.dgvEntries.Location = new System.Drawing.Point(0, 130);
            this.dgvEntries.Size = new System.Drawing.Size(1146, 748);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1726, 878);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Name = "Form1";
            this.Text = "가계부";
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.panelMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntries)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}