using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using QueryBuilderDotNet.Extensions;
using QueryBuilderDotNet.Models;
using QueryBuilderDotNet.Statements.Interfaces;
using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Statements;

public class UpdateStatement<T>(T value, string tableName = "") : IUpdateStatement<T>
{
    public string TableName { get; protected set; } = string.IsNullOrWhiteSpace(tableName) ? NamingHelper.GetTableName(typeof(T)) : tableName;
    public string IdColumnName { get; private set; } = QueryBuilderDefaults.IdColumnName;
    public bool IsReturningIdColumn { get; private set; }
    public bool IsUpdatingAll => _updatingProperties.Count == 0;

    private readonly HashSet<string> _returningColumns = [];
    private readonly HashSet<PropertyInfo> _updatingProperties = [];

    public List<WhereClause> WhereExpressions { get; } = [];

    /// <inheritdoc cref="IQueryBuilderStatement"/>
    public DapperQuery BuildQuery()
    {
        var builder = new StringBuilder();
        builder.Append($"""UPDATE {NamingHelper.FormatSqlName(TableName)} SET ( """);

        var propertyList = FilterProperties(typeof(T));
        var dynamicParameters = new DynamicParameters();

        bool first = true;
        foreach (var property in propertyList)
        {
            if (!first)
            {
                builder.Append(", ");
            }

            first = false;
            var columnName = NamingHelper.GetColumnNameFromMember(property);
            var paramName = NamingHelper.CreateParamName(property.Name);

            builder.Append($"{NamingHelper.FormatSqlName(columnName)} = {paramName}");

            dynamicParameters.Add(paramName, property.GetValue(value));
        }

        builder.Append(") ");
        builder.AppendWhereClauses(WhereExpressions, dynamicParameters);

        if (IsReturningIdColumn)
        {
            _returningColumns.Add(IdColumnName);
        }

        if (_returningColumns.Count != 0)
        {
            builder.Append($"\nRETURNING {string.Join(", ", _returningColumns.Select(NamingHelper.FormatSqlName))}");
        }

        builder.Append(';');

        return new DapperQuery
        {
            CommandText = builder.ToString(),
            Parameters = dynamicParameters
        };
    }

    /// <inheritdoc cref="IReturningStatement{T}"/>
    public IUpdateStatement<T> Returning(params string[] columns)
    {
        _returningColumns.AddRange(columns.Select(NamingHelper.FormatSqlName));
        return this;
    }

    /// <inheritdoc cref="IReturningStatement{T}"/>
    public IUpdateStatement<T> ReturningId(string? idColumnName = null)
    {
        IsReturningIdColumn = true;
        if (!string.IsNullOrWhiteSpace(idColumnName))
        {
            IdColumnName = idColumnName;
        }

        return this;
    }

    /// <inheritdoc cref="IUpdateStatement{TEntity}"/>
    public IUpdateStatement<T> Updating(Expression<Func<T, object>> selector)
    {
        if (selector.Body is NewExpression newExpr)
        {
            foreach (var propertyInfo in newExpr.Arguments.Select(GetMemberFromExpression).OfType<PropertyInfo>())
            {
                _updatingProperties.Add(propertyInfo);
            }
        }
        else
        {
            var member = GetMemberFromExpression(selector.Body);
            if (member is PropertyInfo prop)
            {
                _updatingProperties.Add(prop);
            }
        }

        return this;
    }

    private HashSet<PropertyInfo> FilterProperties(Type modelType)
    {
        if (!IsUpdatingAll)
        {
            return _updatingProperties;
        }

        var modelProperties = modelType.GetProperties();
        HashSet<PropertyInfo> properties = [];
        foreach (var propertyInfo in modelProperties)
        {
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite ||
                propertyInfo.CustomAttributes.Any(att => att.AttributeType == typeof(NotMappedAttribute)))
            {
                continue;
            }

            var foreignKeyProperty = modelProperties.FirstOrDefault(p => p.Name == $"{propertyInfo.Name}Id");
            if (foreignKeyProperty != null && foreignKeyProperty.CustomAttributes.All(att => att.AttributeType != typeof(NotMappedAttribute)))
            {
                continue;
            }

            properties.Add(propertyInfo);
        }

        return properties;
    }

    private static MemberInfo GetMemberFromExpression(Expression expr)
    {
        return expr switch
        {
            MemberExpression memberExpr => memberExpr.Member,
            UnaryExpression { Operand: MemberExpression inner } => inner.Member,
            _ => throw new NotSupportedException("Unsupported expression format.")
        };
    }
}