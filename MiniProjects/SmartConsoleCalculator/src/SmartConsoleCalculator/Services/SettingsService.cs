using SmartConsoleCalculator.Models;

namespace SmartConsoleCalculator.Services;

public class SettingsService
{
    private CalculatorSettings _settings;

    public SettingsService(CalculatorSettings? initialSettings = null)
    {
        _settings = initialSettings ?? CalculatorSettings.CreateDefault();

        if (!_settings.Validate())
            _settings = CalculatorSettings.CreateDefault();
    }

    public CalculatorSettings CurrentSettings => _settings.Clone();

    public (bool Success, string Error) UpdateDecimalPrecision(int precision)
    {
        if (precision < 0 || precision > 15)
            return (false, "Error: Decimal precision must be between 0 and 15.");

        _settings.DecimalPrecision = precision;
        return (true, string.Empty);
    }

    public (bool Success, string Error) UpdateHistoryDisplayCount(int count)
    {
        if (count < 1 || count > 100)
            return (false, "Error: History display count must be between 1 and 100.");

        _settings.HistoryDisplayCount = count;
        return (true, string.Empty);
    }

    public void UpdateConfirmBeforeClear(bool value)
    {
        _settings.ConfirmBeforeClear = value;
    }

    public void UpdateThousandsSeparator(bool value)
    {
        _settings.UseThousandsSeparator = value;
    }

    public void ResetToDefaults()
    {
        _settings = CalculatorSettings.CreateDefault();
    }

    /// <summary>Returns the actual settings object for serialization.</summary>
    public CalculatorSettings GetSettings() => _settings;
}

