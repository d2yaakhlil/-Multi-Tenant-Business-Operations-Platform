using System.Globalization;

namespace SmartConsoleCalculator.Utilities;

public static class InputValidator
{
    public static (bool Success, double Value) TryParseNumber(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, 0);

        string clean = input.Trim().Trim('\uFEFF');

        if (double.TryParse(clean, NumberStyles.Float | NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture, out double value))
        {
            if (!double.IsFinite(value))
                return (false, 0);

            return (true, value);
        }

        return (false, 0);
    }

    public static (bool Success, int Value) TryParseInteger(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, 0);

        string clean = input.Trim().Trim('\uFEFF');

        if (int.TryParse(clean, out int value))
            return (true, value);

        return (false, 0);
    }

    public static bool IsNullOrWhiteSpace(string? input) => string.IsNullOrWhiteSpace(input);

    public static (bool Success, int Value) TryParseMenuChoice(string? input)
    {
        return TryParseInteger(input);
    }
}

