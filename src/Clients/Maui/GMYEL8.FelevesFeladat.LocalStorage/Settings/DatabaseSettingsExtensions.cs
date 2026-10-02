using GMYEL8.FelevesFeladat.Shared.Settings;

namespace GMYEL8.FelevesFeladat.Infrastructure.Settings;

public static class DatabaseSettingsExtensions
{
    public static string FullName(this IDatabaseSettings settings) => Path.Combine(settings.FilePath, settings.FileName);
}
