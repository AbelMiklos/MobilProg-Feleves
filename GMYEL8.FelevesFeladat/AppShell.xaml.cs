using GMYEL8.FelevesFeladat.Views;

namespace GMYEL8.FelevesFeladat
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Register routes for navigation
            Routing.RegisterRoute("AddFuelPage", typeof(AddFuelPage));
        }
    }
}
