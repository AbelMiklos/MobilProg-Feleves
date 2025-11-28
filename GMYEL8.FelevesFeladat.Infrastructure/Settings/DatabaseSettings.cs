using GMYEL8.FelevesFeladat.Shared.Settings;
using SQLite;

namespace GMYEL8.FelevesFeladat.Infrastructure.Settings;

public class DatabaseSettings : IDatabaseSettings
{
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public SQLiteOpenFlags OpenFlags { get; set; }
    public IList<Type> Tables { get; set; }

    public DatabaseSettings()
    {
        Tables = new List<Type>();
    }
}
