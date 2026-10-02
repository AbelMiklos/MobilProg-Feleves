using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Domain.Enums;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels;

public partial class AddVehicleViewModel : ObservableObject
{
    private readonly IRepository<Vehicle> _vehicleRepository;

    public AddVehicleViewModel(IRepository<Vehicle> vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
        VehicleTypes = new ObservableCollection<VehicleType>(
            Enum.GetValues<VehicleType>().Cast<VehicleType>()
        );
        SelectedVehicleType = VehicleType.Car;
        Year = DateTime.Now.Year;
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

        const int minYear = 1900;
        if (Year < minYear || Year > DateTime.Now.Year + 1)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Az évjárat nem lehet kisebb {minYear}-nál és nem lehet nagyobb {DateTime.Now.Year + 1}-nél!", "OK");
            return;
        }

        try
        {
            var vehicle = new Vehicle
            {
                Name = Name.Trim(),
                Type = SelectedVehicleType,
                LicensePlate = LicensePlate.Trim().ToUpper(),
                Year = Year
            };

            await _vehicleRepository.InsertAsync(vehicle);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Hiba", $"Jármű mentése sikertelen: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
