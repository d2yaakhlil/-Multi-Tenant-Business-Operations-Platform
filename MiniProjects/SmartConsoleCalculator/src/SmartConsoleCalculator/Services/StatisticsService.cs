using SmartConsoleCalculator.Models;
using SmartConsoleCalculator.Models.Enums;

namespace SmartConsoleCalculator.Services;

public static class StatisticsService
{
    public static CalculationStatistics CalculateStatistics(List<CalculationRecord> records)
    {
        var stats = new CalculationStatistics
        {
            TotalCount = records.Count,
            BasicOperationCount = records.Count(r => r.OperationType.GetCategory() == OperationCategory.Basic),
            ScientificOperationCount = records.Count(r => r.OperationType.GetCategory() == OperationCategory.Scientific),
            ExpressionCount = records.Count(r => r.OperationType.GetCategory() == OperationCategory.Expression),
            OperationTypeCounts = records
                .GroupBy(r => r.OperationType)
                .ToDictionary(g => g.Key, g => g.Count())
        };

        if (records.Count > 0)
        {
            stats.AverageResult = records.Average(r => r.Result);
            stats.HighestResult = records.Max(r => r.Result);
            stats.LowestResult = records.Min(r => r.Result);
            stats.MostRecentCalculation = records.OrderByDescending(r => r.Timestamp).First();
        }

        return stats;
    }
}

