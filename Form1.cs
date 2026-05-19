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

        public Form1()
        {
            InitializeComponent();


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
                try { SaveTransactionsToJson(); } catch { }

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
                try { SaveTransactionsToJson(); } catch { }
            }
            else if (_transactions.Count > 0)
            {
                var last = _transactions[_transactions.Count - 1];
                _transactions.RemoveAt(_transactions.Count - 1);
                try { SaveTransactionsToJson(); } catch { }
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
