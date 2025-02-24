using System.Linq.Expressions;
using QueryBuilder.NET.Models;
using QueryBuilder.NET.Statements.Interfaces;
using QueryBuilder.NET.Utils;

namespace QueryBuilder.NET.Extensions;

public static class FilterableStatementExtensions
{
    public static IFilterableStatement Where<TEntity, TProperty>(this IFilterableStatement statement, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
    {
        var columnName = NamingHelper.GetColumnName(propertySelector);
        statement.WhereExpressions.Add(new WhereClause(columnName, value));
        return statement;
    }
    
    public static IFilterableStatement<TEntity> Where<TProperty, TEntity>(this IFilterableStatement<TEntity> statement, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
    {
        var columnName = NamingHelper.GetColumnName(propertySelector);
        statement.WhereExpressions.Add(new WhereClause(columnName, value));
        return statement;
    }

    public static IFilterableStatement OrWhere(this IFilterableStatement statement, string columnName, object value)
    {
        statement.WhereExpressions.Add(new WhereClause(columnName, value, LogicalOperators.Or));
        return statement;
    }
    
    public static IFilterableStatement OrWhere<TModel, TProperty>(this IFilterableStatement statement, Expression<Func<TModel, TProperty>> propertySelector, TProperty value)
    {
        var columnName = NamingHelper.GetColumnName(propertySelector);
        statement.WhereExpressions.Add(new WhereClause(columnName, value, LogicalOperators.Or));
        return statement;
    }
}