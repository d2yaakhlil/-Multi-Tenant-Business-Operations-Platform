namespace SmartConsoleCalculator.Services;

/// <summary>
/// Provides scientific mathematical operations with domain validation and error handling.
/// </summary>
public static class ScientificCalculatorService
{
    public const int MaxFactorialInput = 170;
    private const string OverflowErrorMessage = "Calculation resulted in an overflow or non-finite value.";
    private const string NonFiniteOperandMessage = "Operands must be finite numbers.";

    /// <summary>
    /// Calculates the base raised to the specified exponent using <see cref="Math.Pow"/>.
    /// </summary>
    /// <param name="base_">The base value.</param>
    /// <param name="exponent">The exponent value.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Power(double base_, double exponent)
    {
        if (!double.IsFinite(base_) || !double.IsFinite(exponent))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        // Negative base with a non-integer exponent yields a complex number outside the real domain
        if (base_ < 0.0 && exponent % 1 != 0.0)
        {
            return (false, 0.0, "Negative base with a fractional exponent results in a non-real number.");
        }

        // 0 raised to a negative power is division by zero (e.g. 1 / 0^n)
        if (base_ == 0.0 && exponent < 0.0)
        {
            return (false, 0.0, "Cannot raise zero to a negative power (division by zero).");
        }

        double result = Math.Pow(base_, exponent);
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Calculates the square of a number (value * value).
    /// </summary>
    /// <param name="value">The number to square.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Square(double value)
    {
        if (!double.IsFinite(value))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        double result = value * value;
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Calculates the square root of a non-negative number using <see cref="Math.Sqrt"/>.
    /// Rejects negative inputs as operations are restricted to the real-number domain.
    /// </summary>
    /// <param name="value">The number to find the square root of.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) SquareRoot(double value)
    {
        if (!double.IsFinite(value))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        if (value < 0.0)
        {
            return (false, 0.0, "Cannot calculate the square root of a negative number.");
        }

        double result = Math.Sqrt(value);
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Calculates the absolute value of a number using <see cref="Math.Abs"/>.
    /// </summary>
    /// <param name="value">The number to evaluate.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) AbsoluteValue(double value)
    {
        if (!double.IsFinite(value))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        double result = Math.Abs(value);
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Computes the factorial of a non-negative integer represented as a double.
    /// Rejects non-finite values, negative values, non-integers, and values greater than 170.
    /// </summary>
    /// <param name="value">The non-negative integer value.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Factorial(double value)
    {
        if (!double.IsFinite(value))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        if (value < 0.0)
        {
            return (false, 0.0, "Factorial is not defined for negative integers.");
        }

        if (Math.Floor(value) != value)
        {
            return (false, 0.0, "Factorial is only defined for non-negative integers.");
        }

        if (value > MaxFactorialInput)
        {
            return (false, 0.0, $"Factorial input ({value}) exceeds the maximum supported limit of {MaxFactorialInput} (overflow).");
        }

        return Factorial((int)value);
    }

    /// <summary>
    /// Computes the factorial of a non-negative integer n (n!) iteratively using a double accumulator.
    /// Rejects negative values and values greater than 170 to prevent double precision overflow.
    /// </summary>
    /// <param name="n">The non-negative integer.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Factorial(int n)
    {
        if (n < 0)
        {
            return (false, 0.0, "Factorial is not defined for negative integers.");
        }

        if (n > MaxFactorialInput)
        {
            return (false, 0.0, $"Factorial input ({n}) exceeds the maximum supported limit of {MaxFactorialInput} (overflow).");
        }

        double result = 1.0;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
            if (!double.IsFinite(result))
            {
                return (false, 0.0, OverflowErrorMessage);
            }
        }

        return (true, result, string.Empty);
    }

    /// <summary>
    /// Calculates a percentage of a given base value (baseValue * percentage / 100).
    /// </summary>
    /// <param name="percentage">The percentage to calculate.</param>
    /// <param name="baseValue">The base value.</param>
    /// <returns>A tuple containing success status, result, and any error message.</returns>
    public static (bool Success, double Result, string Error) Percentage(double percentage, double baseValue)
    {
        if (!double.IsFinite(percentage) || !double.IsFinite(baseValue))
        {
            return (false, 0.0, NonFiniteOperandMessage);
        }

        double result = baseValue * percentage / 100.0;
        if (!double.IsFinite(result))
        {
            return (false, 0.0, OverflowErrorMessage);
        }

        return (true, result, string.Empty);
    }
}
