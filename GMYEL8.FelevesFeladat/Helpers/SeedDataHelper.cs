using GMYEL8.FelevesFeladat.Models;
using GMYEL8.FelevesFeladat.Services;

namespace GMYEL8.FelevesFeladat.Helpers
{
    public static class SeedDataHelper
    {
        public static async Task SeedDataAsync(DatabaseService databaseService)
        {
            var vehicles = await databaseService.GetVehiclesAsync();
            if (vehicles.Count > 0)
                return; // Already seeded

            // Add sample vehicle
            var vehicle = new Vehicle
            {
                Name = "Saját autó",
                Type = "Személyautó",
                LicensePlate = "ABC-123",
                Year = 2020
            };
            await databaseService.SaveVehicleAsync(vehicle);

            // Get the saved vehicle to get its ID
            vehicles = await databaseService.GetVehiclesAsync();
            if (vehicles.Count == 0)
                return;

            var vehicleId = vehicles[0].Id;

            // Add sample fuel records
            var fuelRecords = new List<FuelRecord>
            {
                new FuelRecord
                {
                    VehicleId = vehicleId,
                    Date = DateTime.Now.AddDays(-30),
                    Distance = 450,
                    FuelAmount = 35.5,
                    PricePerLitre = 620
                },
                new FuelRecord
                {
                    VehicleId = vehicleId,
                    Date = DateTime.Now.AddDays(-15),
                    Distance = 380,
                    FuelAmount = 30.2,
                    PricePerLitre = 615
                },
                new FuelRecord
                {
                    VehicleId = vehicleId,
                    Date = DateTime.Now.AddDays(-5),
                    Distance = 420,
                    FuelAmount = 33.8,
                    PricePerLitre = 625
                }
            };

            foreach (var record in fuelRecords)
            {
                await databaseService.SaveFuelRecordAsync(record);
            }

            // Add sample expenses
            var expenses = new List<Expense>
            {
                new Expense
                {
                    VehicleId = vehicleId,
                    Type = "Karbantartás",
                    Description = "Olajcsere és szûrõcsere",
                    Cost = 25000,
                    Date = DateTime.Now.AddDays(-20)
                },
                new Expense
                {
                    VehicleId = vehicleId,
                    Type = "Biztosítás",
                    Description = "Éves kötelezõ biztosítás",
                    Cost = 45000,
                    Date = DateTime.Now.AddDays(-60)
                }
            };

            foreach (var expense in expenses)
            {
                await databaseService.SaveExpenseAsync(expense);
            }
        }
    }
}
