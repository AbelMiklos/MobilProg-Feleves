using SQLite;

namespace GarageLog.Shared.Services;

public interface IDatabaseService
{
    SQLiteAsyncConnection Connection { get; }
    Task Init();
}
