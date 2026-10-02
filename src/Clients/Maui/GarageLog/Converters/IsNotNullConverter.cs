using System.Globalization;

namespace GarageLog.Converters
{
    public class IsNotNullConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isNotNull = value != null;

            // If the parameter is "Invert", invert the result
            return parameter is string param && param.Equals("Invert", StringComparison.OrdinalIgnoreCase) 
                ? !isNotNull 
                : isNotNull;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
