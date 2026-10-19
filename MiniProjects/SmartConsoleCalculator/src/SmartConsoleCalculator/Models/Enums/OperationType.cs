using System.Text.Json.Serialization;

namespace SmartConsoleCalculator.Models.Enums;

/// <summary>
/// Specifies the type of mathematical calculation or operation performed.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OperationType
{
    // Basic operations
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Modulus,

    // Scientific operations
    Power,
    Square,
    SquareRoot,
    AbsoluteValue,
    Factorial,
    Percentage,

    // Expression operations
    Expression
}

/// <summary>
/// Categorizes operations into broad functional groups.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OperationCategory
{
    Basic,
    Scientific,
    Expression
}

/// <summary>
/// Provides extension methods for the <see cref="OperationType"/> enum.
/// </summary>
public static class OperationTypeExtensions
{
    /// <summary>
    /// Maps an <see cref="OperationType"/> to its corresponding <see cref="OperationCategory"/>.
    /// </summary>
    /// <param name="operationType">The operation type to categorize.</param>
    /// <returns>The operation category for the given operation type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the operation type is not recognized.</exception>
    public static OperationCategory GetCategory(this OperationType operationType) => operationType switch
    {
        OperationType.Addition => OperationCategory.Basic,
        OperationType.Subtraction => OperationCategory.Basic,
        OperationType.Multiplication => OperationCategory.Basic,
        OperationType.Division => OperationCategory.Basic,
        OperationType.Modulus => OperationCategory.Basic,

        OperationType.Power => OperationCategory.Scientific,
        OperationType.Square => OperationCategory.Scientific,
        OperationType.SquareRoot => OperationCategory.Scientific,
        OperationType.AbsoluteValue => OperationCategory.Scientific,
        OperationType.Factorial => OperationCategory.Scientific,
        OperationType.Percentage => OperationCategory.Scientific,

        OperationType.Expression => OperationCategory.Expression,

        _ => throw new ArgumentOutOfRangeException(nameof(operationType), operationType, $"Unknown operation type: {operationType}")
    };
}
