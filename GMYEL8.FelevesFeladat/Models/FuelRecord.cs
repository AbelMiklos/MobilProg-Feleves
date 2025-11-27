using SQLite;

namespace GMYEL8.FelevesFeladat.Models
{
    [Table("FuelRecords")]
    public class FuelRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int VehicleId { get; set; }

        public DateTime Date { get; set; }

        public double Distance { get; set; } // Megtett kilométerek

        public double FuelAmount { get; set; } // Tankolt mennyiség literben

        public double PricePerLitre { get; set; } // Ár literenként

        [Ignore]
        public double TotalCost => FuelAmount * PricePerLitre;

        [Ignore]
        public double AverageConsumption => Distance > 0 ? (FuelAmount / Distance) * 100 : 0;

        // GPS koordináták (extra funkció)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Blokk fotó elérési útja (extra funkció)
        [MaxLength(500)]
        public string? ReceiptPhotoPath { get; set; }
    }
}
