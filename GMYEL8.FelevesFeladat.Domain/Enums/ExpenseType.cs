using System.ComponentModel;

namespace GMYEL8.FelevesFeladat.Domain.Enums;

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
