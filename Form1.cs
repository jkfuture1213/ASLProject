using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SmartAccountBook
{
    public partial class Form1 : Form
    {
        private BindingList<Transaction> _transactions = new BindingList<Transaction>();
        private DateTime _lastAdd = DateTime.MinValue;
        private string _currentUser = null;
        private bool _darkMode = false;


        public Form1(string userID)
        {
            InitializeComponent();
            _currentUser = userID;

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
            var colAmount = new DataGridViewTextBoxColumn
            {
                HeaderText = "금액",
                DataPropertyName = "Amount",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                MinimumWidth = 80,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    WrapMode = DataGridViewTriState.False
                }
            };

            dgvEntries.Columns.AddRange(new DataGridViewColumn[] { colDate, colType, colCategory, colDesc, colAmount });
            // Use per-column sizing modes (do not override with a global AutoSizeColumnsMode)
            dgvEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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
            // 초기 다크 모드 적용 상태는 false
            ApplyDarkMode(_darkMode);
            // 입력 관련 컨트롤은 로그인 전 비활성화
            //UpdateUiForLoginState(false);
        }

        private void btnToggleDarkMode_Click(object sender, EventArgs e)
        {
            _darkMode = !_darkMode;
            ApplyDarkMode(_darkMode);
        }

        private void ApplyDarkMode(bool enabled)
        {
            Color back = enabled ? Color.FromArgb(30, 30, 30) : SystemColors.Control;
            Color panelBack = enabled ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
            Color fore = enabled ? Color.White : SystemColors.ControlText;

            // Form and main panels
            this.BackColor = back;
            try { panelMiddle.BackColor = panelBack; } catch { }
            try { panelRight.BackColor = panelBack; } catch { }

            // Set colors for controls within panels
            void SetControlColors(Control parent)
            {
                foreach (Control c in parent.Controls)
                {
                    try
                    {
                        // Text-based controls
                        if (c is Label)
                        {
                            c.BackColor = Color.Transparent;
                            c.ForeColor = fore;
                        }
                        else if (c is Button btn)
                        {
                            btn.BackColor = enabled ? panelBack : SystemColors.Control;
                            btn.ForeColor = fore;
                            try { btn.FlatStyle = FlatStyle.Flat; } catch { }
                            try { btn.FlatAppearance.BorderColor = enabled ? Color.FromArgb(70, 70, 73) : SystemColors.ControlDark; } catch { }
                        }
                        else if (c is ComboBox cb)
                        {
                            cb.BackColor = enabled ? Color.FromArgb(37, 37, 38) : SystemColors.Window;
                            cb.ForeColor = fore;
                        }
                        else if (c is CheckBox || c is RadioButton)
                        {
                            c.BackColor = Color.Transparent;
                            c.ForeColor = fore;
                        }
                        else if (c is TextBox || c is NumericUpDown)
                        {
                            c.BackColor = enabled ? Color.FromArgb(37, 37, 38) : SystemColors.Window;
                            c.ForeColor = fore;
                        }
                        else if (c is DateTimePicker)
                        {
                            c.ForeColor = fore;
                        }
                        else if (c is DataGridView dgv)
                        {
                            dgv.EnableHeadersVisualStyles = false;
                            dgv.BackgroundColor = panelBack;
                            dgv.DefaultCellStyle.BackColor = enabled ? Color.FromArgb(37, 37, 38) : SystemColors.Window;
                            dgv.DefaultCellStyle.ForeColor = fore;
                            dgv.ColumnHeadersDefaultCellStyle.BackColor = enabled ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
                            dgv.ColumnHeadersDefaultCellStyle.ForeColor = fore;
                            dgv.RowHeadersDefaultCellStyle.BackColor = dgv.ColumnHeadersDefaultCellStyle.BackColor;
                            dgv.RowHeadersDefaultCellStyle.ForeColor = fore;
                        }
                        else
                        {
                            c.BackColor = Color.Transparent;
                            c.ForeColor = fore;
                        }
                    }
                    catch { }

                    if (c.HasChildren) SetControlColors(c);
                }
            }

            try { SetControlColors(this); } catch { }

            // Special cases
            try { txtAnalysis.BackColor = enabled ? Color.FromArgb(37, 37, 38) : SystemColors.Window; txtAnalysis.ForeColor = fore; } catch { }
            try { txtDescription.BackColor = enabled ? Color.FromArgb(37, 37, 38) : SystemColors.Window; txtDescription.ForeColor = fore; } catch { }

            // Update toggle button text
            try { btnToggleDarkMode.Text = enabled ? "라이트 모드" : "다크 모드"; } catch { }
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
                .Select(g => Tuple.Create(g.Key, -g.Sum(t => t.Amount)))
                .OrderByDescending(x => x.Item2)
                .ToList();

            var sb = new StringBuilder();
            decimal grand = 0;
            foreach (var c in byCat)
            {
                grand += c.Item2;
                sb.AppendLine($"{c.Item1}: {FormatWon(c.Item2)}");
            }
            sb.AppendLine("---------------------------");
            sb.AppendLine($"총지출: {FormatWon(grand)}");

            // 전체 잔액(남은 돈)을 계산하고 추천을 표시
            decimal balance = _transactions.Sum(t => t.Amount);
            sb.AppendLine($"남은돈: {FormatWon(balance)}");
            sb.AppendLine();
            var recs = RecommendUsage(balance, byCat);
            sb.AppendLine("추천 사용처:");
            foreach (var r in recs)
            {
                sb.AppendLine(r);
            }

            // 결과를 텍스트박스에 표시 (자동 줄바꿈 적용됨)
            txtAnalysis.Text = sb.ToString();
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

        private string JsonFilePath => Path.Combine(Application.StartupPath, $"{_currentUser}_transactions.json");

        public string ID { get; }

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

        private List<string> RecommendUsage(decimal balance, List<Tuple<string, decimal>> byCategory)
        {
            var rec = new List<string>();
            if (balance <= 0)
            {
                rec.Add("현재 잔액이 없습니다. 지출을 줄이고 비상금 확보를 권장합니다.");
                return rec;
            }

            // 기본 권장 비율: 저축 30%, 생활비 50%, 여유/기타 20%
            decimal save = Math.Floor(balance * 0.30m);
            decimal living = Math.Floor(balance * 0.50m);
            decimal extra = balance - save - living;
            rec.Add($"저축(권장 30%): {FormatWon(save)}");
            rec.Add($"생활비(권장 50%): {FormatWon(living)}");
            rec.Add($"여유/목적자금(약 20%): {FormatWon(extra)}");

            // 카테고리 기반 추가 권장
            if (byCategory != null && byCategory.Count > 0)
            {
                var top = byCategory.Take(3).ToList();
                rec.Add("");
                rec.Add("최근 많이 쓴 항목 기반 제안:");
                foreach (var t in top)
                {
                    var percent = Math.Round((double)(t.Item2 / Math.Max(1, byCategory.Sum(x => x.Item2))) * 100, 0);
                    rec.Add($"- {t.Item1}: 최근 지출 비중 {percent}% 이므로 관련 비용 절감 또는 예산 재배치 고려");
                }
            }

            rec.Add("");
            rec.Add("권장: 비상금은 최소 3개월 생활비 정도를 목표로 하세요.");
            return rec;
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
