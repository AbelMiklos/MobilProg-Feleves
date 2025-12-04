using GMYEL8.FelevesFeladat.Views;

namespace GMYEL8.FelevesFeladat;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(AddFuelPage.ROUTE, typeof(AddFuelPage));
        Routing.RegisterRoute(AddVehiclePage.ROUTE, typeof(AddVehiclePage));
        Routing.RegisterRoute(EditVehiclePage.ROUTE, typeof(EditVehiclePage));
    }
}
