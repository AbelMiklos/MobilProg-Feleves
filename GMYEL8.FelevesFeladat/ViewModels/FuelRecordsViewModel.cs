using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    public partial class FuelRecordsViewModel(IRepository<FuelRecord> repository) : ObservableObject
    {
        private readonly IRepository<FuelRecord> _repository = repository;

        [ObservableProperty]
        private ObservableCollection<FuelRecord> _fuelRecords = new();

        [ObservableProperty]
        private int _currentVehicleId;

        [RelayCommand]
        private async Task LoadFuelRecordsAsync()
        {
            try
            {
                // Alapértelmezetten az elsõ jármû adatait töltjük be
                var vehicles = await _repository.Table
                    .ToListAsync();
                if (vehicles.Count > 0)
                {
                    CurrentVehicleId = vehicles[0].Id;
                    var records = await _repository.Table
                        .Where(fr => fr.VehicleId == CurrentVehicleId)
                        .ToListAsync();

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
                await _repository.DeleteAsync(fuelRecord);
                FuelRecords.Remove(fuelRecord);
            }
        }
    }
}
