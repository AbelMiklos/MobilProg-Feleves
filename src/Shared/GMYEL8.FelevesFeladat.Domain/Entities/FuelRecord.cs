using GMYEL8.FelevesFeladat.Domain.Enums;
using SQLite;

namespace GMYEL8.FelevesFeladat.Domain.Entities;

[Table("FuelRecords")]
public class FuelRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int VehicleId { get; set; }

    public DateTime Date { get; set; }

    public double Distance { get; set; }

    public FuelType Type { get; set; }

    public double FuelAmount { get; set; }

    public double PricePerLitre { get; set; }

    [Ignore]
    public double TotalCost => FuelAmount * PricePerLitre;

    [Ignore]
    public double AverageConsumption => Distance > 0 ? (FuelAmount / Distance) * 100 : 0;

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(500)]
    public string? ReceiptPhotoPath { get; set; }
}
