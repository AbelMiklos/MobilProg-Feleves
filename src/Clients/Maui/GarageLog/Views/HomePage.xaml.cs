using GarageLog.ViewModels;

namespace GarageLog.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HomePageViewModel viewModel)
        {
            viewModel.LoadDataCommand.Execute(null);
        }
    }
}
