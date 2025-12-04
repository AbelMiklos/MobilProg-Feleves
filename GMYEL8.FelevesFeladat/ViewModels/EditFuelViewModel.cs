using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Shared.Repositories;

namespace GMYEL8.FelevesFeladat.ViewModels
{
    [QueryProperty(nameof(FuelRecordId), nameof(FuelRecordId))]
    public partial class EditFuelViewModel(IRepository<FuelRecord> fuelRecordRepository) : ObservableObject
    {
        private readonly IRepository<FuelRecord> _fuelRecordRepository = fuelRecordRepository;
        private int _fuelRecordId;

        public string FuelRecordId
        {
            set
            {
                if (int.TryParse(value, out int fuelId))
                {
                    _fuelRecordId = fuelId;
                    LoadFuelRecordAsync();
                }
            }
        }

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
        private string? _locationAddress;

        [ObservableProperty]
        private bool _isLoading;

        private async void LoadFuelRecordAsync()
        {
            if (_fuelRecordId <= 0)
                return;

            try
            {
                IsLoading = true;
                var record = await _fuelRecordRepository.Table
                    .FirstOrDefaultAsync(fuel => fuel.Id == _fuelRecordId);

                if (record != null)
                {
                    Date = record.Date;
                    Distance = record.Distance.ToString();
                    FuelAmount = record.FuelAmount.ToString();
                    PricePerLitre = record.PricePerLitre.ToString();
                    ReceiptPhotoPath = record.ReceiptPhotoPath;
                    Latitude = record.Latitude;
                    Longitude = record.Longitude;

                    // Ha van GPS koordináta, próbáljuk meg lekérdezni a címet
                    if (Latitude.HasValue && Longitude.HasValue)
                    {
                        try
                        {
                            var placemarks = await Geocoding.Default.GetPlacemarksAsync(Latitude.Value, Longitude.Value);
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
                        catch
                        {
                            LocationAddress = $"{Latitude:F6}, {Longitude:F6}";
                        }
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Hiba", "A tankolási rekord nem található!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Tankolási rekord betöltése sikertelen: {ex.Message}", "OK");
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
            if (!ValidateInputs())
                return;

            try
            {
                var record = await _fuelRecordRepository.Table
                    .FirstOrDefaultAsync(fuel => fuel.Id == _fuelRecordId);

                if (record != null)
                {
                    record.Date = Date;
                    record.Distance = double.Parse(Distance);
                    record.FuelAmount = double.Parse(FuelAmount);
                    record.PricePerLitre = double.Parse(PricePerLitre);
                    record.ReceiptPhotoPath = ReceiptPhotoPath;
                    record.Latitude = Latitude;
                    record.Longitude = Longitude;

                    await _fuelRecordRepository.UpdateAsync(record);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Hiba", "A tankolási rekord nem található!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Mentés sikertelen: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Megerõsítés",
                "Biztosan törölni szeretnéd ezt a tankolási rekordot?",
                "Igen",
                "Nem");

            if (!confirm)
                return;

            try
            {
                var record = await _fuelRecordRepository.Table
                    .FirstOrDefaultAsync(fuel => fuel.Id == _fuelRecordId);

                if (record != null)
                {
                    await _fuelRecordRepository.DeleteAsync(record);
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Törlés sikertelen: {ex.Message}", "OK");
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
                await Shell.Current.DisplayAlertAsync("Hiba", $"Helymeghatározás sikertelen: {ex.Message}", "OK");
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
                    var options = new MapLaunchOptions { Name = LocationAddress ?? "Tankolás helyszíne" };
                    await Map.Default.OpenAsync(location, options);
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Hiba", "Nincs mentett helyszín!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Hiba", $"Térkép megnyitása sikertelen: {ex.Message}", "OK");
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