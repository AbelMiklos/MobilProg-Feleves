using GarageLog.Views;

namespace GarageLog;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(FuelRecordsPage.ROUTE, typeof(FuelRecordsPage));
        Routing.RegisterRoute(ExpensesPage.ROUTE, typeof(ExpensesPage));
        Routing.RegisterRoute(StatisticsPage.ROUTE, typeof(StatisticsPage));
        Routing.RegisterRoute(AddFuelPage.ROUTE, typeof(AddFuelPage));
        Routing.RegisterRoute(AddExpensePage.ROUTE, typeof(AddExpensePage));
        Routing.RegisterRoute(EditExpensePage.ROUTE, typeof(EditExpensePage));
        Routing.RegisterRoute(AddVehiclePage.ROUTE, typeof(AddVehiclePage));
        Routing.RegisterRoute(EditVehiclePage.ROUTE, typeof(EditVehiclePage));
        Routing.RegisterRoute(EditFuelPage.ROUTE, typeof(EditFuelPage));
    }
}
