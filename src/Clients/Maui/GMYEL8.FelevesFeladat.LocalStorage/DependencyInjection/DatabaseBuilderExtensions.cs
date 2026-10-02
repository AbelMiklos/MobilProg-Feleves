using GMYEL8.FelevesFeladat.Infrastructure.Helpers;
using GMYEL8.FelevesFeladat.Infrastructure.Repositories;
using GMYEL8.FelevesFeladat.Infrastructure.Services;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using GMYEL8.FelevesFeladat.Shared.Services;
using GMYEL8.FelevesFeladat.Shared.Settings;

namespace GMYEL8.FelevesFeladat.Infrastructure.DependencyInjection;

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
