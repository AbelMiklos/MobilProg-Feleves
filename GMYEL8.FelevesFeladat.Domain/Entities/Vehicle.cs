using GMYEL8.FelevesFeladat.Domain.Enums;
using SQLite;

namespace GMYEL8.FelevesFeladat.Domain.Entities;

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
