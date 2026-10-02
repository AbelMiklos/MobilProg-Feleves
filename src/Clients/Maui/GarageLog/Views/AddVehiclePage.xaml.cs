using GarageLog.ViewModels;

namespace GarageLog.Views;

public partial class AddVehiclePage : ContentPage
{
    public const string ROUTE = "AddVehiclePage";

    public AddVehiclePage(AddVehicleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}