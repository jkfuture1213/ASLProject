using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace SmartAccountBook
{
    public partial class Form1 : Form
    {
        private BindingList<Transaction> _transactions = new BindingList<Transaction>();
        private DateTime _lastAdd = DateTime.MinValue;
        private string _currentUser = null;

        // =========================================================
        // ▼▼▼ 새로 추가한 로그인 제한 기능 ▼▼▼
        // =========================================================

        // 로그인 실패 횟수 저장
        private int loginFailCount = 0;

        // 로그인 제한 여부 확인
        private bool isLocked = false;

        // 로그인 제한 시작 시간 저장
        private DateTime lockTime;

        // =========================================================
        // ▲▲▲ 새로 추가한 로그인 제한 기능 ▲▲▲
        // =========================================================

        public Form1()
        {
            InitializeComponent();

            // =========================================================
            // ▼▼▼ 새로 추가한 초기 설정 ▼▼▼
            // =========================================================

            // 로그인 제한 타이머 시작
            timerLoginLock.Start();

            // 비밀번호 입력 시 * 표시
            txtPassword.UseSystemPasswordChar = true;

            // =========================================================
            // ▲▲▲ 새로 추가한 초기 설정 ▲▲▲
            // =========================================================

            // 디자이너에서는 아래 런타임 초기화를 실행하지 않음
            if (System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            // setup DataGridView columns and binding
            dgvEntries.AutoGenerateColumns = false;
            dgvEntries.Columns.Clear();

            var colDate = new DataGridViewTextBoxColumn
            {
                HeaderText = "날짜",
                DataPropertyName = "Date",
                Width = 90
            };

            var colType = new DataGridViewTextBoxColumn
            {
                HeaderText = "구분",
                DataPropertyName = "Type",
                Width = 60
            };

            var colCategory = new DataGridViewTextBoxColumn
            {
                HeaderText = "카테고리",
                DataPropertyName = "Category",
                Width = 100
            };

            var colDesc = new DataGridViewTextBoxColumn
            {
                HeaderText = "메모",
                DataPropertyName = "Description",
                AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill
            };

            var colAmount =
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "금액",
                    DataPropertyName = "Amount",
                    Width = 120,
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.AllCells,
                    MinimumWidth = 80,
                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight,
                            WrapMode =
                                DataGridViewTriState.False
                        }
                };

            dgvEntries.Columns.AddRange(
                new DataGridViewColumn[]
                {
                    colDate,
                    colType,
                    colCategory,
                    colDesc,
                    colAmount
                });

            dgvEntries.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            dgvEntries.DataSource = _transactions;

            _transactions.ListChanged +=
                (s, e) => UpdateTotal();

            try
            {
                LoadTransactionsFromJson();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "데이터 로드 오류: " + ex.Message);
            }

            UpdateTotal();

            UpdateUiForLoginState(false);

            try
            {
                var cb =
                    this.Controls.Find(
                        "chkDarkMode",
                        true)
                    .FirstOrDefault() as CheckBox;

                ApplyDarkMode(
                    cb != null && cb.Checked);
            }
            catch { }
        }

        private void UpdateUiForLoginState(
            bool loggedIn)
        {
            try
            {
                dtpDate.Enabled = loggedIn;

                cbType.Enabled = loggedIn;

                try
                {
                    var sel =
                        cbType?.SelectedItem as string;

                    cbCategory.Enabled =
                        loggedIn &&
                        string.Equals(sel, "지출");
                }
                catch
                {
                    cbCategory.Enabled = false;
                }

                txtDescription.Enabled = loggedIn;

                nudAmount.Enabled = loggedIn;

                btnAdd.Enabled = loggedIn;

                btnDelete.Enabled = loggedIn;

                dgvEntries.Enabled = loggedIn;

                btnResearch.Enabled = loggedIn;

                lblTotal.Enabled = loggedIn;
            }
            catch { }
        }

        private void LoadUserTransactions()
        {
            _transactions =
                new BindingList<Transaction>();

            dgvEntries.DataSource =
                _transactions;

            _transactions.ListChanged +=
                (s, e) => UpdateTotal();

            if (string.IsNullOrEmpty(_currentUser))
                return;

            var path =
                UserTransactionsFile(_currentUser);

            if (!File.Exists(path))
                return;

            try
            {
                using (var fs =
                    File.OpenRead(path))
                {
                    var ser =
                        new DataContractJsonSerializer(
                            typeof(List<Transaction>));

                    var list =
                        ser.ReadObject(fs)
                        as List<Transaction>;

                    if (list != null)
                    {
                        foreach (var t in list)
                            _transactions.Add(t);
                    }
                }
            }
            catch { }

            UpdateTotal();
        }

        private void SaveUserTransactions()
        {
            if (string.IsNullOrEmpty(_currentUser))
                return;

            var list =
                _transactions.ToList();

            var path =
                UserTransactionsFile(_currentUser);

            try
            {
                using (var fs =
                    File.Create(path))
                {
                    var ser =
                        new DataContractJsonSerializer(
                            typeof(List<Transaction>));

                    ser.WriteObject(fs, list);
                }
            }
            catch { }
        }

        private string UserTransactionsFile(
            string user)
        {
            var dir =
                Path.Combine(
                    Application.StartupPath,
                    "users");

            Directory.CreateDirectory(dir);

            return Path.Combine(
                dir,
                user + "_transactions.json");
        }

        private void btnLogin_Click(
            object sender,
            EventArgs e)
        {
            // =========================================================
            // ▼▼▼ 새로 추가한 로그인 제한 확인 ▼▼▼
            // =========================================================

            // 로그인 제한 상태인지 확인
            if (isLocked)
            {
                MessageBox.Show(
                    "로그인이 제한되었습니다.");

                return;
            }

            // =========================================================
            // ▲▲▲ 새로 추가한 로그인 제한 확인 ▲▲▲
            // =========================================================

            string user =
                txtUsername.Text.Trim();

            string pass =
                txtPassword.Text;

            if (string.IsNullOrEmpty(user) ||
                string.IsNullOrEmpty(pass))
            {
                MessageBox.Show(
                    "사용자명과 비밀번호를 입력하세요.",
                    "경고",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (UsersStore.Validate(user, pass))
            {
                // =========================================================
                // ▼▼▼ 새로 추가한 로그인 성공 처리 ▼▼▼
                // =========================================================

                // 로그인 성공 시 실패 횟수 초기화
                loginFailCount = 0;

                // 로그인 실패 경고문 숨김
                lblWarning.Visible = false;

                // =========================================================
                // ▲▲▲ 새로 추가한 로그인 성공 처리 ▲▲▲
                // =========================================================

                _currentUser = user;

                MessageBox.Show(
                    "로그인 성공",
                    "정보",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadUserTransactions();

                UpdateUiForLoginState(true);

                this.Text =
                    $"가계부 - {_currentUser}";
            }
            else
            {
                MessageBox.Show(
                    "로그인 실패: 아이디 또는 비밀번호가 일치하지 않습니다.",
                    "경고",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // =========================================================
                // ▼▼▼ 새로 추가한 로그인 실패 제한 기능 ▼▼▼
                // =========================================================

                // 로그인 실패 횟수 증가
                loginFailCount++;

                // 5회 이상 실패 시 로그인 제한
                if (loginFailCount >= 5)
                {
                    // 로그인 제한 활성화
                    isLocked = true;

                    // 제한 시작 시간 저장
                    lockTime = DateTime.Now;

                    // 로그인 버튼 비활성화
                    btnLogin.Enabled = false;

                    // 경고문 표시
                    lblWarning.Visible = true;
                }

                // =========================================================
                // ▲▲▲ 새로 추가한 로그인 실패 제한 기능 ▲▲▲
                // =========================================================
            }
        }

        private void btnRegister_Click(
            object sender,
            EventArgs e)
        {
            using (var f =
                new RegisterForm())
            {
                if (f.ShowDialog() ==
                    DialogResult.OK)
                {
                    if (UsersStore.AddUser(
                        f.Username,
                        f.Password))
                    {
                        MessageBox.Show(
                            "회원가입 성공. 로그인 해주세요.",
                            "정보",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            "이미 존재하는 사용자입니다.",
                            "경고",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnSignUp_Click(
            object sender,
            EventArgs e)
        {
            btnRegister_Click(sender, e);
        }

        private void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            var now = DateTime.Now;

            if ((now - _lastAdd)
                .TotalMilliseconds < 500)
                return;

            _lastAdd = now;

            btnAdd.Enabled = false;

            try
            {
                DateTime date =
                    dtpDate.Value.Date;

                string type =
                    cbType.SelectedItem as string
                    ?? "지출";

                string category =
                    cbCategory.SelectedItem as string
                    ?? "식비";

                string desc =
                    txtDescription.Text.Trim();

                decimal amount =
                    nudAmount.Value;

                if (type == "지출")
                    amount = -Math.Abs(amount);
                else
                    amount = Math.Abs(amount);

                var t = new Transaction
                {
                    Date = date,
                    Type = type,
                    Category = category,
                    Description = desc,
                    Amount = amount
                };

                _transactions.Add(t);

                SaveUserTransactions();

                txtDescription.Clear();

                nudAmount.Value = 0;
            }
            finally
            {
                btnAdd.Enabled = true;
            }
        }

        private void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (dgvEntries.SelectedRows.Count > 0)
            {
                var toRemove =
                    dgvEntries.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Select(r => r.DataBoundItem)
                    .OfType<Transaction>()
                    .ToList();

                foreach (var t in toRemove)
                {
                    _transactions.Remove(t);
                }

                SaveUserTransactions();
            }
        }

        private void UpdateTotal()
        {
            decimal total =
                _transactions.Sum(t => t.Amount);

            lblTotal.Text =
                $"총합: {FormatWon(total)}";
        }

        private string JsonFilePath =>
            Path.Combine(
                Application.StartupPath,
                "transactions.json");

        private void LoadTransactionsFromJson()
        {
            _transactions.Clear();

            if (!File.Exists(JsonFilePath))
                return;

            using (var fs =
                File.OpenRead(JsonFilePath))
            {
                var ser =
                    new DataContractJsonSerializer(
                        typeof(List<Transaction>));

                var list =
                    ser.ReadObject(fs)
                    as List<Transaction>;

                if (list != null)
                {
                    foreach (var t in list)
                        _transactions.Add(t);
                }
            }
        }

        private string FormatWon(decimal value)
        {
            long v =
                (long)Math.Round(value, 0);

            string abs =
                Math.Abs(v).ToString("N0");

            return (v < 0 ? "-" : "")
                + abs + "원";
        }

        // =========================================================
        // ▼▼▼ 새로 추가한 비밀번호 보기 기능 ▼▼▼
        // =========================================================

        private void btnShowPassword_Click(
            object sender,
            EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !txtPassword.UseSystemPasswordChar;
        }

        // =========================================================
        // ▲▲▲ 새로 추가한 비밀번호 보기 기능 ▲▲▲
        // =========================================================

        // =========================================================
        // ▼▼▼ 새로 추가한 로그인 제한 타이머 ▼▼▼
        // =========================================================

        private void timerLoginLock_Tick(
            object sender,
            EventArgs e)
        {
            if (isLocked)
            {
                TimeSpan diff =
                    DateTime.Now - lockTime;

                // 5분 후 로그인 제한 해제
                if (diff.TotalMinutes >= 5)
                {
                    isLocked = false;

                    loginFailCount = 0;

                    btnLogin.Enabled = true;

                    lblWarning.Visible = false;

                    MessageBox.Show(
                        "로그인 제한이 해제되었습니다.");
                }
            }
        }

        // =========================================================
        // ▲▲▲ 새로 추가한 로그인 제한 타이머 ▲▲▲
        // =========================================================

        private void chkDarkMode_CheckedChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                var cb =
                    sender as CheckBox;

                bool enabled =
                    cb != null && cb.Checked;

                ApplyDarkMode(enabled);
            }
            catch { }
        }

        private void ApplyDarkMode(bool dark)
        {
            if (dark)
            {
                this.BackColor =
                    Color.FromArgb(32, 32, 32);

                this.ForeColor =
                    Color.White;
            }
            else
            {
                this.BackColor =
                    SystemColors.Control;

                this.ForeColor =
                    SystemColors.ControlText;
            }
        }

        [DataContract]
        private class Transaction
        {
            [DataMember]
            public int Id { get; set; }

            [DataMember]
            public DateTime Date { get; set; }

            [DataMember]
            public string Type { get; set; }

            [DataMember]
            public string Category { get; set; }

            [DataMember]
            public string Description { get; set; }

            [DataMember]
            public decimal Amount { get; set; }
        }
    }
}