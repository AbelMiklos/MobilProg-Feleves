using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views
{
    public partial class ExpensesPage : ContentPage
    {
        public const string ROUTE = "ExpensesPage";

        public ExpensesPage(ExpensesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
