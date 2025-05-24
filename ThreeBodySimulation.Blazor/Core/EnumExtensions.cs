using System.ComponentModel.DataAnnotations;

namespace ThreeBodySimulation.Blazor.Core;

public static class EnumExtensions
{
    public static string ToHumanReadableString(this Enum enumValue)
    {
        DisplayAttribute? displayAttribute = enumValue.GetType()
            .GetField(enumValue.ToString())!
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .SingleOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? enumValue.ToString();
    }
}
