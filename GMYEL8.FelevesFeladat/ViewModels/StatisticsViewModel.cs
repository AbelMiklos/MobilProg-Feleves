using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Services;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class StatisticsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private double _averageConsumption;

        [ObservableProperty]
        private double _totalFuelCost;

        [ObservableProperty]
        private double _totalExpenses;

        [ObservableProperty]
        private double _totalCost;

        [ObservableProperty]
        private string _statisticsInfo = "Statisztikák betöltése...";

        public StatisticsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task LoadStatisticsAsync()
        {
            try
            {
                var vehicles = await _databaseService.GetVehiclesAsync();
                if (vehicles.Count > 0)
                {
                    var vehicleId = vehicles[0].Id;
                    var fuelRecords = await _databaseService.GetFuelRecordsAsync(vehicleId);
                    var expenses = await _databaseService.GetExpensesAsync(vehicleId);

                    if (fuelRecords.Count > 0)
                    {
                        AverageConsumption = fuelRecords.Average(f => f.AverageConsumption);
                        TotalFuelCost = fuelRecords.Sum(f => f.TotalCost);
                    }

                    TotalExpenses = expenses.Sum(e => e.Cost);
                    TotalCost = TotalFuelCost + TotalExpenses;

                    StatisticsInfo = $"Tankolások száma: {fuelRecords.Count}\n" +
                                   $"Költségek száma: {expenses.Count}\n" +
                                   $"Átlagfogyasztás: {AverageConsumption:F2} L/100km\n" +
                                   $"Összes üzemanyag költség: {TotalFuelCost:F0} Ft\n" +
                                   $"Összes egyéb költség: {TotalExpenses:F0} Ft\n" +
                                   $"Teljes költség: {TotalCost:F0} Ft";
                }
                else
                {
                    StatisticsInfo = "Nincs még jármû rögzítve.";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", $"Statisztikák betöltése sikertelen: {ex.Message}", "OK");
            }
        }
    }
}
