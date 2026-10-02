using GarageLog.ViewModels;

namespace GarageLog.Views;

public partial class EditExpensePage : ContentPage
{
    public const string ROUTE = "EditExpensePage";

    public EditExpensePage(EditExpenseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}