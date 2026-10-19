using System.Text.Json.Serialization;
using SmartConsoleCalculator.Models.Enums;

namespace SmartConsoleCalculator.Models;

/// <summary>
/// Represents an immutable historical record of a single completed calculation.
/// </summary>
public class CalculationRecord
{
    /// <summary>
    /// Unique identifier for this calculation record.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The specific operation type that was performed.
    /// </summary>
    [JsonPropertyName("operationType")]
    public OperationType OperationType { get; set; }

    /// <summary>
    /// Human-readable expression representing the calculation (e.g., "10 + 5", "sqrt(81)").
    /// </summary>
    [JsonPropertyName("expression")]
    public string Expression { get; set; }

    /// <summary>
    /// The numeric result of the calculation.
    /// </summary>
    [JsonPropertyName("result")]
    public double Result { get; set; }

    /// <summary>
    /// UTC timestamp of when the calculation was recorded.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The first operand involved in the calculation, if applicable.
    /// </summary>
    [JsonPropertyName("operand1")]
    public double? Operand1 { get; set; }

    /// <summary>
    /// The second operand involved in the calculation, if applicable.
    /// </summary>
    [JsonPropertyName("operand2")]
    public double? Operand2 { get; set; }

    /// <summary>
    /// Convenience property retrieving the broad category of the operation.
    /// </summary>
    [JsonIgnore]
    public OperationCategory Category => OperationType.GetCategory();

    /// <summary>
    /// Parameterless constructor required for JSON deserialization.
    /// Automatically initializes Id with a new GUID and Timestamp with UTC now.
    /// </summary>
    [JsonConstructor]
    public CalculationRecord()
    {
        Id = Guid.NewGuid().ToString();
        Expression = string.Empty;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Primary constructor for recording a completed calculation.
    /// Automatically generates a new GUID for Id and assigns the current UTC timestamp.
    /// </summary>
    /// <param name="expression">Human-readable calculation expression.</param>
    /// <param name="result">Computed result.</param>
    /// <param name="operationType">The operation type performed.</param>
    /// <param name="operand1">Optional first operand.</param>
    /// <param name="operand2">Optional second operand.</param>
    public CalculationRecord(
        string expression,
        double result,
        OperationType operationType,
        double? operand1 = null,
        double? operand2 = null)
    {
        Id = Guid.NewGuid().ToString();
        Expression = expression ?? string.Empty;
        Result = result;
        OperationType = operationType;
        Timestamp = DateTime.UtcNow;
        Operand1 = operand1;
        Operand2 = operand2;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Expression} = {Result}";
}
