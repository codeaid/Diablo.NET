using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Diablo4.Shared.Extensions;

public static class EnumDescriptionExtension
{
    // Use ConcurrentDictionary instead of MemoryCache as we never need to expire the cache.
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    /// <summary>
    /// Retrieve display name of an enum member if available or fall back to the default string representation of it.
    /// Found <a href="https://josipmisko.com/posts/string-enums-in-c-sharp-everything-you-need-to-know">here</a>.
    /// </summary>
    /// <param name="member">Target member whose display name to retrieve.</param>
    /// <returns>Display name of the member or its default string representation.</returns>
    public static string ToDescription(this Enum member)
    {
        // Retrieve type information of the current enumeration.
        Type type = member.GetType();

        // Convert the enumeration value to a string;
        var value = member.ToString();

        // Build cache key from the enum name and the member value.
        var key = $"{type.FullName}::{value}";

        // Retrieve the existing one or detect and store the value of the DescriptionAttribute of the member.
        return Cache.GetOrAdd(key, _ =>
        {
            // Retrieve field information for the current member.
            FieldInfo? field = type.GetTypeInfo().GetField(value);
            if (field == null)
            {
                return value;
            }

            // Retrieve the description attribute defined on the member.
            DescriptionAttribute? description = field
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .OfType<DescriptionAttribute>()
                .FirstOrDefault();

            // Use description value or fall back to the default string representation of the enum member.
            return description == null ? value : description.Description;
        });
    }
}
