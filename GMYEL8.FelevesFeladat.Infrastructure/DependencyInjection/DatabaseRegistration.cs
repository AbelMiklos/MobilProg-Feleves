using GMYEL8.FelevesFeladat.Infrastructure.Settings;
using GMYEL8.FelevesFeladat.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMYEL8.FelevesFeladat.Infrastructure.DependencyInjection;

public class DatabaseRegistration
{
    private readonly IDatabaseSettings _databaseSettings;

    public IDatabaseSettings CurrentSettings => _databaseSettings;

    public DatabaseRegistration(Action<IDatabaseSettings> settingsDelegate)
    {
        _databaseSettings = GetDefaultSettings();

        settingsDelegate?.Invoke(_databaseSettings);
    }

    internal IDatabaseSettings GetDefaultSettings()
    {
        return new DatabaseSettings
        {
            FilePath = FileSystem.Current.AppDataDirectory,
            FileName = null,
            OpenFlags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create,
        };
    }
}
