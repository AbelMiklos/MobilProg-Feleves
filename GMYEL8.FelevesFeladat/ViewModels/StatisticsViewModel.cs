using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Domain.Enums;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels;

public class ExpenseGroupModel
{
    public ExpenseType Type { get; set; }
    public double TotalCost { get; set; }
    public int Count { get; set; }
}

[QueryProperty(nameof(VehicleId), nameof(VehicleId))]
public partial class StatisticsViewModel(
    IRepository<Vehicle> vehicleRepository,
    IRepository<FuelRecord> fuelRecordRepository,
    IRepository<Expense> expenseRepository) : ObservableObject
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IRepository<FuelRecord> _fuelRecordRepository = fuelRecordRepository;
    private readonly IRepository<Expense> _expenseRepository = expenseRepository;

    [ObservableProperty]
    private int _currentVehicleId;

    [ObservableProperty]
    private Vehicle? _currentVehicle;

    [ObservableProperty]
    private double _averageConsumption;

    [ObservableProperty]
    private double _totalFuelCost;

    [ObservableProperty]
    private double _totalExpenses;

    [ObservableProperty]
    private double _totalCost;

    [ObservableProperty]
    private int _fuelRecordCount;

    [ObservableProperty]
    private int _expenseCount;

    [ObservableProperty]
    private bool _hasExpenseGroups;

    [ObservableProperty]
    private ObservableCollection<ExpenseGroupModel> _expenseGroups = [];

    [ObservableProperty]
    private string _statisticsInfo = "Statisztikák betöltése...";

    public string VehicleId
    {
        set
        {
            if (int.TryParse(value, out int vehicleId))
            {
                CurrentVehicleId = vehicleId;
                _ = LoadStatisticsAsync();
            }
        }
    }

    [RelayCommand]
    private async Task LoadStatisticsAsync()
    {
        try
        {
            if (CurrentVehicleId <= 0)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Nincs kiválasztva jármű!", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            CurrentVehicle = await _vehicleRepository.Table
                    .Where(v => v.Id == CurrentVehicleId)
                    .FirstOrDefaultAsync();

            var fuelRecords = await _fuelRecordRepository.Table
                .Where(fuel => fuel.VehicleId == CurrentVehicleId)
                .ToListAsync();

            var expenses = await _expenseRepository.Table
                .Where(exp => exp.VehicleId == CurrentVehicleId)
                .ToListAsync();

            FuelRecordCount = fuelRecords.Count;
            ExpenseCount = expenses.Count;

            if (fuelRecords.Count > 0)
            {
                AverageConsumption = fuelRecords.Average(f => f.AverageConsumption);
                TotalFuelCost = fuelRecords.Sum(f => f.TotalCost);
            }
            else
            {
                AverageConsumption = 0;
                TotalFuelCost = 0;
            }

            TotalExpenses = expenses.Sum(e => e.Cost);
            TotalCost = TotalFuelCost + TotalExpenses;

            var expenseGroupsList = expenses
                .GroupBy(e => e.Type)
                .Select(g => new ExpenseGroupModel
                {
                    Type = g.Key,
                    TotalCost = g.Sum(e => e.Cost),
                    Count = g.Count()
                })
                .Where(g => g.TotalCost > 0)
                .OrderByDescending(g => g.TotalCost)
                .ToList();

            ExpenseGroups = new ObservableCollection<ExpenseGroupModel>(expenseGroupsList);
            HasExpenseGroups = ExpenseGroups.Count > 0;

            StatisticsInfo = $"Jármű: {CurrentVehicle.Name} - {CurrentVehicle.LicensePlate}\n" +
                           $"Tankolások száma: {FuelRecordCount}\n" +
                           $"Egyéb kiadások száma: {ExpenseCount}\n";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Statisztikák betöltése sikertelen: {ex.Message}", "OK");
        }
    }
}
