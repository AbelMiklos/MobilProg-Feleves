using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace GMYEL8.FelevesFeladat.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (!value.GetType().IsEnum)
                return value.ToString();

            var fieldInfo = value.GetType().GetField(value.ToString()!);
            if (fieldInfo == null)
                return value.ToString();

            var attributes = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            
            return attributes?.Description ?? value.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || !targetType.IsEnum)
                return null;

            foreach (var field in targetType.GetFields())
            {
                if (field.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute attribute)
                {
                    if (attribute.Description == value.ToString())
                        return field.GetValue(null);
                }
                else if (field.Name == value.ToString())
                {
                    return field.GetValue(null);
                }
            }

            return null;
        }
    }
}