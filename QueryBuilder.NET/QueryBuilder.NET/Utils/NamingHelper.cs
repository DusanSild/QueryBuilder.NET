using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace QueryBuilder.NET.Utils;

internal static class NamingHelper
{
    internal static string GetTableName(Type entityType)
    {
        if (entityType.GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault() is not TableAttribute tableAttribute)
        {
            throw new ArgumentException("Model type is missing Table attribute!", nameof(entityType));
        }

        return tableAttribute.Name;
    }
    
    internal static string CreateParamName(string propertyName)
    {
        return $"@{propertyName.ToCamelCase()}";
    }
}