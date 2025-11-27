using SQLite;

namespace GMYEL8.FelevesFeladat.Models
{
    [Table("Expenses")]
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int VehicleId { get; set; }

        [MaxLength(100)]
        public string Type { get; set; } = string.Empty; // Maintenance, Service, Repair, Insurance, etc.

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public double Cost { get; set; }

        public DateTime Date { get; set; }
    }
}
