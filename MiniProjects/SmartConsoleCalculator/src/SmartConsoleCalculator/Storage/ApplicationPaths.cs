namespace SmartConsoleCalculator.Storage;

public static class ApplicationPaths
{
    private const string AppFolderName = "SmartConsoleCalculator";
    private const string HistoryFileName = "history.json";
    private const string SettingsFileName = "settings.json";

    public static string GetDataDirectory()
    {
        string? envDir = Environment.GetEnvironmentVariable("SMART_CALC_DATA_DIR");
        if (!string.IsNullOrWhiteSpace(envDir))
        {
            return envDir;
        }

        try
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!string.IsNullOrWhiteSpace(localAppData))
            {
                string target = Path.Combine(localAppData, AppFolderName);
                Directory.CreateDirectory(target);

                // Probe write permission
                string probeFile = Path.Combine(target, ".probe_" + Guid.NewGuid().ToString("N"));
                File.WriteAllText(probeFile, "test");
                File.Delete(probeFile);

                return target;
            }
        }
        catch
        {
            // Fall back when LocalApplicationData is restricted or unwritable
        }

        string fallback = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".data");
        try
        {
            Directory.CreateDirectory(fallback);
        }
        catch
        {
            // In case of any issue, return fallback path
        }
        return fallback;
    }

    public static string GetHistoryFilePath() => Path.Combine(GetDataDirectory(), HistoryFileName);

    public static string GetSettingsFilePath() => Path.Combine(GetDataDirectory(), SettingsFileName);

    public static void EnsureDataDirectoryExists()
    {
        string dir = GetDataDirectory();
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    // Overloads for testing with custom directories
    public static string GetHistoryFilePath(string baseDir) => Path.Combine(baseDir, HistoryFileName);

    public static string GetSettingsFilePath(string baseDir) => Path.Combine(baseDir, SettingsFileName);
}

