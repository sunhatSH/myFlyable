namespace Flyable.Utils;

public class SettingsReader
{
    private static readonly SettingsReader Instance = new();
    public static string? GetSetting(string key) => Instance[key];
    private SettingsReader() {}
    private string? this[string key]
    {
        get
        {
            ConfigurationBuilder builder = new();
            builder.SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
            .AddJsonFile("appsettings.Development.json");
#else
            .AddJsonFile("appsettings.json");
#endif
            return builder.Build()[key];
        }
    }
}