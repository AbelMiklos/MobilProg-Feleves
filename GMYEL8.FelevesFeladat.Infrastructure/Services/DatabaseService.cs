using GMYEL8.FelevesFeladat.Infrastructure.Settings;
using GMYEL8.FelevesFeladat.Shared.Services;
using GMYEL8.FelevesFeladat.Shared.Settings;
using SQLite;

namespace GMYEL8.FelevesFeladat.Infrastructure.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IDatabaseSettings _settings;

    public SQLiteAsyncConnection Connection { get; private set; }

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
        if (Connection is not null)
        {
            return;
        }

        Connection = new SQLiteAsyncConnection(_settings.FullName(), _settings.OpenFlags);

        foreach (var table in _settings.Tables)
        {
            await Connection.CreateTableAsync(table);
        }
    }
}
