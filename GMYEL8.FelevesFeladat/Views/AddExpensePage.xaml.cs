using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views;

public partial class AddExpensePage : ContentPage
{
	public const string ROUTE = "AddExpensePage";

    public AddExpensePage(AddExpenseViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}