using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using GMYEL8.FelevesFeladat.Views;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class HomePageViewModel(
        IRepository<Vehicle> vehicleRepository,
        IRepository<FuelRecord> fuelRecordRepository,
        IRepository<Expense> expenseRepository) : ObservableObject
    {
        private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
        private readonly IRepository<FuelRecord> _fuelRecordRepository = fuelRecordRepository;
        private readonly IRepository<Expense> _expenseRepository = expenseRepository;

        [ObservableProperty]
        private ObservableCollection<Vehicle> _vehicles = [];

        [ObservableProperty]
        private Vehicle? _selectedVehicle;

        [ObservableProperty]
        private double _averageConsumption;

        [ObservableProperty]
        private double _monthlyCost;

        [ObservableProperty]
        private FuelRecord? _lastFuelRecord;

        [ObservableProperty]
        private bool _isVehicleSelected;

        async partial void OnSelectedVehicleChanged(Vehicle? value)
        {
            IsVehicleSelected = value != null;
            
            if (IsVehicleSelected)
            {
                await LoadVehicleStatisticsAsync();
            }
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                var vehicles = await _vehicleRepository.Table.ToListAsync();
                Vehicles = new ObservableCollection<Vehicle>(vehicles);

                if (SelectedVehicle != null && !vehicles.Any(v => v.Id == SelectedVehicle.Id))
                {
                    SelectedVehicle = null;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Adatok betöltése sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddVehicleAsync()
        {
            await Shell.Current.GoToAsync(AddVehiclePage.ROUTE);
        }

        [RelayCommand]
        private async Task EditVehicleAsync()
        {
            if (SelectedVehicle == null)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "Válassz ki egy járművet a szerkesztéshez!", "OK");
                return;
            }

            var navigationParameter = new ShellNavigationQueryParameters()
            {
                { "VehicleId", SelectedVehicle.Id.ToString() }
            };

            await Shell.Current.GoToAsync(EditVehiclePage.ROUTE, navigationParameter);
        }

        [RelayCommand]
        private async Task NavigateToFuelRecordsAsync()
        {
            if (SelectedVehicle == null)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "Válassz ki egy járművet!", "OK");
                return;
            }

            var navigationParameter = new ShellNavigationQueryParameters()
            {
                { "VehicleId", SelectedVehicle.Id.ToString() }
            };

            await Shell.Current.GoToAsync(FuelRecordsPage.ROUTE, navigationParameter);
        }

        [RelayCommand]
        private async Task NavigateToExpensesAsync()
        {
            if (SelectedVehicle == null)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "Válassz ki egy járművet!", "OK");
                return;
            }

            var navigationParameter = new ShellNavigationQueryParameters()
            {
                { "VehicleId", SelectedVehicle.Id.ToString() }
            };

            await Shell.Current.GoToAsync(ExpensesPage.ROUTE, navigationParameter);
        }

        [RelayCommand]
        private async Task NavigateToStatisticsAsync()
        {
            if (SelectedVehicle == null)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "Válassz ki egy járművet!", "OK");
                return;
            }

            var navigationParameter = new ShellNavigationQueryParameters()
            {
                { "VehicleId", SelectedVehicle.Id.ToString() }
            };

            await Shell.Current.GoToAsync(StatisticsPage.ROUTE, navigationParameter);
        }

        private async Task LoadVehicleStatisticsAsync()
        {
            if (SelectedVehicle == null)
                return;

            try
            {
                var fuelRecords = await _fuelRecordRepository.Table
                    .Where(fuel => fuel.VehicleId == SelectedVehicle.Id)
                    .OrderByDescending(f => f.Date)
                    .ToListAsync();
                
                var expenses = await _expenseRepository.Table
                    .Where(exp => exp.VehicleId == SelectedVehicle.Id)
                    .ToListAsync();

                if (fuelRecords.Count > 0)
                {
                    LastFuelRecord = fuelRecords[0];
                    AverageConsumption = fuelRecords.Average(f => f.AverageConsumption);
                }
                else
                {
                    LastFuelRecord = null;
                    AverageConsumption = 0;
                }

                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;

                double monthlyFuelCost = fuelRecords
                    .Where(f => f.Date.Month == currentMonth && f.Date.Year == currentYear)
                    .Sum(f => f.TotalCost);

                double monthlyExpenseCost = expenses
                    .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
                    .Sum(e => e.Cost);

                MonthlyCost = monthlyFuelCost + monthlyExpenseCost;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Statisztikák betöltése sikertelen: {ex.Message}", "OK");
            }
        }
    }
}
