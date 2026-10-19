using SmartConsoleCalculator.Models;
using SmartConsoleCalculator.Models.Enums;

namespace SmartConsoleCalculator.Services;

public class CalculationHistoryService
{
    private readonly List<CalculationRecord> _records;

    public CalculationHistoryService(List<CalculationRecord>? initialRecords = null)
    {
        _records = initialRecords ?? new List<CalculationRecord>();
    }

    public void AddRecord(CalculationRecord record)
    {
        _records.Add(record);
    }

    public List<CalculationRecord> GetAllRecords()
    {
        return _records.OrderByDescending(r => r.Timestamp).ToList();
    }

    public List<CalculationRecord> GetRecentRecords(int count)
    {
        return _records.OrderByDescending(r => r.Timestamp).Take(count).ToList();
    }

    public CalculationRecord? GetRecordById(string id)
    {
        return _records.FirstOrDefault(r => r.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public List<CalculationRecord> SearchRecords(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<CalculationRecord>();

        return _records
            .Where(r => r.Expression.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.Timestamp)
            .ToList();
    }

    public List<CalculationRecord> FilterByCategory(OperationCategory category)
    {
        return _records
            .Where(r => r.OperationType.GetCategory() == category)
            .OrderByDescending(r => r.Timestamp)
            .ToList();
    }

    public List<CalculationRecord> FilterByOperationType(OperationType type)
    {
        return _records
            .Where(r => r.OperationType == type)
            .OrderByDescending(r => r.Timestamp)
            .ToList();
    }

    public void ClearHistory()
    {
        _records.Clear();
    }

    public int GetRecordCount() => _records.Count;

    /// <summary>Returns the internal list for serialization purposes.</summary>
    public List<CalculationRecord> GetAllRecordsRaw() => _records;
}

