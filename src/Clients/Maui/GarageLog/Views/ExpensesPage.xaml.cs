using GarageLog.ViewModels;

namespace GarageLog.Views
{
    public partial class ExpensesPage : ContentPage
    {
        public const string ROUTE = "ExpensesPage";

        public ExpensesPage(ExpensesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (BindingContext is ExpensesViewModel viewModel)
            {
                viewModel.LoadExpensesCommand.Execute(null);
            }
        }
    }
}
