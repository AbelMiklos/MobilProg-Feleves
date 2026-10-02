using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageLog.Domain.Entities;
using GarageLog.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GarageLog.ViewModels;

[QueryProperty(nameof(VehicleId), nameof(VehicleId))]
public partial class FuelRecordsViewModel(IRepository<FuelRecord> repository) : ObservableObject
{
    private readonly IRepository<FuelRecord> _repository = repository;

    [ObservableProperty]
    private ObservableCollection<FuelRecord> _fuelRecords = [];

    [ObservableProperty]
    private int _currentVehicleId;

    public string VehicleId
    {
        set
        {
            if (int.TryParse(value, out int vehicleId))
            {
                CurrentVehicleId = vehicleId;
                _ = LoadFuelRecordsAsync();
            }
        }
    }

    [RelayCommand]
    private async Task LoadFuelRecordsAsync()
    {
        try
        {
            if (CurrentVehicleId > 0)
            {
                var records = await _repository.Table
                    .Where(fr => fr.VehicleId == CurrentVehicleId)
                    .OrderByDescending(fr => fr.Date)
                    .ToListAsync();

                FuelRecords = new ObservableCollection<FuelRecord>(records);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Betöltés sikertelen: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task AddFuelRecordAsync()
    {
        var navigationParameter = new ShellNavigationQueryParameters()
        {
            { "VehicleId", CurrentVehicleId }
        };
        
        await Shell.Current.GoToAsync("AddFuelPage", navigationParameter);
    }

    [RelayCommand]
    private async Task EditFuelRecordAsync(FuelRecord fuelRecord)
    {
        if (fuelRecord == null)
            return;

        var navigationParameter = new ShellNavigationQueryParameters()
        {
            { "FuelRecordId", fuelRecord.Id.ToString() }
        };

        await Shell.Current.GoToAsync("EditFuelPage", navigationParameter);
    }

    [RelayCommand]
    private async Task DeleteFuelRecordAsync(FuelRecord fuelRecord)
    {
        if (fuelRecord == null) return;

        bool answer = await Shell.Current.DisplayAlertAsync(
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
