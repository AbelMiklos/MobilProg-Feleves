using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views
{
    public partial class ExpensesPage : ContentPage
    {
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
