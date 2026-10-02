using GarageLog.LocalStorage.Helpers;
using GarageLog.LocalStorage.Repositories;
using GarageLog.LocalStorage.Services;
using GarageLog.Shared.Repositories;
using GarageLog.Shared.Services;
using GarageLog.Shared.Settings;

namespace GarageLog.LocalStorage.DependencyInjection;

public static class DatabaseBuilderExtensions
{
    public static void Add<T>(this IList<Type> _this) where T : class
    {
        _this.Add(typeof(T));
    }

    public static MauiAppBuilder UseDatabase(this MauiAppBuilder builder, Action<IDatabaseSettings> configureDelegate, bool seedData = false)
    {
        var databaseRegistration = new DatabaseRegistration(configureDelegate);
        var databaseService = new DatabaseService(databaseRegistration.CurrentSettings);

        Task.Run(async () =>
        {
            await databaseService.Init();

            // Seed data if requested
            if (seedData)
            {
                await SeedDataHelper.SeedDataAsync(databaseService);
            }
        }).GetAwaiter().GetResult();

        builder.Services.AddSingleton<IDatabaseService>(databaseService);
        builder.Services.AddTransient(typeof(IRepository<>), typeof(DefaultRepository<>));

        return builder;
    }
}
