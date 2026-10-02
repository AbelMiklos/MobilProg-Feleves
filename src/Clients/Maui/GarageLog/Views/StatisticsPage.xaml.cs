using GarageLog.ViewModels;

namespace GarageLog.Views
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
