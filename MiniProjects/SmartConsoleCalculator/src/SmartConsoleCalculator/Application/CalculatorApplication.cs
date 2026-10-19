using SmartConsoleCalculator.Models;
using SmartConsoleCalculator.Services;
using SmartConsoleCalculator.Storage;
using SmartConsoleCalculator.UI;

namespace SmartConsoleCalculator.Application;

public class CalculatorApplication
{
    private readonly JsonFileStorage _storage;
    private readonly CalculationHistoryService _historyService;
    private readonly CalculatorMemoryService _memoryService;
    private readonly SettingsService _settingsService;
    private readonly ExpressionEvaluator _expressionEvaluator;
    private readonly ConsoleMenu _consoleMenu;

    public CalculatorApplication()
    {
        _storage = new JsonFileStorage();
        _expressionEvaluator = new ExpressionEvaluator();
        _memoryService = new CalculatorMemoryService();

        // Load settings
        _settingsService = new SettingsService(LoadSettings());

        // Load history
        _historyService = new CalculationHistoryService(LoadHistory());

        // Create the console menu with save delegates
        _consoleMenu = new ConsoleMenu(
            _historyService,
            _memoryService,
            _settingsService,
            _expressionEvaluator,
            SaveHistory,
            SaveSettings);
    }

    public void Run()
    {
        while (true)
        {
            _consoleMenu.ShowMainMenu();
            string? input = Console.ReadLine();

            if (input == null || IsExitChoice(input))
            {
                Console.WriteLine("\nThank you for using Smart Console Calculator. Goodbye!");
                break;
            }

            _consoleMenu.HandleMainMenuChoice(input);
        }
    }

    private static bool IsExitChoice(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        return input.Trim() == "0";
    }

    private List<CalculationRecord> LoadHistory()
    {
        try
        {
            ApplicationPaths.EnsureDataDirectoryExists();
            string path = ApplicationPaths.GetHistoryFilePath();
            var result = _storage.Load<List<CalculationRecord>>(path);

            if (result.Success && result.Data != null)
            {
                Console.WriteLine($"  Loaded {result.Data.Count} history records.");
                return result.Data;
            }

            if (!result.Success && result.Error != "File not found.")
            {
                Console.WriteLine($"  Warning: {result.Error}");
                Console.WriteLine("  Starting with empty history.");
            }

            return new List<CalculationRecord>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Warning: Could not load history: {ex.Message}");
            return new List<CalculationRecord>();
        }
    }

    private CalculatorSettings LoadSettings()
    {
        try
        {
            ApplicationPaths.EnsureDataDirectoryExists();
            string path = ApplicationPaths.GetSettingsFilePath();
            var result = _storage.Load<CalculatorSettings>(path);

            if (result.Success && result.Data != null && result.Data.Validate())
            {
                return result.Data;
            }

            if (!result.Success && result.Error != "File not found.")
            {
                Console.WriteLine($"  Warning: {result.Error}");
                Console.WriteLine("  Using default settings.");
            }

            return CalculatorSettings.CreateDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Warning: Could not load settings: {ex.Message}");
            return CalculatorSettings.CreateDefault();
        }
    }

    private void SaveHistory()
    {
        try
        {
            string path = ApplicationPaths.GetHistoryFilePath();
            var result = _storage.Save(path, _historyService.GetAllRecordsRaw());
            if (!result.Success)
            {
                Console.WriteLine($"  Warning: Could not save history: {result.Error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Warning: Could not save history: {ex.Message}");
        }
    }

    private void SaveSettings()
    {
        try
        {
            string path = ApplicationPaths.GetSettingsFilePath();
            var result = _storage.Save(path, _settingsService.GetSettings());
            if (!result.Success)
            {
                Console.WriteLine($"  Warning: Could not save settings: {result.Error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Warning: Could not save settings: {ex.Message}");
        }
    }
}

