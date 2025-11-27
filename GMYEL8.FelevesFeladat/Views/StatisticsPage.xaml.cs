using GMYEL8.FelevesFeladat.ViewModels;

namespace GMYEL8.FelevesFeladat.Views
{
    public partial class StatisticsPage : ContentPage
    {
        public StatisticsPage(StatisticsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is StatisticsViewModel viewModel)
            {
                viewModel.LoadStatisticsCommand.Execute(null);
            }
        }
    }
}
