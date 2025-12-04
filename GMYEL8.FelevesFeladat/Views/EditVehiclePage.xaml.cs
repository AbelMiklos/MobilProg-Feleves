using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views;

public partial class EditVehiclePage : ContentPage
{
	public const string ROUTE = "EditVehiclePage";

    public EditVehiclePage(EditVehicleViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}