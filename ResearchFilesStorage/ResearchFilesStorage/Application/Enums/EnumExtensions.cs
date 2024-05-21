using System.ComponentModel;

namespace ResearchFilesStorage.Application.Enums;

public static class EnumExtensions
{
    public static string? GetDescription<T>(this T enumerationValue)
        where T : struct
    {
        var type = enumerationValue.GetType();

        if (!type.IsEnum)
        {
            throw new ArgumentException("EnumerationValue must be of type Enum");
        }

        var memberInfo = type.GetMember(enumerationValue.ToString() ?? throw new InvalidOperationException());

        if (memberInfo.Length > 0)
        {
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return ((DescriptionAttribute)attributes[0]).Description;
            }
        }

        return enumerationValue.ToString();
    }
}