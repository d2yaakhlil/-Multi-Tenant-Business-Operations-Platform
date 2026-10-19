using System.Globalization;
using SmartConsoleCalculator.Models;

namespace SmartConsoleCalculator.Utilities;

public static class NumberFormatter
{
    public static string FormatNumber(double value, int decimalPrecision, bool useThousandsSeparator)
    {
        if (!double.IsFinite(value))
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        decimalPrecision = Math.Clamp(decimalPrecision, 0, 15);

        if (decimalPrecision == 0)
        {
            return useThousandsSeparator
                ? value.ToString("N0", CultureInfo.InvariantCulture)
                : value.ToString("F0", CultureInfo.InvariantCulture);
        }

        string format = useThousandsSeparator ? $"N{decimalPrecision}" : $"F{decimalPrecision}";
        string formatted = value.ToString(format, CultureInfo.InvariantCulture);

        if (formatted.Contains('.'))
        {
            int dotIndex = formatted.IndexOf('.');
            string integerPart = formatted[..dotIndex];
            string fractionalPart = formatted[(dotIndex + 1)..].TrimEnd('0');

            if (fractionalPart.Length > 0)
            {
                return $"{integerPart}.{fractionalPart}";
            }

            return integerPart;
        }

        return formatted;
    }

    public static string FormatResult(double value, CalculatorSettings settings)
    {
        return FormatNumber(value, settings.DecimalPrecision, settings.UseThousandsSeparator);
    }
}

