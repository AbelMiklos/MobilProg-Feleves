using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is HomeViewModel viewModel)
            {
                viewModel.LoadDataCommand.Execute(null);
            }
        }
    }
}
