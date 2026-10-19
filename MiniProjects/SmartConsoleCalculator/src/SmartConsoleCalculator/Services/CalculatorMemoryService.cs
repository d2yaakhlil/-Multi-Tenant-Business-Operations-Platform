namespace SmartConsoleCalculator.Services;

public class CalculatorMemoryService
{
    public double? Ans { get; private set; }

    public double MemoryValue { get; private set; }

    public bool HasMemory { get; private set; }

    public void UpdateAns(double value)
    {
        Ans = value;
    }

    public (bool Success, double NewValue, string Error) MemoryAdd(double value)
    {
        if (!double.IsFinite(value))
            return (false, MemoryValue, "Error: Input value must be a finite number.");

        double newValue = MemoryValue + value;
        if (!double.IsFinite(newValue))
            return (false, MemoryValue, "Error: Memory operation result is not a finite number.");

        MemoryValue = newValue;
        HasMemory = true;
        return (true, MemoryValue, string.Empty);
    }

    public (bool Success, double NewValue, string Error) MemorySubtract(double value)
    {
        if (!double.IsFinite(value))
            return (false, MemoryValue, "Error: Input value must be a finite number.");

        double newValue = MemoryValue - value;
        if (!double.IsFinite(newValue))
            return (false, MemoryValue, "Error: Memory operation result is not a finite number.");

        MemoryValue = newValue;
        HasMemory = true;
        return (true, MemoryValue, string.Empty);
    }

    public (bool HasValue, double Value) MemoryRecall()
    {
        return (HasMemory, MemoryValue);
    }

    public void MemoryClear()
    {
        MemoryValue = 0;
        HasMemory = false;
    }
}

