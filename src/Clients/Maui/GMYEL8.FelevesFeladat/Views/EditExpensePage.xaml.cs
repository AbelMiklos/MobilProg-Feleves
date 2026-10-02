using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views;

public partial class EditExpensePage : ContentPage
{
    public const string ROUTE = "EditExpensePage";

    public EditExpensePage(EditExpenseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}