using GMYEL8.FelevesFeladat.Domain.Enums;
using SQLite;

namespace GMYEL8.FelevesFeladat.Domain.Entities;

[Table("Expenses")]
public class Expense
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int VehicleId { get; set; }

    [MaxLength(100)]
    public ExpenseType Type { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public double Cost { get; set; }

    public DateTime Date { get; set; }
}
