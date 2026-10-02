using GarageLog.LocalStorage.Settings;
using GarageLog.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarageLog.LocalStorage.DependencyInjection;

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
