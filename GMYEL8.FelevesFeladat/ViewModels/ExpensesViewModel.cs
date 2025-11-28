using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class ExpensesViewModel(IRepository<Expense> repository) : ObservableObject
    {
        private readonly IRepository<Expense> _repository = repository;

        [ObservableProperty]
        private ObservableCollection<Expense> _expenses = [];

        [ObservableProperty]
        private int _currentVehicleId;

        [RelayCommand]
        private async Task LoadExpensesAsync()
        {
            try
            {
                var vehicles = await _repository.Table.ToListAsync();
                if (vehicles.Count > 0)
                {
                    CurrentVehicleId = vehicles[0].Id;
                    var expensesList = await _repository.Table
                        .Where(exp => exp.VehicleId == CurrentVehicleId)
                        .ToListAsync();
                    Expenses = new ObservableCollection<Expense>(expensesList);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", $"Betöltés sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddExpenseAsync()
        {
            // TODO: Navigate to AddExpensePage
            await Shell.Current.DisplayAlert("Info", "Új költség hozzáadása - hamarosan!", "OK");
        }

        [RelayCommand]
        private async Task DeleteExpenseAsync(Expense expense)
        {
            if (expense == null) return;

            bool answer = await Shell.Current.DisplayAlert(
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
