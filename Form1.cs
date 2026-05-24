using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartAccountBook
{
    public partial class Form1 : Form
    {
        private BindingList<Transaction> _transactions = new BindingList<Transaction>();
        private DateTime _lastAdd = DateTime.MinValue;
        private string _currentUser = null;

        public Form1()
        {
            InitializeComponent();

            // 디자이너에서는 아래 런타임 초기화를 실행하지 않음
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            // setup DataGridView columns and binding
            dgvEntries.AutoGenerateColumns = false;
            dgvEntries.Columns.Clear();

            var colDate = new DataGridViewTextBoxColumn { HeaderText = "날짜", DataPropertyName = "Date", Width = 90 };
            var colType = new DataGridViewTextBoxColumn { HeaderText = "구분", DataPropertyName = "Type", Width = 60 };
            var colCategory = new DataGridViewTextBoxColumn { HeaderText = "카테고리", DataPropertyName = "Category", Width = 100 };
            var colDesc = new DataGridViewTextBoxColumn { HeaderText = "메모", DataPropertyName = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var colAmount = new DataGridViewTextBoxColumn { HeaderText = "금액", DataPropertyName = "Amount", Width = 120, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } };

            dgvEntries.Columns.AddRange(new DataGridViewColumn[] { colDate, colType, colCategory, colDesc, colAmount });
            dgvEntries.DataSource = _transactions;

            _transactions.ListChanged += (s, e) => UpdateTotal();

            try
            {
                LoadTransactionsFromJson();
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 로드 오류: " + ex.Message);
            }

            UpdateTotal();
            // 입력 관련 컨트롤은 로그인 전 비활성화
            UpdateUiForLoginState(false);
        }

        private void UpdateUiForLoginState(bool loggedIn)
        {
            // 입력 가능한 컨트롤들만 활성/비활성 처리
            try
            {
                dtpDate.Enabled = loggedIn;
                cbType.Enabled = loggedIn;
                // 카테고리는 로그인 상태이면서 구분이 '지출'으로 선택된 경우에만 활성화
                try
                {
                    var sel = cbType?.SelectedItem as string;
                    cbCategory.Enabled = loggedIn && string.Equals(sel, "지출");
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
            catch
            {
                // 디자이너 로드 시 일부 컨트롤이 null일 수 있으니 무시
            }
        }

        private void LoadUserTransactions()
        {
            _transactions.ListChanged -= (s, e) => UpdateTotal();
            _transactions = new BindingList<Transaction>();
            dgvEntries.DataSource = _transactions;
            _transactions.ListChanged += (s, e) => UpdateTotal();

            if (string.IsNullOrEmpty(_currentUser)) return;
            var path = UserTransactionsFile(_currentUser);
            if (!File.Exists(path)) return;
            try
            {
                using (var fs = File.OpenRead(path))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<Transaction>));
                    var list = ser.ReadObject(fs) as List<Transaction>;
                    if (list != null) foreach (var t in list) _transactions.Add(t);
                }
            }
            catch { }
            UpdateTotal();
        }

        private void SaveUserTransactions()
        {
            if (string.IsNullOrEmpty(_currentUser)) return;
            var list = _transactions.ToList();
            var path = UserTransactionsFile(_currentUser);
            try
            {
                using (var fs = File.Create(path))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<Transaction>));
                    ser.WriteObject(fs, list);
                }
            }
            catch { }
        }

        private string UserTransactionsFile(string user)
        {
            var dir = Path.Combine(Application.StartupPath, "users");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, user + "_transactions.json");
        }

        private void btnResearch_Click(object sender, EventArgs e)
        {
            // 지출(음수) 항목만 분석하여 카테고리별 합계를 표시
            var expenses = _transactions.Where(t => t.Amount < 0).ToList();
            var byCat = expenses
                .GroupBy(t => string.IsNullOrEmpty(t.Category) ? "미분류" : t.Category)
                .Select(g => new { Category = g.Key, Total = -g.Sum(t => t.Amount) })
                .OrderByDescending(x => x.Total)
                .ToList();

            lstAnalysis.Items.Clear();
            decimal grand = 0;
            foreach (var c in byCat)
            {
                grand += c.Total;
                lstAnalysis.Items.Add($"{c.Category}: {FormatWon(c.Total)}");
            }
            lstAnalysis.Items.Add("---------------------------");
            lstAnalysis.Items.Add($"총지출: {FormatWon(grand)}");
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sel = cbType.SelectedItem as string;
                if (string.IsNullOrEmpty(sel))
                {
                    // 타입이 선택되지 않았으면 카테고리 비활성화
                    cbCategory.Enabled = false;
                    return;
                }

                if (sel == "수입")
                {
                    // 수입이면 카테고리는 월급으로 설정하고 비활성화
                    if (!cbCategory.Items.Contains("월급")) cbCategory.Items.Insert(0, "월급");
                    cbCategory.SelectedItem = "월급";
                    cbCategory.Enabled = false;
                }
                else
                {
                    // 지출이면 카테고리 활성화
                    cbCategory.Enabled = true;
                    if (cbCategory.Items.Count > 0 && cbCategory.SelectedItem == null) cbCategory.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("사용자명과 비밀번호를 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UsersStore.Validate(user, pass))
            {
                _currentUser = user;
                MessageBox.Show("로그인 성공", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUserTransactions();
                UpdateUiForLoginState(true);
                this.Text = $"가계부 - {_currentUser}";
            }
            else
            {
                MessageBox.Show("로그인 실패: 아이디 또는 비밀번호가 일치하지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (var f = new RegisterForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // Create user
                    if (UsersStore.AddUser(f.Username, f.Password))
                    {
                        MessageBox.Show("회원가입 성공. 로그인 해주세요.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("이미 존재하는 사용자입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // wrapper for designer btnSignUp Click
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            btnRegister_Click(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            if ((now - _lastAdd).TotalMilliseconds < 500) return;
            _lastAdd = now;

            btnAdd.Enabled = false;
            try
            {
                DateTime date = dtpDate.Value.Date;
                string type = cbType.SelectedItem as string ?? "지출";
                string category = cbCategory.SelectedItem as string ?? "식비";
                string desc = txtDescription.Text.Trim();
                decimal amount = nudAmount.Value;

                // ignore adding if amount is zero and no description/category
                if (amount == 0 && string.IsNullOrEmpty(category) && string.IsNullOrEmpty(desc))
                    return;

                // 지출이면 금액을 음수로 저장하여 총합 계산에 반영
                if (type == "지출") amount = -Math.Abs(amount);
                else amount = Math.Abs(amount);

                var t = new Transaction
                {
                    Date = date,
                    Type = type,
                    Category = category,
                    Description = desc,
                    Amount = amount
                };

                _transactions.Add(t);
                try { SaveUserTransactions(); } catch { }

                // 입력 필드 초기화
                txtDescription.Clear();
                nudAmount.Value = 0;
            }
            finally
            {
                btnAdd.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEntries.SelectedRows.Count > 0)
            {
                var toRemove = dgvEntries.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Select(r => r.DataBoundItem)
                    .OfType<Transaction>()
                    .ToList();

                foreach (var t in toRemove)
                {
                    _transactions.Remove(t);
                }
                try { SaveUserTransactions(); } catch { }
            }
            else if (_transactions.Count > 0)
            {
                var last = _transactions[_transactions.Count - 1];
                _transactions.RemoveAt(_transactions.Count - 1);
                try { SaveUserTransactions(); } catch { }
            }
        }

        private void UpdateTotal()
        {
            decimal total = _transactions.Sum(t => t.Amount);
            lblTotal.Text = $"총합: {FormatWon(total)}";
        }

        private string JsonFilePath => Path.Combine(Application.StartupPath, "transactions.json");

        private void LoadTransactionsFromJson()
        {
            _transactions.Clear();
            if (!File.Exists(JsonFilePath)) return;
            using (var fs = File.OpenRead(JsonFilePath))
            {
                var ser = new DataContractJsonSerializer(typeof(List<Transaction>));
                var list = ser.ReadObject(fs) as List<Transaction>;
                if (list != null)
                {
                    foreach (var t in list) _transactions.Add(t);
                }
            }
        }

        private void SaveTransactionsToJson()
        {
            var list = _transactions.ToList();
            using (var fs = File.Create(JsonFilePath))
            {
                var ser = new DataContractJsonSerializer(typeof(List<Transaction>));
                ser.WriteObject(fs, list);
            }
        }

        private void dgvEntries_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null || e.ColumnIndex < 0) return;

            var col = dgv.Columns[e.ColumnIndex];
            if (col.DataPropertyName == "Amount" && e.Value != null)
            {
                if (e.Value is decimal dec) e.Value = FormatWon(dec);
                else if (decimal.TryParse(e.Value.ToString(), out dec)) e.Value = FormatWon(dec);
                e.FormattingApplied = true;
            }

            if (col.DataPropertyName == "Date" && e.Value != null)
            {
                if (e.Value is DateTime dt) e.Value = dt.ToString("yyyy-MM-dd");
                e.FormattingApplied = true;
            }
        }

        private string FormatWon(decimal value)
        {
            // 정수 단위로 처리, 음수 처리 포함
            long v = (long)Math.Round(value, 0);
            string abs = Math.Abs(v).ToString("N0");
            return (v < 0 ? "-" : "") + abs + "원";
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
