using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Domain.Enums;
using GMYEL8.FelevesFeladat.Shared.Repositories;
using System.Collections.ObjectModel;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    [QueryProperty(nameof(VehicleId), nameof(VehicleId))]
    public partial class AddFuelViewModel : ObservableObject
    {
        private readonly IRepository<FuelRecord> _fuelRecordRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public AddFuelViewModel(IRepository<FuelRecord> fuelRecordRepository, IRepository<Vehicle> vehicleRepository)
        {
            _fuelRecordRepository = fuelRecordRepository;
            _vehicleRepository = vehicleRepository;
            FuelTypes = new ObservableCollection<FuelType>(
                Enum.GetValues<FuelType>().Cast<FuelType>()
            );
            SelectedFuelType = FuelType.Petrol;
        }

        [ObservableProperty]
        private int _vehicleId;

        [ObservableProperty]
        private DateTime _date = DateTime.Now;

        [ObservableProperty]
        private string _distance = string.Empty;

        [ObservableProperty]
        private string _fuelAmount = string.Empty;

        [ObservableProperty]
        private string _pricePerLitre = string.Empty;

        [ObservableProperty]
        private FuelType _selectedFuelType;

        [ObservableProperty]
        private ObservableCollection<FuelType> _fuelTypes = [];

        [ObservableProperty]
        private string? _receiptPhotoPath;

        [ObservableProperty]
        private double? _latitude;

        [ObservableProperty]
        private double? _longitude;

        [ObservableProperty]
        private string? _locationAddress;

        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                if (!ValidateInputs())
                    return;

                var vehicle = await _vehicleRepository.Table
                    .FirstOrDefaultAsync(v => v.Id == VehicleId);

                if (vehicle == null)
                {
                    await Shell.Current.DisplayAlertAsync("Hiba", "Nincs j�rm� kiv�lasztva!", "OK");
                    return;
                }

                var fuelRecord = new FuelRecord
                {
                    VehicleId = vehicle.Id,
                    Date = Date,
                    Distance = double.Parse(Distance),
                    Type = SelectedFuelType,
                    FuelAmount = double.Parse(FuelAmount),
                    PricePerLitre = double.Parse(PricePerLitre),
                    ReceiptPhotoPath = ReceiptPhotoPath,
                    Latitude = Latitude,
                    Longitude = Longitude
                };

                await _fuelRecordRepository.InsertAsync(fuelRecord);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Ment�s sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task TakePhotoAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await Shell.Current.DisplayAlertAsync("Hiba", "A kamera használatához engedély szükséges!", "OK");
                        return;
                    }
                }

                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        string newFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                        using (var stream = await photo.OpenReadAsync())
                        using (var newStream = File.OpenWrite(newFilePath))
                        {
                            await stream.CopyToAsync(newStream);
                        }
                        ReceiptPhotoPath = newFilePath;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Fotó készítése sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task GetLocationAsync()
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(15)
                });

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    
                    var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();
                    
                    if (placemark != null)
                    {
                        LocationAddress = $"{placemark.Thoroughfare} {placemark.SubThoroughfare}, {placemark.Locality}";
                    }
                    else
                    {
                        LocationAddress = $"{Latitude:F6}, {Longitude:F6}";
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Helymeghat�roz�s sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task OpenMapAsync()
        {
            try
            {
                if (Latitude.HasValue && Longitude.HasValue)
                {
                    var location = new Location(Latitude.Value, Longitude.Value);
                    var options = new MapLaunchOptions { Name = LocationAddress ?? "Tankol�s helysz�ne" };
                    await Map.Default.OpenAsync(location, options);
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Hiba", "Nincs mentett helysz�n!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"T�rk�p megnyit�sa sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Distance) || !double.TryParse(Distance, out _))
            {
                Shell.Current.DisplayAlertAsync("Hiba", "Kérlek adj meg érvényes kilómétert!", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(FuelAmount) || !double.TryParse(FuelAmount, out _))
            {
                Shell.Current.DisplayAlertAsync("Hiba", "Kérlek adj meg érvényes üzemanyag mennyiséget!", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PricePerLitre) || !double.TryParse(PricePerLitre, out _))
            {
                Shell.Current.DisplayAlertAsync("Hiba", "Kérlek adj meg érvényes árat!", "OK");
                return false;
            }

            return true;
        }
    }
}
