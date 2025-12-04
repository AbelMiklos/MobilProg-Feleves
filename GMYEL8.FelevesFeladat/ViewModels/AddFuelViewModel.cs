using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Shared.Repositories;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    [QueryProperty(nameof(VehicleId), nameof(VehicleId))]
    public partial class AddFuelViewModel(IRepository<FuelRecord> fuelRecordRepository, IRepository<Vehicle> vehicleRepository) : ObservableObject
    {
        private readonly IRepository<FuelRecord> _fuelRecordRepository = fuelRecordRepository;
        private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;

        [ObservableProperty]
        private int _fuelRecordId;

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
        private string? _receiptPhotoPath;

        [ObservableProperty]
        private double? _latitude;

        [ObservableProperty]
        private double? _longitude;

        [ObservableProperty]
        private bool _isEditMode;

        partial void OnFuelRecordIdChanged(int value)
        {
            if (value > 0)
            {
                IsEditMode = true;
                LoadFuelRecordAsync(value);
            }
        }

        private async void LoadFuelRecordAsync(int id)
        {
            var record = await _fuelRecordRepository.Table.FirstOrDefaultAsync(fuel => fuel.Id == id);
            if (record != null)
            {
                Date = record.Date;
                Distance = record.Distance.ToString();
                FuelAmount = record.FuelAmount.ToString();
                PricePerLitre = record.PricePerLitre.ToString();
                ReceiptPhotoPath = record.ReceiptPhotoPath;
                Latitude = record.Latitude;
                Longitude = record.Longitude;
            }
        }

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
                    await Shell.Current.DisplayAlertAsync("Hiba", "Nincs jármû kiválasztva!", "OK");
                    return;
                }

                var fuelRecord = new FuelRecord
                {
                    Id = FuelRecordId,
                    VehicleId = vehicle.Id,
                    Date = Date,
                    Distance = double.Parse(Distance),
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
                await Shell.Current.DisplayAlertAsync("Hiba", $"Mentés sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task TakePhotoAsync()
        {
            try
            {
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
                    await Shell.Current.DisplayAlertAsync("Sikeres",
                        $"Hely mentve: {Latitude:F6}, {Longitude:F6}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Helymeghatározás sikertelen: {ex.Message}", "OK");
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
                Shell.Current.DisplayAlertAsync("Hiba", "Kérlek adj meg érvényes kilométert!", "OK");
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
