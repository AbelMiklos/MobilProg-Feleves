using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views;

public partial class AddVehiclePage : ContentPage
{
    public const string ROUTE = "AddVehiclePage";

    public AddVehiclePage(AddVehicleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}