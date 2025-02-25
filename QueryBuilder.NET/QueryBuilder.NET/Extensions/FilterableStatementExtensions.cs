using System.Linq.Expressions;
using QueryBuilderDotNet.Models;
using QueryBuilderDotNet.Statements.Interfaces;
using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Extensions;

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