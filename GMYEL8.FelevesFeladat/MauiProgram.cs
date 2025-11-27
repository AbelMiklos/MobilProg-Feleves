using Microsoft.Extensions.Logging;
using GMYEL8.FelevesFeladat.Services;
using GMYEL8.FelevesFeladat.ViewModels;
using GMYEL8.FelevesFeladat.Views;

namespace GMYEL8.FelevesFeladat
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Services
            builder.Services.AddSingleton<DatabaseService>();

            // ViewModels
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<FuelRecordsViewModel>();
            builder.Services.AddTransient<AddFuelViewModel>();
            builder.Services.AddTransient<ExpensesViewModel>();
            builder.Services.AddTransient<StatisticsViewModel>();

            // Views
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<FuelRecordsPage>();
            builder.Services.AddTransient<AddFuelPage>();
            builder.Services.AddTransient<ExpensesPage>();
            builder.Services.AddTransient<StatisticsPage>();

            return builder.Build();
        }
    }
}
