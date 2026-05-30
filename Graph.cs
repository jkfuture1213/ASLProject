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
        public Graph()
        {
            InitializeComponent();

            this.Paint += Graph_Paint;
        }

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            decimal income = Form1.incomeList.Sum();
            decimal spend = Math.Abs(Form1.spendList.Sum());
            decimal total = income + spend;
            decimal balance = income - spend;

            if (total <= 0) {
                g.DrawString("데이터가 존재하지 않습니다.", new Font("Arial", 12), Brushes.Black, 50, 30);
                return;
            }

            float incomeAngle = (float)(income / total * 360);
            float spendAngle = (float)(spend / total * 360);

            Rectangle rect = new Rectangle(130, 110, 150, 150);

            g.FillPie(Brushes.Green, rect, 0, incomeAngle);
            g.FillPie(Brushes.Red, rect, incomeAngle, spendAngle);

            g.FillEllipse(new SolidBrush(Form1.DefaultBackColor), 170, 150, 70, 70);

            g.DrawString("수입: " + income.ToString("C"), new Font("Arial", 10), Brushes.Green, 50, 30);
            g.DrawString("지출: " + spend.ToString("C"), new Font("Arial", 10), Brushes.Red, 50, 50);
            g.DrawString("잔액: " + balance.ToString("C"), new Font("Arial", 10), Brushes.Black, 50, 70);
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
