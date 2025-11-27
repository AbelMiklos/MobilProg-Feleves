using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Models;
using GMYEL8.FelevesFeladat.Services;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class ExpensesViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Expense> _expenses = new();

        [ObservableProperty]
        private int _currentVehicleId;

        public ExpensesViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task LoadExpensesAsync()
        {
            try
            {
                var vehicles = await _databaseService.GetVehiclesAsync();
                if (vehicles.Count > 0)
                {
                    CurrentVehicleId = vehicles[0].Id;
                    var expensesList = await _databaseService.GetExpensesAsync(CurrentVehicleId);
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
                await _databaseService.DeleteExpenseAsync(expense);
                Expenses.Remove(expense);
            }
        }
    }
}
