namespace SmartConsoleCalculator.Services;

/// <summary>
/// Provides basic arithmetic operations with comprehensive input validation and error handling.
/// </summary>
public static class CalculatorService
{
    private const string OverflowErrorMessage = "Calculation resulted in an overflow or non-finite value.";
    private const string NonFiniteOperandMessage = "Operands must be finite numbers.";
    private const string DivisionByZeroMessage = "Cannot divide by zero.";
    private const string ModulusByZeroMessage = "Cannot perform modulus with a zero divisor.";

    /// <summary>
    /// Adds two numbers.
    /// </summary>
    /// <param name="a">The first operand.</param>
    /// <param name="b">The second operand.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Add(double a, double b)
    {
        if (!double.IsFinite(a) || !double.IsFinite(b))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        double result = a + b;
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Subtracts the second number from the first.
    /// </summary>
    /// <param name="a">The first operand (minuend).</param>
    /// <param name="b">The second operand (subtrahend).</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Subtract(double a, double b)
    {
        if (!double.IsFinite(a) || !double.IsFinite(b))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        double result = a - b;
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Multiplies two numbers.
    /// </summary>
    /// <param name="a">The first operand.</param>
    /// <param name="b">The second operand.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Multiply(double a, double b)
    {
        if (!double.IsFinite(a) || !double.IsFinite(b))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        double result = a * b;
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Divides the first number by the second.
    /// </summary>
    /// <param name="a">The dividend.</param>
    /// <param name="b">The divisor.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Divide(double a, double b)
    {
        if (!double.IsFinite(a) || !double.IsFinite(b))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        if (b == 0.0)
        {
            return (false, 0.0, DivisionByZeroMessage);
        }

        double result = a / b;
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Calculates the remainder of dividing the first number by the second.
    /// </summary>
    /// <param name="a">The dividend.</param>
    /// <param name="b">The divisor.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Modulus(double a, double b)
    {
        if (!double.IsFinite(a) || !double.IsFinite(b))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        if (b == 0.0)
        {
            return (false, 0.0, ModulusByZeroMessage);
        }

        double result = a % b;
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }
}
