using System.ComponentModel;

namespace GarageLog.Domain.Enums;

public enum ExpenseType
{
    [Description("Karbantartás")]
    Maintenance,

    [Description("Javítás")]
    Repair,

    [Description("Biztosítás")]
    Insurance,

    [Description("Adó")]
    Tax,

    [Description("Egyéb")]
    Other
}
