using SmartConsoleCalculator.Models;
using SmartConsoleCalculator.Models.Enums;
using SmartConsoleCalculator.Services;
using SmartConsoleCalculator.Utilities;

namespace SmartConsoleCalculator.UI;

public class ConsoleMenu
{
    private readonly CalculationHistoryService _historyService;
    private readonly CalculatorMemoryService _memoryService;
    private readonly SettingsService _settingsService;
    private readonly ExpressionEvaluator _expressionEvaluator;
    private readonly Action _saveHistory;
    private readonly Action _saveSettings;

    public ConsoleMenu(
        CalculationHistoryService historyService,
        CalculatorMemoryService memoryService,
        SettingsService settingsService,
        ExpressionEvaluator expressionEvaluator,
        Action saveHistory,
        Action saveSettings)
    {
        _historyService = historyService;
        _memoryService = memoryService;
        _settingsService = settingsService;
        _expressionEvaluator = expressionEvaluator;
        _saveHistory = saveHistory;
        _saveSettings = saveSettings;
    }

    private CalculatorSettings Settings => _settingsService.CurrentSettings;

    private string Format(double value) => NumberFormatter.FormatResult(value, Settings);

    // ───────── Main Menu ─────────

    public void ShowMainMenu()
    {
        Console.WriteLine();
        Console.WriteLine("================================================");
        Console.WriteLine("             SMART CONSOLE CALCULATOR");
        Console.WriteLine("================================================");
        Console.WriteLine();
        Console.WriteLine("  1. Basic Calculations");
        Console.WriteLine("  2. Scientific Operations");
        Console.WriteLine("  3. Expression Evaluator");
        Console.WriteLine("  4. Calculation History");
        Console.WriteLine("  5. Memory Functions");
        Console.WriteLine("  6. Statistics");
        Console.WriteLine("  7. Settings");
        Console.WriteLine();
        Console.WriteLine("  0. Exit");
        Console.WriteLine();
        Console.WriteLine("================================================");
        Console.Write("Enter your choice: ");
    }

    public void HandleMainMenuChoice(string? input)
    {
        var (success, choice) = InputValidator.TryParseMenuChoice(input);
        if (!success)
        {
            Console.WriteLine("Invalid selection. Please enter a number from 0 to 7.");
            return;
        }

        switch (choice)
        {
            case 1: RunBasicCalculations(); break;
            case 2: RunScientificOperations(); break;
            case 3: RunExpressionEvaluator(); break;
            case 4: RunHistoryMenu(); break;
            case 5: RunMemoryMenu(); break;
            case 6: ShowStatistics(); break;
            case 7: RunSettingsMenu(); break;
            case 0: break; // exit handled by caller
            default:
                Console.WriteLine("Invalid selection. Please enter a number from 0 to 7.");
                break;
        }
    }

    // ───────── Basic Calculations ─────────

    private void RunBasicCalculations()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("============= BASIC CALCULATIONS ==============");
            Console.WriteLine();
            Console.WriteLine("  1. Addition (+)");
            Console.WriteLine("  2. Subtraction (-)");
            Console.WriteLine("  3. Multiplication (*)");
            Console.WriteLine("  4. Division (/)");
            Console.WriteLine("  5. Modulus (%)");
            Console.WriteLine();
            Console.WriteLine("  0. Back");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();
            if (input == null) return;

            var (success, choice) = InputValidator.TryParseMenuChoice(input);

            if (!success || choice < 0 || choice > 5)
            {
                Console.WriteLine("Invalid selection.");
                continue;
            }

            if (choice == 0) return;

            Console.Write("Enter the first number: ");
            string? num1Input = Console.ReadLine();
            var (ok1, a) = InputValidator.TryParseNumber(num1Input);
            if (!ok1) { Console.WriteLine("Error: Invalid number."); continue; }

            Console.Write("Enter the second number: ");
            string? num2Input = Console.ReadLine();
            var (ok2, b) = InputValidator.TryParseNumber(num2Input);
            if (!ok2) { Console.WriteLine("Error: Invalid number."); continue; }

            var (opType, symbol) = choice switch
            {
                1 => (OperationType.Addition, "+"),
                2 => (OperationType.Subtraction, "-"),
                3 => (OperationType.Multiplication, "*"),
                4 => (OperationType.Division, "/"),
                5 => (OperationType.Modulus, "%"),
                _ => (OperationType.Addition, "+")
            };

            (bool Success, double Result, string Error) result = choice switch
            {
                1 => CalculatorService.Add(a, b),
                2 => CalculatorService.Subtract(a, b),
                3 => CalculatorService.Multiply(a, b),
                4 => CalculatorService.Divide(a, b),
                5 => CalculatorService.Modulus(a, b),
                _ => (false, 0.0, "Unknown operation.")
            };

            if (result.Success)
            {
                string expression = $"{Format(a)} {symbol} {Format(b)}";
                Console.WriteLine($"  {expression} = {Format(result.Result)}");
                RecordAndUpdateAns(expression, result.Result, opType, a, b);
            }
            else
            {
                Console.WriteLine($"  {result.Error}");
            }
        }
    }

    // ───────── Scientific Operations ─────────

    private void RunScientificOperations()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("============ SCIENTIFIC OPERATIONS =============");
            Console.WriteLine();
            Console.WriteLine("  1. Power (x^y)");
            Console.WriteLine("  2. Square (x²)");
            Console.WriteLine("  3. Square Root (√x)");
            Console.WriteLine("  4. Absolute Value (|x|)");
            Console.WriteLine("  5. Factorial (n!)");
            Console.WriteLine("  6. Percentage");
            Console.WriteLine();
            Console.WriteLine("  0. Back");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();
            if (input == null) return;

            var (success, choice) = InputValidator.TryParseMenuChoice(input);

            if (!success || choice < 0 || choice > 6)
            {
                Console.WriteLine("Invalid selection.");
                continue;
            }

            if (choice == 0) return;

            switch (choice)
            {
                case 1: HandlePower(); break;
                case 2: HandleSquare(); break;
                case 3: HandleSquareRoot(); break;
                case 4: HandleAbsoluteValue(); break;
                case 5: HandleFactorial(); break;
                case 6: HandlePercentage(); break;
            }
        }
    }

    private void HandlePower()
    {
        Console.Write("Enter the base: ");
        var (ok1, baseVal) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok1) { Console.WriteLine("Error: Invalid number."); return; }

        Console.Write("Enter the exponent: ");
        var (ok2, exp) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok2) { Console.WriteLine("Error: Invalid number."); return; }

        var result = ScientificCalculatorService.Power(baseVal, exp);
        if (result.Success)
        {
            string expression = $"{Format(baseVal)} ^ {Format(exp)}";
            Console.WriteLine($"  {expression} = {Format(result.Result)}");
            RecordAndUpdateAns(expression, result.Result, OperationType.Power, baseVal, exp);
        }
        else
        {
            Console.WriteLine($"  {result.Error}");
        }
    }

    private void HandleSquare()
    {
        Console.Write("Enter a number: ");
        var (ok, value) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok) { Console.WriteLine("Error: Invalid number."); return; }

        var result = ScientificCalculatorService.Square(value);
        if (result.Success)
        {
            string expression = $"{Format(value)}² = {Format(result.Result)}";
            Console.WriteLine($"  {expression}");
            RecordAndUpdateAns($"{Format(value)}²", result.Result, OperationType.Square, value);
        }
        else
        {
            Console.WriteLine($"  {result.Error}");
        }
    }

    private void HandleSquareRoot()
    {
        Console.Write("Enter a number: ");
        var (ok, value) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok) { Console.WriteLine("Error: Invalid number."); return; }

        var result = ScientificCalculatorService.SquareRoot(value);
        if (result.Success)
        {
            string expression = $"sqrt({Format(value)})";
            Console.WriteLine($"  {expression} = {Format(result.Result)}");
            RecordAndUpdateAns(expression, result.Result, OperationType.SquareRoot, value);
        }
        else
        {
            Console.WriteLine($"  {result.Error}");
        }
    }

    private void HandleAbsoluteValue()
    {
        Console.Write("Enter a number: ");
        var (ok, value) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok) { Console.WriteLine("Error: Invalid number."); return; }

        var result = ScientificCalculatorService.AbsoluteValue(value);
        if (result.Success)
        {
            string expression = $"abs({Format(value)})";
            Console.WriteLine($"  {expression} = {Format(result.Result)}");
            RecordAndUpdateAns(expression, result.Result, OperationType.AbsoluteValue, value);
        }
        else
        {
            Console.WriteLine($"  {result.Error}");
        }
    }

    private void HandleFactorial()
    {
        Console.Write("Enter a non-negative integer (max 170): ");
        var (ok, value) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok) { Console.WriteLine("Error: Invalid number."); return; }

        var result = ScientificCalculatorService.Factorial(value);
        if (result.Success)
        {
            string expression = $"{(int)value}!";
            Console.WriteLine($"  {expression} = {Format(result.Result)}");
            RecordAndUpdateAns(expression, result.Result, OperationType.Factorial, value);
        }
        else
        {
            Console.WriteLine($"  {result.Error}");
        }
    }

    private void HandlePercentage()
    {
        Console.Write("Enter the percentage: ");
        var (ok1, percentage) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok1) { Console.WriteLine("Error: Invalid number."); return; }

        Console.Write("Enter the base value: ");
        var (ok2, baseValue) = InputValidator.TryParseNumber(Console.ReadLine());
        if (!ok2) { Console.WriteLine("Error: Invalid number."); return; }

        var result = ScientificCalculatorService.Percentage(percentage, baseValue);
        if (result.Success)
        {
            string expression = $"{Format(percentage)}% of {Format(baseValue)}";
            Console.WriteLine($"  {expression} = {Format(result.Result)}");
            RecordAndUpdateAns(expression, result.Result, OperationType.Percentage, percentage, baseValue);
        }
        else
        {
            Console.WriteLine($"  {result.Error}");
        }
    }

    // ───────── Expression Evaluator ─────────

    private void RunExpressionEvaluator()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("============= EXPRESSION EVALUATOR =============");
            Console.WriteLine();
            Console.WriteLine("  Enter a mathematical expression, or type 'back' to return.");
            Console.WriteLine("  Supported: +, -, *, /, %, parentheses, decimals (.)");
            Console.WriteLine("  Example: (5 + 3) * 2");
            Console.WriteLine();
            Console.Write("Expression: ");

            string? input = Console.ReadLine();
            if (input == null) return;

            if (string.IsNullOrWhiteSpace(input)) { Console.WriteLine("Error: Expression is empty."); continue; }
            if (input.Trim().Equals("back", StringComparison.OrdinalIgnoreCase)) return;

            var result = _expressionEvaluator.Evaluate(input);
            if (result.Success)
            {
                Console.WriteLine($"  {input.Trim()} = {Format(result.Result)}");
                RecordAndUpdateAns(input.Trim(), result.Result, OperationType.Expression);
            }
            else
            {
                Console.WriteLine($"  {result.Error}");
            }
        }
    }

    // ───────── History Menu ─────────

    private void RunHistoryMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("============= CALCULATION HISTORY ==============");
            Console.WriteLine();
            Console.WriteLine("  1. View All Records");
            Console.WriteLine("  2. View Recent Records");
            Console.WriteLine("  3. View Record Details");
            Console.WriteLine("  4. Search History");
            Console.WriteLine("  5. Filter by Operation Type");
            Console.WriteLine("  6. Clear History");
            Console.WriteLine();
            Console.WriteLine("  0. Back");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();
            if (input == null) return;

            var (success, choice) = InputValidator.TryParseMenuChoice(input);

            if (!success || choice < 0 || choice > 6)
            {
                Console.WriteLine("Invalid selection.");
                continue;
            }

            if (choice == 0) return;

            switch (choice)
            {
                case 1: ViewAllRecords(); break;
                case 2: ViewRecentRecords(); break;
                case 3: ViewRecordDetails(); break;
                case 4: SearchHistory(); break;
                case 5: FilterByOperationType(); break;
                case 6: ClearHistory(); break;
            }
        }
    }

    private void ViewAllRecords()
    {
        var records = _historyService.GetAllRecords();
        if (records.Count == 0)
        {
            Console.WriteLine("  No calculation records found.");
            return;
        }

        Console.WriteLine($"\n  All Records ({records.Count} total):");
        Console.WriteLine("  " + new string('-', 50));
        foreach (var record in records)
            PrintRecordSummary(record);
    }

    private void ViewRecentRecords()
    {
        int count = Settings.HistoryDisplayCount;
        var records = _historyService.GetRecentRecords(count);
        if (records.Count == 0)
        {
            Console.WriteLine("  No calculation records found.");
            return;
        }

        Console.WriteLine($"\n  Recent Records (showing {records.Count}):");
        Console.WriteLine("  " + new string('-', 50));
        foreach (var record in records)
            PrintRecordSummary(record);
    }

    private void ViewRecordDetails()
    {
        Console.Write("Enter record ID: ");
        string? id = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("Error: ID is required.");
            return;
        }

        var record = _historyService.GetRecordById(id.Trim());
        if (record == null)
        {
            Console.WriteLine($"  No record found with ID: {id.Trim()}");
            return;
        }

        PrintRecordDetails(record);
    }

    private void SearchHistory()
    {
        Console.Write("Enter search term: ");
        string? term = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(term))
        {
            Console.WriteLine("Error: Search term is required.");
            return;
        }

        var records = _historyService.SearchRecords(term.Trim());
        if (records.Count == 0)
        {
            Console.WriteLine($"  No records matching '{term.Trim()}'.");
            return;
        }

        Console.WriteLine($"\n  Search Results ({records.Count} found):");
        Console.WriteLine("  " + new string('-', 50));
        foreach (var record in records)
            PrintRecordSummary(record);
    }

    private void FilterByOperationType()
    {
        Console.WriteLine("\n  Operation Categories:");
        Console.WriteLine("    1. Basic");
        Console.WriteLine("    2. Scientific");
        Console.WriteLine("    3. Expression");
        Console.Write("  Select category: ");

        var (success, choice) = InputValidator.TryParseMenuChoice(Console.ReadLine());
        if (!success || choice < 1 || choice > 3)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        var category = choice switch
        {
            1 => OperationCategory.Basic,
            2 => OperationCategory.Scientific,
            3 => OperationCategory.Expression,
            _ => OperationCategory.Basic
        };

        var records = _historyService.FilterByCategory(category);
        if (records.Count == 0)
        {
            Console.WriteLine($"  No {category} records found.");
            return;
        }

        Console.WriteLine($"\n  {category} Records ({records.Count} found):");
        Console.WriteLine("  " + new string('-', 50));
        foreach (var record in records)
            PrintRecordSummary(record);
    }

    private void ClearHistory()
    {
        if (_historyService.GetRecordCount() == 0)
        {
            Console.WriteLine("  History is already empty.");
            return;
        }

        if (Settings.ConfirmBeforeClear)
        {
            Console.Write("  Are you sure you want to clear all history? (yes/no): ");
            string? confirm = Console.ReadLine();
            if (!string.Equals(confirm?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("  Clear cancelled.");
                return;
            }
        }
        else
        {
            // Even with confirmation disabled, display a warning
            Console.WriteLine("  WARNING: This will permanently delete all history records.");
            Console.Write("  Type 'yes' to confirm: ");
            string? confirm = Console.ReadLine();
            if (!string.Equals(confirm?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("  Clear cancelled.");
                return;
            }
        }

        _historyService.ClearHistory();
        _saveHistory();
        Console.WriteLine("  History cleared successfully.");
    }

    private void PrintRecordSummary(CalculationRecord record)
    {
        string time = record.Timestamp.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"  [{record.Id[..8]}] {time}  {record.Expression} = {Format(record.Result)}");
    }

    private void PrintRecordDetails(CalculationRecord record)
    {
        Console.WriteLine();
        Console.WriteLine("  Record Details:");
        Console.WriteLine($"  ID:         {record.Id}");
        Console.WriteLine($"  Type:       {record.OperationType}");
        Console.WriteLine($"  Category:   {record.OperationType.GetCategory()}");
        Console.WriteLine($"  Expression: {record.Expression}");
        Console.WriteLine($"  Result:     {Format(record.Result)}");
        Console.WriteLine($"  Time:       {record.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss}");
        if (record.Operand1.HasValue)
            Console.WriteLine($"  Operand 1:  {Format(record.Operand1.Value)}");
        if (record.Operand2.HasValue)
            Console.WriteLine($"  Operand 2:  {Format(record.Operand2.Value)}");
    }

    // ───────── Memory Menu ─────────

    private void RunMemoryMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("============== MEMORY FUNCTIONS ================");
            Console.WriteLine();

            string ansDisplay = _memoryService.Ans.HasValue ? Format(_memoryService.Ans.Value) : "(none)";
            var (hasMemory, memValue) = _memoryService.MemoryRecall();
            string memDisplay = hasMemory ? Format(memValue) : "(empty)";

            Console.WriteLine($"  Ans: {ansDisplay}");
            Console.WriteLine($"  Memory: {memDisplay}");
            Console.WriteLine();
            Console.WriteLine("  1. M+ (Add to memory)");
            Console.WriteLine("  2. M- (Subtract from memory)");
            Console.WriteLine("  3. MR (Recall memory)");
            Console.WriteLine("  4. MC (Clear memory)");
            Console.WriteLine();
            Console.WriteLine("  0. Back");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();
            if (input == null) return;

            var (success, choice) = InputValidator.TryParseMenuChoice(input);

            if (!success || choice < 0 || choice > 4)
            {
                Console.WriteLine("Invalid selection.");
                continue;
            }

            if (choice == 0) return;

            switch (choice)
            {
                case 1:
                    Console.Write("  Enter value to add to memory: ");
                    var (ok1, val1) = InputValidator.TryParseNumber(Console.ReadLine());
                    if (!ok1) { Console.WriteLine("  Error: Invalid number."); break; }
                    var addResult = _memoryService.MemoryAdd(val1);
                    if (addResult.Success)
                        Console.WriteLine($"  Memory updated: {Format(addResult.NewValue)}");
                    else
                        Console.WriteLine($"  {addResult.Error}");
                    break;

                case 2:
                    Console.Write("  Enter value to subtract from memory: ");
                    var (ok2, val2) = InputValidator.TryParseNumber(Console.ReadLine());
                    if (!ok2) { Console.WriteLine("  Error: Invalid number."); break; }
                    var subResult = _memoryService.MemorySubtract(val2);
                    if (subResult.Success)
                        Console.WriteLine($"  Memory updated: {Format(subResult.NewValue)}");
                    else
                        Console.WriteLine($"  {subResult.Error}");
                    break;

                case 3:
                    var (hasMem, value) = _memoryService.MemoryRecall();
                    if (hasMem)
                        Console.WriteLine($"  Memory value: {Format(value)}");
                    else
                        Console.WriteLine("  Memory has not been set.");
                    break;

                case 4:
                    _memoryService.MemoryClear();
                    Console.WriteLine("  Memory cleared.");
                    break;
            }
        }
    }

    // ───────── Statistics ─────────

    private void ShowStatistics()
    {
        var records = _historyService.GetAllRecordsRaw();
        var stats = StatisticsService.CalculateStatistics(records);

        Console.WriteLine();
        Console.WriteLine("================= STATISTICS ===================");
        Console.WriteLine();

        if (stats.TotalCount == 0)
        {
            Console.WriteLine("  No calculations recorded yet.");
            return;
        }

        Console.WriteLine($"  Total Calculations:    {stats.TotalCount}");
        Console.WriteLine($"  Basic Operations:      {stats.BasicOperationCount}");
        Console.WriteLine($"  Scientific Operations: {stats.ScientificOperationCount}");
        Console.WriteLine($"  Expressions Evaluated: {stats.ExpressionCount}");
        Console.WriteLine();

        Console.WriteLine("  Breakdown by Type:");
        foreach (var kvp in stats.OperationTypeCounts.OrderByDescending(kv => kv.Value))
            Console.WriteLine($"    {kvp.Key}: {kvp.Value}");
        Console.WriteLine();

        if (stats.AverageResult.HasValue)
            Console.WriteLine($"  Average Result: {Format(stats.AverageResult.Value)}");
        if (stats.HighestResult.HasValue)
            Console.WriteLine($"  Highest Result: {Format(stats.HighestResult.Value)}");
        if (stats.LowestResult.HasValue)
            Console.WriteLine($"  Lowest Result:  {Format(stats.LowestResult.Value)}");

        if (stats.MostRecentCalculation != null)
        {
            Console.WriteLine();
            Console.WriteLine("  Most Recent Calculation:");
            Console.WriteLine($"    {stats.MostRecentCalculation.Expression} = {Format(stats.MostRecentCalculation.Result)}");
            Console.WriteLine($"    at {stats.MostRecentCalculation.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss}");
        }

        Console.WriteLine();
        Console.WriteLine("  Note: The average is the arithmetic mean of all recorded");
        Console.WriteLine("  result values. Different operation types may produce values");
        Console.WriteLine("  that are not directly comparable.");
    }

    // ───────── Settings Menu ─────────

    private void RunSettingsMenu()
    {
        while (true)
        {
            var settings = Settings;
            Console.WriteLine();
            Console.WriteLine("================== SETTINGS ====================");
            Console.WriteLine();
            Console.WriteLine($"  1. Decimal Precision:        {settings.DecimalPrecision}");
            Console.WriteLine($"  2. History Display Count:    {settings.HistoryDisplayCount}");
            Console.WriteLine($"  3. Confirm Before Clear:     {(settings.ConfirmBeforeClear ? "On" : "Off")}");
            Console.WriteLine($"  4. Thousands Separator:      {(settings.UseThousandsSeparator ? "On" : "Off")}");
            Console.WriteLine($"  5. Reset to Defaults");
            Console.WriteLine();
            Console.WriteLine("  0. Back");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();
            if (input == null) return;

            var (success, choice) = InputValidator.TryParseMenuChoice(input);

            if (!success || choice < 0 || choice > 5)
            {
                Console.WriteLine("Invalid selection.");
                continue;
            }

            if (choice == 0) return;

            switch (choice)
            {
                case 1:
                    Console.Write("  Enter decimal precision (0-15): ");
                    var (okP, precision) = InputValidator.TryParseInteger(Console.ReadLine());
                    if (!okP) { Console.WriteLine("  Error: Invalid number."); break; }
                    var pResult = _settingsService.UpdateDecimalPrecision(precision);
                    if (pResult.Success)
                    {
                        _saveSettings();
                        Console.WriteLine($"  Decimal precision set to {precision}.");
                    }
                    else
                        Console.WriteLine($"  {pResult.Error}");
                    break;

                case 2:
                    Console.Write("  Enter history display count (1-100): ");
                    var (okH, count) = InputValidator.TryParseInteger(Console.ReadLine());
                    if (!okH) { Console.WriteLine("  Error: Invalid number."); break; }
                    var hResult = _settingsService.UpdateHistoryDisplayCount(count);
                    if (hResult.Success)
                    {
                        _saveSettings();
                        Console.WriteLine($"  History display count set to {count}.");
                    }
                    else
                        Console.WriteLine($"  {hResult.Error}");
                    break;

                case 3:
                    var currentConfirm = Settings.ConfirmBeforeClear;
                    _settingsService.UpdateConfirmBeforeClear(!currentConfirm);
                    _saveSettings();
                    Console.WriteLine($"  Confirm before clear: {(!currentConfirm ? "On" : "Off")}");
                    break;

                case 4:
                    var currentSep = Settings.UseThousandsSeparator;
                    _settingsService.UpdateThousandsSeparator(!currentSep);
                    _saveSettings();
                    Console.WriteLine($"  Thousands separator: {(!currentSep ? "On" : "Off")}");
                    break;

                case 5:
                    Console.Write("  Are you sure you want to reset all settings? (yes/no): ");
                    string? confirm = Console.ReadLine();
                    if (string.Equals(confirm?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
                    {
                        _settingsService.ResetToDefaults();
                        _saveSettings();
                        Console.WriteLine("  Settings reset to defaults.");
                    }
                    else
                    {
                        Console.WriteLine("  Reset cancelled.");
                    }
                    break;
            }
        }
    }

    // ───────── Shared Helpers ─────────

    private void RecordAndUpdateAns(string expression, double result, OperationType opType,
        double? operand1 = null, double? operand2 = null)
    {
        var record = new CalculationRecord(expression, result, opType, operand1, operand2);
        _historyService.AddRecord(record);
        _memoryService.UpdateAns(result);
        _saveHistory();
    }
}

