using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views
{
    public partial class StatisticsPage : ContentPage
    {
        public const string ROUTE = "StatisticsPage";

        public StatisticsPage(StatisticsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
