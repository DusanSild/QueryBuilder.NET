using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using Dapper;
using Dapper.ColumnMapper;
using QueryBuilderDotNet.Models;
using QueryBuilderDotNet.Statements.Interfaces;
using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Statements;

public sealed class InsertIntoStatement<T>(T value, string tableName = "") : IInsertIntoStatement
{
    public string TableName { get; } = string.IsNullOrWhiteSpace(tableName) ? NamingHelper.GetTableName(typeof(T)) : tableName;
    public bool InsertIdColumn { get; private set; }
    public string IdColumnName { get; private set; } = QueryBuilderDefaults.IdColumnName;

    private readonly List<string> _returningColumns = new();

    public DapperQuery BuildQuery()
    {
        var builder = new StringBuilder();
        builder.Append($"""INSERT INTO "{TableName}" """);

        var propertyList = FilterProperties(typeof(T));

        var columns = propertyList.Select(ResolveColumnName).ToArray();
        builder.AppendLine($"( {string.Join(", ", columns)} )");

        var dynamicParameters = new DynamicParameters();

        builder.Append("VALUES ( ");
        for (var propertyIdx = 0; propertyIdx < propertyList.Count; propertyIdx++)
        {
            var property = propertyList[propertyIdx];
            var paramName = NamingHelper.CreateParamName(property.Name);
            builder.Append(paramName);
            if (propertyIdx != propertyList.Count - 1)
            {
                builder.Append(", ");
            }
            dynamicParameters.Add(paramName, property.GetValue(value));
        }

        builder.Append(')');

        if (_returningColumns.Count != 0)
        {
            builder.Append($"\nRETURNING {string.Join(", ", _returningColumns)}");
        }

        builder.Append(';');

        return new DapperQuery
        {
            CommandText = builder.ToString(),
            Parameters = dynamicParameters
        };
    }

    public IInsertIntoStatement Returning(params string[] columns)
    {
        _returningColumns.AddRange(columns.Select(s => $"\"{s}\""));
        return this;
    }

    public IInsertIntoStatement ReturningId()
    {
        _returningColumns.Add($"\"{QueryBuilderDefaults.IdColumnName}\"");
        return this;
    }

    public IInsertIntoStatement InsertingId(string idColumnName = QueryBuilderDefaults.IdColumnName)
    {
        InsertIdColumn = true;
        IdColumnName = idColumnName;
        return this;
    }

    private List<PropertyInfo> FilterProperties(Type modelType)
    {
        List<PropertyInfo> properties = new();
        foreach (var propertyInfo in modelType.GetProperties())
        {
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite ||
                propertyInfo.CustomAttributes.Any(att => att.AttributeType == typeof(NotMappedAttribute)))
            {
                continue;
            }

            if (!InsertIdColumn && (propertyInfo.Name.Equals(IdColumnName, StringComparison.OrdinalIgnoreCase) ||
                                     propertyInfo.CustomAttributes.Any(att => att.AttributeType == typeof(KeyAttribute))))
            {
                continue;
            }

            properties.Add(propertyInfo);
        }

        return properties;
    }

    private static string ResolveColumnName(PropertyInfo propertyInfo)
    {
        string colName = propertyInfo.Name;
        var colAttribute = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute)).ToList();
        if (colAttribute.Count != 0)
        {
            if (colAttribute[0] is ColumnAttribute columnAttribute && !string.IsNullOrWhiteSpace(columnAttribute.Name))
            {
                colName = columnAttribute.Name;
            }
        }
        else
        {
            colAttribute = propertyInfo.GetCustomAttributes(typeof(ColumnMappingAttribute)).ToList();
            if (colAttribute.Count != 0 && colAttribute[0] is ColumnMappingAttribute columnMappingAttribute)
            {
                colName = columnMappingAttribute.ColumnName;
            }
        }
        
        return $"\"{colName}\"";
    }
}