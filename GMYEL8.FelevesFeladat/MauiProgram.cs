using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Infrastructure.DependencyInjection;
using GMYEL8.FelevesFeladat.ViewModels;
using GMYEL8.FelevesFeladat.Views;
using Microsoft.Extensions.Logging;

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
                })
                .UseDatabase(settings =>
                {
                    settings.FileName = "VehicleExpenses.db3";
                    settings.Tables.Add<Expense>();
                    settings.Tables.Add<FuelRecord>();
                    settings.Tables.Add<Vehicle>();
#if DEBUG
                }, seedData: true);
#else
                });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // ViewModels
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<AddVehicleViewModel>();
            builder.Services.AddTransient<EditVehicleViewModel>();
            builder.Services.AddTransient<FuelRecordsViewModel>();
            builder.Services.AddTransient<AddFuelViewModel>();
            builder.Services.AddTransient<AddExpenseViewModel>();
            builder.Services.AddTransient<EditExpenseViewModel>();
            builder.Services.AddTransient<ExpensesViewModel>();
            builder.Services.AddTransient<StatisticsViewModel>();
            builder.Services.AddTransient<EditFuelViewModel>();

            // Views
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<AddVehiclePage>();
            builder.Services.AddTransient<EditVehiclePage>();
            builder.Services.AddTransient<FuelRecordsPage>();
            builder.Services.AddTransient<AddFuelPage>();
            builder.Services.AddTransient<AddExpensePage>();
            builder.Services.AddTransient<EditExpensePage>();
            builder.Services.AddTransient<ExpensesPage>();
            builder.Services.AddTransient<StatisticsPage>();
            builder.Services.AddTransient<EditFuelPage>();

            return builder.Build();
        }
    }
}
