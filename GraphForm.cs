using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartAccountBook
{
    public partial class GraphForm : Form
    {
        private List<Tuple<string, decimal>> _data;
        private List<Transaction> _transactions;

        public GraphForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(700, 420);
        }

        public GraphForm(List<Transaction> transactions, string title) : this()
        {
            _transactions = transactions?.ToList() ?? new List<Transaction>();
            this.Text = title ?? this.Text;

            // 폼 디자이너에서 선언한 컨트롤 사용 (cbYear, cbMonth, btnApply, pbGraph)
            // initialize references
            try
            {
                cbYear.Items.Clear();
                cbMonth.Items.Clear();
                // populate year/month using transaction data
                PopulateYearMonth();
                btnApply.Click += (s, e) => { UpdateDataAndRefresh(); };
                // initial render
                UpdateDataAndRefresh();
            }
            catch { }
        }

        private void PopulateYearMonth()
        {
            cbYear.Items.Clear();
            cbYear.Items.Add("전체");
            var years = _transactions.Select(t => t.Date.Year).Distinct().OrderByDescending(y => y).ToList();
            foreach (var y in years) cbYear.Items.Add(y.ToString());
            cbYear.SelectedIndex = 0;

            cbMonth.Items.Clear();
            cbMonth.Items.Add("전체");
            for (int m = 1; m <= 12; m++) cbMonth.Items.Add(m.ToString());
            cbMonth.SelectedIndex = 0;
        }

        private void UpdateDataAndRefresh()
        {
            int selYear = 0;
            int selMonth = 0;
            try
            {
                if (cbYear.SelectedItem != null && cbYear.SelectedItem.ToString() != "전체") selYear = int.Parse(cbYear.SelectedItem.ToString());
                if (cbMonth.SelectedItem != null && cbMonth.SelectedItem.ToString() != "전체") selMonth = int.Parse(cbMonth.SelectedItem.ToString());
            }
            catch { }

            var filtered = _transactions.Where(t => t.Amount < 0);
            if (selYear != 0) filtered = filtered.Where(t => t.Date.Year == selYear);
            if (selMonth != 0) filtered = filtered.Where(t => t.Date.Month == selMonth);

            var list = filtered.ToList();
            if (list.Count == 0)
            {
                _data = new List<Tuple<string, decimal>>();
            }
            else
            {
                _data = list.GroupBy(t => string.IsNullOrEmpty(t.Category) ? "미분류" : t.Category)
                    .Select(g => Tuple.Create(g.Key, -g.Sum(t => t.Amount)))
                    .OrderByDescending(x => x.Item2)
                    .ToList();
            }

            try
            {
                // pbGraph는 디자이너에서 생성됨
                if (this.Controls.OfType<System.Windows.Forms.PictureBox>().Any())
                {
                    var pic = this.Controls.OfType<System.Windows.Forms.PictureBox>().First();
                    pic.Invalidate();
                }
            }
            catch { }
        }

        private void Pb_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (_data == null || _data.Count == 0)
            {
                using (var f = new Font(FontFamily.GenericSansSerif, 12))
                using (var b = new SolidBrush(Color.Black))
                {
                    g.DrawString("데이터 없음", f, b, 12, 12);
                }
                return;
            }

            decimal total = _data.Sum(x => x.Item2);
            if (total <= 0)
            {
                using (var f = new Font(FontFamily.GenericSansSerif, 12))
                using (var b = new SolidBrush(Color.Black))
                {
                    g.DrawString("유효한 지출 데이터가 없습니다.", f, b, 10, 10);
                }
                return;
            }

            // define pie area and legend area
            var rect = new Rectangle(20, 20, 300, 300);
            int legendX = rect.Right + 20;
            int legendY = rect.Top;

            var colors = new Color[] { Color.FromArgb(91, 155, 213), Color.FromArgb(237, 125, 49), Color.FromArgb(165, 165, 165), Color.FromArgb(255, 192, 0), Color.FromArgb(112, 173, 71), Color.FromArgb(155, 89, 182), Color.FromArgb(230, 126, 34) };

            float startAngle = 0f;
            int i = 0;
            using (var font = new Font(FontFamily.GenericSansSerif, 10))
            using (var brush = new SolidBrush(Color.Black))
            {
                foreach (var item in _data)
                {
                    float sweep = (float)((double)item.Item2 / (double)total * 360.0);
                    var c = colors[i % colors.Length];
                    using (var b = new SolidBrush(c))
                    {
                        g.FillPie(b, rect, startAngle, sweep);
                    }
                    // legend
                    var legendRect = new Rectangle(legendX, legendY + i * 24, 16, 16);
                    g.FillRectangle(new SolidBrush(c), legendRect);
                    g.DrawRectangle(Pens.Black, legendRect);
                    string text = string.Format("{0}: {1:N0}원 ({2}%)", item.Item1, item.Item2, Math.Round((double)item.Item2 / (double)total * 100.0, 1));
                    g.DrawString(text, font, brush, legendX + 22, legendY + i * 24);

                    startAngle += sweep;
                    i++;
                }
            }
        }
    }
}
