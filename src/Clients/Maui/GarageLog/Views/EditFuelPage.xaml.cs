using GarageLog.ViewModels;

namespace GarageLog.Views;

public partial class EditFuelPage : ContentPage
{
    public const string ROUTE = "EditFuelPage";

    public EditFuelPage(EditFuelViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}