using GarageLog.Domain.Enums;
using SQLite;

namespace GarageLog.Domain.Entities;

[Table("Vehicles")]
public class Vehicle
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public VehicleType Type { get; set; }

    [MaxLength(20)]
    public string LicensePlate { get; set; } = string.Empty;

    public int Year { get; set; }
}
