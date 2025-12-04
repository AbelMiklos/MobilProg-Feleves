using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views;

public partial class EditFuelPage : ContentPage
{
    public const string ROUTE = "EditFuelPage";

    public EditFuelPage(EditFuelViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}