using SQLite;
using GMYEL8.FelevesFeladat.Models;

namespace GMYEL8.FelevesFeladat.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        public DatabaseService()
        {
        }

        private async Task InitAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "VehicleCosts.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<Vehicle>();
            await _database.CreateTableAsync<FuelRecord>();
            await _database.CreateTableAsync<Expense>();
        }

        // Vehicle CRUD
        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            await InitAsync();
            return await _database!.Table<Vehicle>().ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<Vehicle>().Where(v => v.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveVehicleAsync(Vehicle vehicle)
        {
            await InitAsync();
            if (vehicle.Id != 0)
                return await _database!.UpdateAsync(vehicle);
            else
                return await _database!.InsertAsync(vehicle);
        }

        public async Task<int> DeleteVehicleAsync(Vehicle vehicle)
        {
            await InitAsync();
            return await _database!.DeleteAsync(vehicle);
        }

        // FuelRecord CRUD
        public async Task<List<FuelRecord>> GetFuelRecordsAsync(int vehicleId)
        {
            await InitAsync();
            return await _database!.Table<FuelRecord>()
                .Where(f => f.VehicleId == vehicleId)
                .OrderByDescending(f => f.Date)
                .ToListAsync();
        }

        public async Task<FuelRecord?> GetFuelRecordAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<FuelRecord>().Where(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveFuelRecordAsync(FuelRecord fuelRecord)
        {
            await InitAsync();
            if (fuelRecord.Id != 0)
                return await _database!.UpdateAsync(fuelRecord);
            else
                return await _database!.InsertAsync(fuelRecord);
        }

        public async Task<int> DeleteFuelRecordAsync(FuelRecord fuelRecord)
        {
            await InitAsync();
            return await _database!.DeleteAsync(fuelRecord);
        }

        // Expense CRUD
        public async Task<List<Expense>> GetExpensesAsync(int vehicleId)
        {
            await InitAsync();
            return await _database!.Table<Expense>()
                .Where(e => e.VehicleId == vehicleId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<Expense?> GetExpenseAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<Expense>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveExpenseAsync(Expense expense)
        {
            await InitAsync();
            if (expense.Id != 0)
                return await _database!.UpdateAsync(expense);
            else
                return await _database!.InsertAsync(expense);
        }

        public async Task<int> DeleteExpenseAsync(Expense expense)
        {
            await InitAsync();
            return await _database!.DeleteAsync(expense);
        }
    }
}
