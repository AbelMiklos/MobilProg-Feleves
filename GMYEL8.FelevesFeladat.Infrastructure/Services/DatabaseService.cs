using GMYEL8.FelevesFeladat.Infrastructure.Settings;
using GMYEL8.FelevesFeladat.Shared.Services;
using GMYEL8.FelevesFeladat.Shared.Settings;
using SQLite;

namespace GMYEL8.FelevesFeladat.Infrastructure.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IDatabaseSettings _settings;

    private SQLiteAsyncConnection _connection;

    public SQLiteAsyncConnection Connection => _connection;

    public DatabaseService(IDatabaseSettings settigns)
    {
        if (string.IsNullOrEmpty(settigns.FileName))
        {
            throw new ArgumentException("A fájlnév nem lehet üres.", nameof(settigns.FileName));
        }

        _settings = settigns;
    }

    public async Task Init()
    {
        if (_connection is not null)
        {
            return;
        }

        _connection = new SQLiteAsyncConnection(_settings.FullName(), _settings.OpenFlags);

        foreach (var table in _settings.Tables)
        {
            await _connection.CreateTableAsync(table);
        }
    }
}
