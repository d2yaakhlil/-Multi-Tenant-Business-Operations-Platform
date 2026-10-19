using System.Text.Json.Serialization;

namespace SmartConsoleCalculator.Models;

/// <summary>
/// Encapsulates user-configurable settings and display preferences for the calculator.
/// </summary>
public class CalculatorSettings
{
    public const int MinDecimalPrecision = 0;
    public const int MaxDecimalPrecision = 15;
    public const int DefaultDecimalPrecision = 4;

    public const int MinHistoryDisplayCount = 1;
    public const int MaxHistoryDisplayCount = 100;
    public const int DefaultHistoryDisplayCount = 10;

    /// <summary>
    /// Gets or sets the number of decimal places used when formatting calculation results (0 to 15).
    /// </summary>
    [JsonPropertyName("decimalPrecision")]
    public int DecimalPrecision { get; set; } = DefaultDecimalPrecision;

    /// <summary>
    /// Gets or sets the default number of recent history records to display (1 to 100).
    /// </summary>
    [JsonPropertyName("historyDisplayCount")]
    public int HistoryDisplayCount { get; set; } = DefaultHistoryDisplayCount;

    /// <summary>
    /// Gets or sets a value indicating whether confirmation is required before clearing calculation history.
    /// </summary>
    [JsonPropertyName("confirmBeforeClear")]
    public bool ConfirmBeforeClear { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether thousands group separators are displayed in numeric results.
    /// </summary>
    [JsonPropertyName("useThousandsSeparator")]
    public bool UseThousandsSeparator { get; set; } = false;

    /// <summary>
    /// Creates a new <see cref="CalculatorSettings"/> instance populated with default configuration values.
    /// </summary>
    /// <returns>A default <see cref="CalculatorSettings"/> instance.</returns>
    public static CalculatorSettings CreateDefault() => new()
    {
        DecimalPrecision = DefaultDecimalPrecision,
        HistoryDisplayCount = DefaultHistoryDisplayCount,
        ConfirmBeforeClear = true,
        UseThousandsSeparator = false
    };

    /// <summary>
    /// Validates that all settings properties fall within their allowable numeric ranges.
    /// </summary>
    /// <returns><c>true</c> if all settings are valid; otherwise, <c>false</c>.</returns>
    public bool Validate()
    {
        if (DecimalPrecision < MinDecimalPrecision || DecimalPrecision > MaxDecimalPrecision)
        {
            return false;
        }

        if (HistoryDisplayCount < MinHistoryDisplayCount || HistoryDisplayCount > MaxHistoryDisplayCount)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Creates a deep copy of the current settings instance.
    /// </summary>
    /// <returns>A new <see cref="CalculatorSettings"/> instance with identical property values.</returns>
    public CalculatorSettings Clone() => new()
    {
        DecimalPrecision = DecimalPrecision,
        HistoryDisplayCount = HistoryDisplayCount,
        ConfirmBeforeClear = ConfirmBeforeClear,
        UseThousandsSeparator = UseThousandsSeparator
    };
}
