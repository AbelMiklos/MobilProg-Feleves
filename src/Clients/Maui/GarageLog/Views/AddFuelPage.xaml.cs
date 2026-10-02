using GarageLog.ViewModels;

namespace GarageLog.Views
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
