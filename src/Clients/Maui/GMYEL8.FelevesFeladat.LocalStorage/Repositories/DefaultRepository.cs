using GMYEL8.FelevesFeladat.Shared.Repositories;
using GMYEL8.FelevesFeladat.Shared.Services;
using SQLite;

namespace GMYEL8.FelevesFeladat.Infrastructure.Repositories;

public class DefaultRepository<T> : IRepository<T>
    where T : class, new()
{
    public DefaultRepository(IDatabaseService dbService)
    {
        DbService = dbService;
        DbService.Init().Wait();
    }

    public IDatabaseService DbService { get; }

    public AsyncTableQuery<T> Table => DbService.Connection.Table<T>();

    public async Task<int> DeleteAsync(T item) => await DbService.Connection.DeleteAsync(item);
    public async Task<int> InsertAsync(T item) => await DbService.Connection.InsertAsync(item);
    public async Task<int> UpdateAsync(T item) => await DbService.Connection.UpdateAsync(item);
}
