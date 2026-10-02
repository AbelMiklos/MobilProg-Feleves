using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Domain.Enums;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels;

[QueryProperty(nameof(VehicleId), nameof(VehicleId))]
public partial class EditVehicleViewModel : ObservableObject
{
    private readonly IRepository<Vehicle> _vehicleRepository;
    private int _vehicleId;

    public EditVehicleViewModel(IRepository<Vehicle> vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
        VehicleTypes = new ObservableCollection<VehicleType>(
            Enum.GetValues<VehicleType>().Cast<VehicleType>()
        );
    }

    public int VehicleId
    {
        get => _vehicleId;
        set
        {
            _vehicleId = value;
            LoadVehicleAsync();
        }
    }

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private VehicleType _selectedVehicleType;

    [ObservableProperty]
    private string _licensePlate = string.Empty;

    [ObservableProperty]
    private int _year;

    [ObservableProperty]
    private ObservableCollection<VehicleType> _vehicleTypes = [];

    [ObservableProperty]
    private bool _isLoading;

    private async void LoadVehicleAsync()
    {
        if (_vehicleId <= 0)
            return;

        try
        {
            IsLoading = true;
            var vehicle = await _vehicleRepository.Table
                .Where(v => v.Id == _vehicleId)
                .FirstOrDefaultAsync();

            if (vehicle != null)
            {
                Name = vehicle.Name;
                SelectedVehicleType = vehicle.Type;
                LicensePlate = vehicle.LicensePlate;
                Year = vehicle.Year;
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "A jármű nem található!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Jármű betöltése sikertelen: {ex.Message}", "OK");
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
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Hiba", "A jármű nevét kötelező megadni!", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(LicensePlate))
        {
            await Shell.Current.DisplayAlertAsync("Hiba", "A rendszámot kötelező megadni!", "OK");
            return;
        }

        if (Year < 1900 || Year > DateTime.Now.Year + 1)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Az évjárat nem lehet kisebb 1900-nál és nem lehet nagyobb {DateTime.Now.Year + 1}-nél!", "OK");
            return;
        }

        try
        {
            var vehicle = await _vehicleRepository.Table
                .Where(v => v.Id == _vehicleId)
                .FirstOrDefaultAsync();

            if (vehicle != null)
            {
                vehicle.Name = Name.Trim();
                vehicle.Type = SelectedVehicleType;
                vehicle.LicensePlate = LicensePlate.Trim().ToUpper();
                vehicle.Year = Year;

                await _vehicleRepository.UpdateAsync(vehicle);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Hiba", "A jármű nem található!", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Jármű mentése sikertelen: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Megerősítés",
            "Biztosan törölni szeretnéd ezt a járművet? Ez törli az összes hozzá tartozó tankolási és költség adatot is!",
            "Igen",
            "Nem");

        if (!confirm)
            return;

        try
        {
            var vehicle = await _vehicleRepository.Table
                .Where(v => v.Id == _vehicleId)
                .FirstOrDefaultAsync();

            if (vehicle != null)
            {
                await _vehicleRepository.DeleteAsync(vehicle);
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Jármű törlése sikertelen: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
