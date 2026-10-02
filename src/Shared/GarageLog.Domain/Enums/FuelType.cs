using System.ComponentModel;

namespace GarageLog.Domain.Enums;

public enum FuelType
{
    [Description("Benzin")]
    Petrol,

    [Description("Dízel")]
    Diesel,

    [Description("LPG")]
    LPG,

    [Description("Egyéb")]
    Other
}
