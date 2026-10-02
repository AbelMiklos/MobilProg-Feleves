using System.ComponentModel;

namespace GarageLog.Domain.Enums;

public enum VehicleType
{
    [Description("Személyautó")]
    Car,

    [Description("Motorkerékpár")]
    Motorcycle,

    [Description("Teherautó")]
    Truck,

    [Description("Busz")]
    Bus,

    [Description("Furgon")]
    Van,

    [Description("Egyéb")]
    Other
}
