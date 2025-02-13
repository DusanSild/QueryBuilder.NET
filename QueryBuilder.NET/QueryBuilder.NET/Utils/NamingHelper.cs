using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Dapper.ColumnMapper;

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

    internal static string FormatSqlName(string entityName)
    {
        return $"\"{entityName}\"";
    }

    internal static string GetColumnName<TModel, TProperty>(Expression<Func<TModel, TProperty>> propertySelector)
    {
        if (propertySelector == null)
        {
            throw new ArgumentNullException(nameof(propertySelector));
        }

        MemberExpression? memberExp = null;

        // The property might be wrapped in a conversion if it's a value type.
        if (propertySelector.Body.NodeType == ExpressionType.Convert)
        {
            var unaryExp = propertySelector.Body as UnaryExpression;
            memberExp = unaryExp?.Operand as MemberExpression;
        }
        else if (propertySelector.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExp = propertySelector.Body as MemberExpression;
        }

        if (memberExp == null)
        {
            throw new ArgumentException("The expression is not a valid property expression.", nameof(propertySelector));
        }

        var columnMappingAttribute = memberExp.Member.GetCustomAttribute<ColumnMappingAttribute>();
        if (columnMappingAttribute != null)
        {
            return columnMappingAttribute.ColumnName;
        }
        
        var columnAttribute = memberExp.Member.GetCustomAttribute<ColumnAttribute>();
        return columnAttribute?.Name ?? memberExp.Member.Name;
    }
}