using GMYEL8.FelevesFeladat.Shared.Settings;
using SQLite;

namespace GMYEL8.FelevesFeladat.Infrastructure.Settings;

public class DatabaseSettings : IDatabaseSettings
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public SQLiteOpenFlags OpenFlags { get; set; }
    public IList<Type> Tables { get; set; } = [];
}
