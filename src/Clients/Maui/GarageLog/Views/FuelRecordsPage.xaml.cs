using GarageLog.ViewModels;

namespace GarageLog.Views;

public partial class FuelRecordsPage : ContentPage
{
    public const string ROUTE = "FuelRecordsPage";

    public FuelRecordsPage(FuelRecordsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is FuelRecordsViewModel viewModel)
        {
            viewModel.LoadFuelRecordsCommand.Execute(null);
        }
    }
}
