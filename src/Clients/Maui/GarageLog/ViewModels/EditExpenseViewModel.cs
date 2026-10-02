using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageLog.Domain.Entities;
using GarageLog.Domain.Enums;
using GarageLog.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GarageLog.ViewModels;

[QueryProperty(nameof(ExpenseId), nameof(ExpenseId))]
public partial class EditExpenseViewModel : ObservableObject
{
    private readonly IRepository<Expense> _expenseRepository;
    private int _expenseId;

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

    [ObservableProperty]
    private bool _isLoading;

    public string ExpenseId
    {
        set
        {
            if (int.TryParse(value, out int expenseId))
            {
                _expenseId = expenseId;
                LoadExpenseAsync();
            }
        }
    }

    public EditExpenseViewModel(IRepository<Expense> expenseRepository)
    {
        _expenseRepository = expenseRepository;
        ExpenseTypes = new ObservableCollection<ExpenseType>(
            Enum.GetValues<ExpenseType>().Cast<ExpenseType>()
        );
    }

    private async void LoadExpenseAsync()
    {
        if (_expenseId <= 0)
            return;

        try
        {
            IsLoading = true;
            var expense = await _expenseRepository.Table
                .Where(e => e.Id == _expenseId)
                .FirstOrDefaultAsync();

            if (expense != null)
            {
                Date = expense.Date;
                SelectedExpenseType = expense.Type;
                Description = expense.Description;
                Cost = expense.Cost.ToString();
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "A költség nem található!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Költség betöltése sikertelen: {ex.Message}", "OK");
            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!await ValidateInputsAsync())
            return;

        try
        {
            var expense = await _expenseRepository.Table
                .Where(e => e.Id == _expenseId)
                .FirstOrDefaultAsync();

            if (expense != null)
            {
                expense.Date = Date;
                expense.Type = SelectedExpenseType;
                expense.Description = Description.Trim();
                expense.Cost = double.Parse(Cost);

                await _expenseRepository.UpdateAsync(expense);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "A költség nem található!", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Mentés sikertelen: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Megerősítés",
            "Biztosan törölni szeretnéd ezt a költséget?",
            "Igen",
            "Nem");

        if (!confirm)
            return;

        try
        {
            var expense = await _expenseRepository.Table
                .Where(e => e.Id == _expenseId)
                .FirstOrDefaultAsync();

            if (expense != null)
            {
                await _expenseRepository.DeleteAsync(expense);
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Törlés sikertelen: {ex.Message}", "OK");
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

        return true;
    }
}