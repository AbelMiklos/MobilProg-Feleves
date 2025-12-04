using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Domain.Enums;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels;

[QueryProperty(nameof(VehicleId), nameof(VehicleId))]
public partial class AddExpenseViewModel : ObservableObject
{
    private readonly IRepository<Expense> _expenseRepository;
    private int _vehicleId;

    [ObservableProperty]
    private DateTime _date = DateTime.Now;

    [ObservableProperty]
    private ExpenseType _selectedExpenseType = ExpenseType.Maintenance;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _cost = string.Empty;

    [ObservableProperty]
    private ObservableCollection<ExpenseType> _expenseTypes = [];

    public string VehicleId
    {
        set
        {
            if (int.TryParse(value, out int vehicleId))
            {
                _vehicleId = vehicleId;
            }
        }
    }

    public AddExpenseViewModel(IRepository<Expense> expenseRepository)
    {
        _expenseRepository = expenseRepository;
        ExpenseTypes = new ObservableCollection<ExpenseType>(
            Enum.GetValues<ExpenseType>().Cast<ExpenseType>()
        );
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!await ValidateInputsAsync())
            return;

        try
        {
            var expense = new Expense
            {
                VehicleId = _vehicleId,
                Date = Date,
                Type = SelectedExpenseType,
                Description = Description.Trim(),
                Cost = double.Parse(Cost)
            };

            await _expenseRepository.InsertAsync(expense);
            await Shell.Current.DisplayAlertAsync("Siker", "Költség sikeresen rögzítve!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Mentés sikertelen: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private async Task<bool> ValidateInputsAsync()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlertAsync("Hiba", "A leírás megadása kötelező!", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(Cost) || !double.TryParse(Cost, out double costValue) || costValue <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", "Kérlek adj meg egy érvényes költséget!", "OK");
            return false;
        }

        if (_vehicleId <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", "Nincs kiválasztva jármű!", "OK");
            return false;
        }

        return true;
    }
}