using System;
using System.Reflection;

namespace Continental.API.Core.Attributes;

public class StringValueAttribute : Attribute
{
    public string Value { get; protected set; }

    public StringValueAttribute(string value)
    {
        Value = value;
    }
}

public static class StringValueAttributeExtensions
{
    public static string GetStringValue(this Enum value)
    {
        var type = value.GetType();

        var fieldInfo = type.GetField(value.ToString());

        var attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

        return attributes.Length > 0 ? attributes[0].Value : null;
    }

    public static Enum GetEnumBaseValue<T>(this string value) where T : struct, Enum
    {
        var members = typeof(T)
            .GetTypeInfo()
            .DeclaredMembers;

        foreach (var member in members)
        {
            var attribute = member.GetCustomAttribute<StringValueAttribute>(false);
            if (attribute is null)
            {
                continue;
            }

            if (attribute.Value.Equals(value))
            {
                return (T) Enum.Parse(typeof(T), member.Name);
            }
        }

        return default;
    }
}