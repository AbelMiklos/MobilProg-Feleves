using GarageLog.ViewModels;

namespace GarageLog.Views;

public partial class EditVehiclePage : ContentPage
{
	public const string ROUTE = "EditVehiclePage";

    public EditVehiclePage(EditVehicleViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}