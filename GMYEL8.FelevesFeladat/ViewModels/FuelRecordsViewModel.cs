using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Models;
using GMYEL8.FelevesFeladat.Services;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class FuelRecordsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<FuelRecord> _fuelRecords = new();

        [ObservableProperty]
        private int _currentVehicleId;

        public FuelRecordsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task LoadFuelRecordsAsync()
        {
            try
            {
                // Alapértelmezetten az elsõ jármû adatait töltjük be
                var vehicles = await _databaseService.GetVehiclesAsync();
                if (vehicles.Count > 0)
                {
                    CurrentVehicleId = vehicles[0].Id;
                    var records = await _databaseService.GetFuelRecordsAsync(CurrentVehicleId);
                    FuelRecords = new ObservableCollection<FuelRecord>(records);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", $"Betöltés sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddFuelRecordAsync()
        {
            await Shell.Current.GoToAsync("AddFuelPage");
        }

        [RelayCommand]
        private async Task EditFuelRecordAsync(FuelRecord fuelRecord)
        {
            if (fuelRecord == null) return;
            
            await Shell.Current.GoToAsync($"AddFuelPage?id={fuelRecord.Id}");
        }

        [RelayCommand]
        private async Task DeleteFuelRecordAsync(FuelRecord fuelRecord)
        {
            if (fuelRecord == null) return;

            bool answer = await Shell.Current.DisplayAlert(
                "Törlés megerõsítése",
                $"Biztosan törölni szeretnéd ezt a tankolást?",
                "Igen",
                "Nem");

            if (answer)
            {
                await _databaseService.DeleteFuelRecordAsync(fuelRecord);
                FuelRecords.Remove(fuelRecord);
            }
        }
    }
}
