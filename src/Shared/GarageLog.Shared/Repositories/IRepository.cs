using GarageLog.Shared.Services;
using SQLite;

namespace GarageLog.Shared.Repositories;

public interface IRepository<T> where T : class, new()
{
    IDatabaseService DbService { get; }
    AsyncTableQuery<T> Table { get; }
    Task<int> InsertAsync(T item);
    Task<int> UpdateAsync(T item);
    Task<int> DeleteAsync(T item);
}
