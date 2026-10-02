using GarageLog.ViewModels;

namespace GarageLog.Views;

public partial class AddExpensePage : ContentPage
{
	public const string ROUTE = "AddExpensePage";

    public AddExpensePage(AddExpenseViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}