using Microsoft.Extensions.DependencyInjection;
using GMYEL8.FelevesFeladat.Shared.Services;
using GMYEL8.FelevesFeladat.Helpers;

namespace GMYEL8.FelevesFeladat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();
            
            //// Seed initial data
            //var databaseService = Handler?.MauiContext?.Services.GetService<IDatabaseService>();
            //if (databaseService != null)
            //{
            //    await SeedDataHelper.SeedDataAsync(databaseService);
            //}
        }
    }
}