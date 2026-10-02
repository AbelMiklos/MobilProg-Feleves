using SQLite;

namespace GarageLog.Shared.Settings;

public interface IDatabaseSettings
{
    string FilePath { get; set; }
    string FileName { get; set; }
    SQLiteOpenFlags OpenFlags { get; set; }
    IList<Type> Tables { get; set; }
}
