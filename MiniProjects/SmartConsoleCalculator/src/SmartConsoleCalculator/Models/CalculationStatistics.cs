using System.Text.Json.Serialization;
using SmartConsoleCalculator.Models.Enums;

namespace SmartConsoleCalculator.Models;

/// <summary>
/// Encapsulates summary metrics and aggregates computed from calculation history.
/// </summary>
public class CalculationStatistics
{
    /// <summary>
    /// Total number of successful calculation records.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Count of basic arithmetic calculations.
    /// </summary>
    [JsonPropertyName("basicOperationCount")]
    public int BasicOperationCount { get; set; }

    /// <summary>
    /// Count of scientific calculations.
    /// </summary>
    [JsonPropertyName("scientificOperationCount")]
    public int ScientificOperationCount { get; set; }

    /// <summary>
    /// Count of evaluated expressions.
    /// </summary>
    [JsonPropertyName("expressionCount")]
    public int ExpressionCount { get; set; }

    /// <summary>
    /// Operation counts grouped by specific operation type.
    /// </summary>
    [JsonPropertyName("operationTypeCounts")]
    public Dictionary<OperationType, int> OperationTypeCounts { get; set; } = new();

    /// <summary>
    /// Arithmetic mean of all calculation results, or null if no calculations exist.
    /// </summary>
    [JsonPropertyName("averageResult")]
    public double? AverageResult { get; set; }

    /// <summary>
    /// Maximum numeric result across all calculations, or null if no calculations exist.
    /// </summary>
    [JsonPropertyName("highestResult")]
    public double? HighestResult { get; set; }

    /// <summary>
    /// Minimum numeric result across all calculations, or null if no calculations exist.
    /// </summary>
    [JsonPropertyName("lowestResult")]
    public double? LowestResult { get; set; }

    /// <summary>
    /// The most recent calculation record, or null if history is empty.
    /// </summary>
    [JsonPropertyName("mostRecentCalculation")]
    public CalculationRecord? MostRecentCalculation { get; set; }
}
