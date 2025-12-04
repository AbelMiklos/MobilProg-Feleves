using GMYEL8.FelevesFeladat.Domain.Entities;
using GMYEL8.FelevesFeladat.Domain.Enums;
using GMYEL8.FelevesFeladat.Shared.Services;

namespace GMYEL8.FelevesFeladat.Infrastructure.Helpers;

public static class SeedDataHelper
{
    public static async Task SeedDataAsync(IDatabaseService databaseService)
    {
        // Ensure DB is initialized
        await databaseService.Init();

        // Check if already seeded
        var vehicleCount = await databaseService.Connection.Table<Vehicle>().CountAsync();
        if (vehicleCount > 0)
            return; // Already seeded

        // Add sample vehicle
        var vehicle = new Vehicle
        {
            Name = "Saját autó",
            Type = VehicleType.Car,
            LicensePlate = "ABC-123",
            Year = 2020
        };
        await databaseService.Connection.InsertAsync(vehicle);

        // vehicle.Id is set after insert due to AutoIncrement
        var vehicleId = vehicle.Id;

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
            await databaseService.Connection.InsertAsync(record);
        }

        // Add sample expenses
        var expenses = new List<Expense>
        {
            new Expense
            {
                VehicleId = vehicleId,
                Type = ExpenseType.Maintenance,
                Description = "Olajcsere és szűrőcsere",
                Cost = 25000,
                Date = DateTime.Now.AddDays(-20)
            },
            new Expense
            {
                VehicleId = vehicleId,
                Type = ExpenseType.Insurance,
                Description = "Éves kötelező biztosítás",
                Cost = 45000,
                Date = DateTime.Now.AddDays(-60)
            }
        };

        foreach (var expense in expenses)
        {
            await databaseService.Connection.InsertAsync(expense);
        }
    }
}

