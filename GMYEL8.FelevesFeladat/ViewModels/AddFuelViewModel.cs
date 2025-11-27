using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Models;
using GMYEL8.FelevesFeladat.Services;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    [QueryProperty(nameof(FuelRecordId), "id")]
    public partial class AddFuelViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private int _fuelRecordId;

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

        public AddFuelViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

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
            var record = await _databaseService.GetFuelRecordAsync(id);
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

                var vehicles = await _databaseService.GetVehiclesAsync();
                if (vehicles.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Hiba", "Nincs jármû megadva!", "OK");
                    return;
                }

                var fuelRecord = new FuelRecord
                {
                    Id = FuelRecordId,
                    VehicleId = vehicles[0].Id, // Elsõ jármû használata
                    Date = Date,
                    Distance = double.Parse(Distance),
                    FuelAmount = double.Parse(FuelAmount),
                    PricePerLitre = double.Parse(PricePerLitre),
                    ReceiptPhotoPath = ReceiptPhotoPath,
                    Latitude = Latitude,
                    Longitude = Longitude
                };

                await _databaseService.SaveFuelRecordAsync(fuelRecord);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", $"Mentés sikertelen: {ex.Message}", "OK");
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
                        var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                        using (var stream = await photo.OpenReadAsync())
                        using (var newStream = File.OpenWrite(newFile))
                        {
                            await stream.CopyToAsync(newStream);
                        }
                        ReceiptPhotoPath = newFile;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", $"Fotó készítése sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task GetLocationAsync()
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    await Shell.Current.DisplayAlert("Sikeres", 
                        $"Hely mentve: {Latitude:F6}, {Longitude:F6}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", $"Helymeghatározás sikertelen: {ex.Message}", "OK");
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
                Shell.Current.DisplayAlert("Hiba", "Kérlek adj meg érvényes kilométert!", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(FuelAmount) || !double.TryParse(FuelAmount, out _))
            {
                Shell.Current.DisplayAlert("Hiba", "Kérlek adj meg érvényes üzemanyag mennyiséget!", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PricePerLitre) || !double.TryParse(PricePerLitre, out _))
            {
                Shell.Current.DisplayAlert("Hiba", "Kérlek adj meg érvényes árat!", "OK");
                return false;
            }

            return true;
        }
    }
}
