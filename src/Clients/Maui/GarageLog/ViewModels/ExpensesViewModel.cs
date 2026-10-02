using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageLog.Domain.Entities;
using GarageLog.Shared.Repositories;
using GarageLog.Views;
using System.Collections.ObjectModel;

namespace GarageLog.ViewModels
{
    [QueryProperty(nameof(VehicleId), nameof(VehicleId))]
    public partial class ExpensesViewModel(IRepository<Expense> repository) : ObservableObject
    {
        private readonly IRepository<Expense> _repository = repository;

        [ObservableProperty]
        private ObservableCollection<Expense> _expenses = [];

        [ObservableProperty]
        private int _currentVehicleId;

        public string VehicleId
        {
            set
            {
                if (int.TryParse(value, out int vehicleId))
                {
                    CurrentVehicleId = vehicleId;
                    _ = LoadExpensesAsync();
                }
            }
        }

        [RelayCommand]
        private async Task LoadExpensesAsync()
        {
            try
            {
                if (CurrentVehicleId > 0)
                {
                    var expensesList = await _repository.Table
                        .Where(exp => exp.VehicleId == CurrentVehicleId)
                        .OrderByDescending(exp => exp.Date)
                        .ToListAsync();
                    
                    Expenses = new ObservableCollection<Expense>(expensesList);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Betöltés sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddExpenseAsync()
        {
            var navigationParameter = new ShellNavigationQueryParameters()
            {
                { "VehicleId", CurrentVehicleId.ToString() }
            };

            await Shell.Current.GoToAsync(AddExpensePage.ROUTE, navigationParameter);
        }

        [RelayCommand]
        private async Task EditExpenseAsync(Expense expense)
        {
            if (expense == null) return;

            var navigationParameter = new ShellNavigationQueryParameters()
            {
                { "ExpenseId", expense.Id.ToString() }
            };

            await Shell.Current.GoToAsync(EditExpensePage.ROUTE, navigationParameter);
        }

        [RelayCommand]
        private async Task DeleteExpenseAsync(Expense expense)
        {
            if (expense == null) return;

            bool answer = await Shell.Current.DisplayAlertAsync(
                "Törlés megerõsítése",
                $"Biztosan törölni szeretnéd ezt a költséget?",
                "Igen",
                "Nem");

            if (answer)
            {
                await _repository.DeleteAsync(expense);
                Expenses.Remove(expense);
            }
        }
    }
}
