using GMYEL8.FelevesFeladat.Shared.Repositories;
using GMYEL8.FelevesFeladat.Shared.Services;
using SQLite;

namespace GMYEL8.FelevesFeladat.Infrastructure.Repositories;

public class DefaultRepository<T> : IRepository<T>
    where T : class, new()
{
    private readonly IDatabaseService _dbService;

    public DefaultRepository(IDatabaseService dbService)
    {
        _dbService = dbService;
        _dbService.Init().Wait();
    }

    public IDatabaseService DbService => _dbService;

    public AsyncTableQuery<T> Table => _dbService.Connection.Table<T>();

    public async Task<int> DeleteAsync(T item) => await _dbService.Connection.DeleteAsync(item);
    public async Task<int> InsertAsync(T item) => await _dbService.Connection.InsertAsync(item);
    public async Task<int> UpdateAsync(T item) => await _dbService.Connection.UpdateAsync(item);
}
