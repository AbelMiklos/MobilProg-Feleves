using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Shared.Repositories;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class HomeViewModel(
        IRepository<Vehicle> vehicleRepository,
        IRepository<FuelRecord> fuelRecordRepository,
        IRepository<Expense> expenseRepository) : ObservableObject
    {
        private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
        private readonly IRepository<FuelRecord> _fuelRecordRepository = fuelRecordRepository;
        private readonly IRepository<Expense> _expenseRepository = expenseRepository;

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

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                var vehicles = await _vehicleRepository.Table.ToListAsync();
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

            var fuelRecords = await _fuelRecordRepository.Table
                .Where(fuel => fuel.VehicleId == CurrentVehicle.Id)
                .ToListAsync();
            var expenses = await _expenseRepository.Table.Where(exp => exp.VehicleId == CurrentVehicle.Id).ToListAsync();

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
