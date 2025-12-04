using GMYEL8.FelevesFeladat.Infrastructure.Settings;
using GMYEL8.FelevesFeladat.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMYEL8.FelevesFeladat.Infrastructure.DependencyInjection;

public class DatabaseRegistration
{
    public IDatabaseSettings CurrentSettings { get; }

    public DatabaseRegistration(Action<IDatabaseSettings> settingsDelegate)
    {
        CurrentSettings = GetDefaultSettings();

        settingsDelegate?.Invoke(CurrentSettings);
    }

    internal IDatabaseSettings GetDefaultSettings()
    {
        return new DatabaseSettings
        {
            FilePath = FileSystem.Current.AppDataDirectory,
            FileName = string.Empty,
            OpenFlags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create,
        };
    }
}
