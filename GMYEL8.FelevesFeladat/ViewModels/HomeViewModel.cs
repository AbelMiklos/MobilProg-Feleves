using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Models;
using GMYEL8.FelevesFeladat.Services;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private Vehicle? _currentVehicle;

        [ObservableProperty]
        private double _averageConsumption;

        [ObservableProperty]
        private double _monthlyCost;

        [ObservableProperty]
        private FuelRecord? _lastFuelRecord;

        [ObservableProperty]
        private string _welcomeMessage = "Válassz egy jármûvet";

        public HomeViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                var vehicles = await _databaseService.GetVehiclesAsync();
                if (vehicles.Count > 0)
                {
                    CurrentVehicle = vehicles[0]; // Elsõ jármû betöltése
                    await LoadVehicleStatisticsAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", $"Adatok betöltése sikertelen: {ex.Message}", "OK");
            }
        }

        private async Task LoadVehicleStatisticsAsync()
        {
            if (CurrentVehicle == null) return;

            var fuelRecords = await _databaseService.GetFuelRecordsAsync(CurrentVehicle.Id);
            var expenses = await _databaseService.GetExpensesAsync(CurrentVehicle.Id);

            if (fuelRecords.Count > 0)
            {
                LastFuelRecord = fuelRecords[0];
                AverageConsumption = fuelRecords.Average(f => f.AverageConsumption);
            }

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var monthlyFuelCost = fuelRecords
                .Where(f => f.Date.Month == currentMonth && f.Date.Year == currentYear)
                .Sum(f => f.TotalCost);

            var monthlyExpenseCost = expenses
                .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
                .Sum(e => e.Cost);

            MonthlyCost = monthlyFuelCost + monthlyExpenseCost;

            WelcomeMessage = $"Üdv! Aktuális jármû: {CurrentVehicle.Name}";
        }
    }
}
