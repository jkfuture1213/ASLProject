using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartAccountBook
{
    // Simple DTO for analysis results
    public class AnalysisResult
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal TotalSpend { get; set; }
        public Dictionary<string, decimal> ByCategory { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> CategoryRatio { get; set; } = new Dictionary<string, decimal>();
        public decimal PrevTotalSpend { get; set; }
        public decimal ChangePercent { get; set; }
        public List<string> OverSpending { get; set; } = new List<string>();
        public List<string> FixedExpenses { get; set; } = new List<string>();
        public string SpendingProfile { get; set; }
        public int SavingScore { get; set; }
    }

    // RuleEngine encapsulates rules used for analysis
    public static class RuleEngine
    {
        // Detect fixed expenses: categories that appear every month with similar amounts
        public static List<string> DetectFixedExpenses(List<Transaction> txs, DateTime asOf)
        {
            var months = txs.GroupBy(t => new DateTime(t.Date.Year, t.Date.Month, 1));
            var candidates = txs.GroupBy(t => t.Category ?? "미분류")
                .Select(g => new { Cat = g.Key, Months = g.GroupBy(t => new DateTime(t.Date.Year, t.Date.Month, 1)).Count(), Avg = -g.Where(t=>t.Amount<0).Sum(t=>t.Amount) / Math.Max(1, g.GroupBy(t => new DateTime(t.Date.Year, t.Date.Month, 1)).Count()) })
                .Where(x => x.Months >= Math.Max(2, months.Count() - 1))
                .OrderByDescending(x => x.Months)
                .Take(10)
                .Select(x => x.Cat)
                .ToList();
            return candidates;
        }

        // Detect overspending: categories whose current month ratio >> historical average
        public static List<string> DetectOverSpending(Dictionary<string, decimal> currentByCat, Dictionary<string, decimal> historicalAvg)
        {
            var list = new List<string>();
            foreach (var kv in currentByCat)
            {
                var cat = kv.Key;
                var cur = kv.Value;
                var avg = historicalAvg.ContainsKey(cat) ? historicalAvg[cat] : 0m;
                if (avg <= 0) continue;
                if (cur > avg * 1.5m) list.Add(cat);
            }
            return list;
        }

        public static int ComputeSavingScore(decimal total, decimal incomeEstimate, Dictionary<string, decimal> byCat)
        {
            // naive scoring: higher savings => higher score
            if (incomeEstimate <= 0) incomeEstimate = Math.Max(1, total * 2);
            decimal ratio = 1m - Math.Min(1m, total / incomeEstimate);
            int score = (int)Math.Round((double)(ratio * 100));
            return Math.Max(0, Math.Min(100, score));
        }
    }

    public class AnalysisEngine
    {
        // Analyze transactions for the month that contains 'asOf'
        public AnalysisResult Analyze(List<Transaction> transactions, DateTime asOf)
        {
            var result = new AnalysisResult();
            var monthStart = new DateTime(asOf.Year, asOf.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            result.PeriodStart = monthStart;
            result.PeriodEnd = monthEnd;

            var monthTx = transactions.Where(t => t.Date.Date >= monthStart && t.Date.Date <= monthEnd && t.Amount < 0).ToList();
            result.TotalSpend = -monthTx.Sum(t => t.Amount);

            // by category
            var byCat = monthTx.GroupBy(t => string.IsNullOrEmpty(t.Category) ? "미분류" : t.Category)
                .ToDictionary(g => g.Key, g => -g.Sum(t => t.Amount));
            result.ByCategory = byCat;

            decimal grand = result.TotalSpend > 0 ? result.TotalSpend : 1m;
            result.CategoryRatio = byCat.ToDictionary(k => k.Key, k => Math.Round(k.Value / grand * 100m, 1));

            // previous month comparison
            var prevStart = monthStart.AddMonths(-1);
            var prevEnd = monthStart.AddDays(-1);
            var prevTx = transactions.Where(t => t.Date.Date >= prevStart && t.Date.Date <= prevEnd && t.Amount < 0).ToList();
            result.PrevTotalSpend = -prevTx.Sum(t => t.Amount);
            if (result.PrevTotalSpend > 0)
                result.ChangePercent = Math.Round((result.TotalSpend - result.PrevTotalSpend) / result.PrevTotalSpend * 100m, 1);
            else
                result.ChangePercent = result.TotalSpend > 0 ? 100m : 0m;

            // historical averages per category (over all previous months)
            var hist = transactions.Where(t => t.Date < monthStart && t.Amount < 0)
                .GroupBy(t => new { t.Category, Month = new DateTime(t.Date.Year, t.Date.Month, 1) })
                .GroupBy(g => g.Key.Category)
                .ToDictionary(g => g.Key ?? "미분류", g => g.Average(grp => -grp.Sum(t => t.Amount)));

            // overspending detection
            result.OverSpending = RuleEngine.DetectOverSpending(result.ByCategory, hist ?? new Dictionary<string, decimal>());

            // fixed expenses
            result.FixedExpenses = RuleEngine.DetectFixedExpenses(transactions, asOf);

            // spending profile (simple heuristic)
            var top = result.ByCategory.OrderByDescending(k => k.Value).Take(3).Select(k => k.Key).ToList();
            result.SpendingProfile = top.Count == 0 ? "지출 없음" : "주로 " + string.Join(", ", top) + " 항목에 지출이 많습니다.";

            // saving score (estimate income as sum of positive amounts this month)
            var incomeThisMonth = transactions.Where(t => t.Date >= monthStart && t.Date <= monthEnd && t.Amount > 0).Sum(t => t.Amount);
            result.SavingScore = RuleEngine.ComputeSavingScore(result.TotalSpend, incomeThisMonth, result.ByCategory);

            return result;
        }
    }

    public static class ReportGenerator
    {
        public static List<string> GenerateReport(AnalysisResult r)
        {
            var lines = new List<string>();
            lines.Add($"이번 달 총 지출은 {FormatWon(r.TotalSpend)}입니다.");

            if (r.CategoryRatio != null && r.CategoryRatio.Count > 0)
            {
                var top = r.CategoryRatio.OrderByDescending(k => k.Value).First();
                lines.Add($"{top.Key} 비중이 {Math.Round(top.Value)}%로 가장 높습니다.");
            }

            lines.Add($"전월 대비 소비가 {(r.ChangePercent >= 0 ? "증가" : "감소")}하여 {Math.Abs(r.ChangePercent)}% {(r.ChangePercent>=0?"증가":"감소")}했습니다.");

            if (r.OverSpending != null && r.OverSpending.Count > 0)
            {
                lines.Add($"과소비로 보이는 항목: {string.Join(", ", r.OverSpending)}.");
            }

            if (r.FixedExpenses != null && r.FixedExpenses.Count > 0)
            {
                lines.Add($"고정 지출로 추정되는 항목: {string.Join(", ", r.FixedExpenses.Take(5))}.");
            }

            lines.Add($"현재 소비 패턴: {r.SpendingProfile}");
            lines.Add($"절약 점수: {r.SavingScore}/100");

            // short recommendation
            if (r.SavingScore < 40)
            {
                lines.Add("권장: 지출을 줄이고 비상금 관리를 우선하세요.");
            }
            else if (r.SavingScore < 70)
            {
                lines.Add("권장: 지출 중 상위 항목을 재검토하고 예산을 조정하세요.");
            }
            else
            {
                lines.Add("현재 절약 상태는 양호합니다. 계속 유지하세요.");
            }

            return lines;
        }

        private static string FormatWon(decimal value)
        {
            long v = (long)Math.Round(value, 0);
            string abs = Math.Abs(v).ToString("N0");
            return (v < 0 ? "-" : "") + abs + "원";
        }
    }
}
