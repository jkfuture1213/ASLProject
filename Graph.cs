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
    public partial class Graph : Form
    {
        private List<Tuple<string, decimal>> _data;

        public Graph()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(600, 400);
        }

        public Graph(List<Tuple<string, decimal>> data, string title) : this()
        {
            _data = data ?? new List<Tuple<string, decimal>>();
            this.Text = title ?? this.Text;
            // create a PictureBox to render the pie
            var pb = new PictureBox { Dock = DockStyle.Fill, BackColor = Color.White };
            pb.Paint += Pb_Paint;
            this.Controls.Add(pb);
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
                    g.DrawString("데이터 없음", f, b, 10, 10);
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
