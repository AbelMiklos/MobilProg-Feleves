using System.ComponentModel;

namespace GMYEL8.FelevesFeladat.Domain.Enums;

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
