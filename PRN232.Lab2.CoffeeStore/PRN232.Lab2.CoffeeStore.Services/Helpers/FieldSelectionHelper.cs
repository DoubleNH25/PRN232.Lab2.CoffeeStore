using System.Reflection;

namespace PRN232.Lab2.CoffeeStore.Services.Helpers;

public static class FieldSelectionHelper
{
    public static IDictionary<string, object?> SelectFields<T>(T source, string? select)
    {
        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        if (source == null)
        {
            return result;
        }

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (string.IsNullOrWhiteSpace(select))
        {
            foreach (var property in properties)
            {
                result[property.Name] = property.GetValue(source);
            }
            return result;
        }

        var selectedFields = select
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        foreach (var field in selectedFields)
        {
            var property = properties.FirstOrDefault(p => string.Equals(p.Name, field, StringComparison.OrdinalIgnoreCase));
            if (property == null)
            {
                continue;
            }
            result[property.Name] = property.GetValue(source);
        }

        return result;
    }

    public static IEnumerable<IDictionary<string, object?>> SelectFields<T>(IEnumerable<T> source, string? select)
    {
        foreach (var item in source)
        {
            yield return SelectFields(item, select);
        }
    }
}
