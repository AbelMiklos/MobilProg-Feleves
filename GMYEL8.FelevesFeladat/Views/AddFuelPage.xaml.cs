using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views
{
    public partial class AddFuelPage : ContentPage
    {
        public const string ROUTE = "AddFuelPage";

        public AddFuelPage(AddFuelViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
