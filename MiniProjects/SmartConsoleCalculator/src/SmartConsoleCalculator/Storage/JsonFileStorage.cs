using System.Text.Json;

namespace SmartConsoleCalculator.Storage;

public class JsonFileStorage
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public (bool Success, T? Data, string Error) Load<T>(string filePath) where T : class
    {
        try
        {
            if (!File.Exists(filePath))
                return (false, default, "File not found.");

            string content = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(content))
                return (false, default, "File is empty.");

            T? data = JsonSerializer.Deserialize<T>(content, SerializerOptions);

            if (data is null)
                return (false, default, "Deserialization returned null.");

            return (true, data, string.Empty);
        }
        catch (JsonException ex)
        {
            // Rename corrupted file as backup
            try
            {
                string backupPath = filePath + ".corrupt.bak";
                if (File.Exists(backupPath))
                    File.Delete(backupPath);
                File.Move(filePath, backupPath);
            }
            catch
            {
                // If we can't rename, continue anyway
            }

            return (false, default, $"Invalid JSON: {ex.Message}. The corrupted file has been backed up.");
        }
        catch (IOException ex)
        {
            return (false, default, $"File access error: {ex.Message}");
        }
    }

    public (bool Success, string Error) Save<T>(string filePath, T data) where T : class
    {
        string tempPath = filePath + ".tmp";

        try
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string json = JsonSerializer.Serialize(data, SerializerOptions);
            File.WriteAllText(tempPath, json);

            // Atomic replacement: delete target then move temp
            if (File.Exists(filePath))
                File.Delete(filePath);
            File.Move(tempPath, filePath);

            return (true, string.Empty);
        }
        catch (IOException ex)
        {
            // Clean up temp file if the move failed but temp was written
            try
            {
                if (File.Exists(tempPath) && File.Exists(filePath))
                    File.Delete(tempPath);
            }
            catch
            {
                // Don't delete temp if it might contain the only good copy
            }

            return (false, $"Save failed for '{filePath}': {ex.Message}");
        }
        catch (Exception ex)
        {
            return (false, $"Save failed for '{filePath}': {ex.Message}");
        }
    }
}

