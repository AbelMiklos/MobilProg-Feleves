using SQLite;

namespace GMYEL8.FelevesFeladat.Shared.Services;

public interface IDatabaseService
{
    SQLiteAsyncConnection Connection { get; }
    Task Init();
}
