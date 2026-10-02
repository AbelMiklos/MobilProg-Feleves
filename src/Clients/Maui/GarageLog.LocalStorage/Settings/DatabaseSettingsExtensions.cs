using GarageLog.Shared.Settings;

namespace GarageLog.LocalStorage.Settings;

public static class DatabaseSettingsExtensions
{
    public static string FullName(this IDatabaseSettings settings) => Path.Combine(settings.FilePath, settings.FileName);
}
