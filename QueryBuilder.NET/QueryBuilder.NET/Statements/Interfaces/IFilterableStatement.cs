using QueryBuilder.NET.Models;

namespace QueryBuilder.NET.Statements.Interfaces;

public interface IFilterableStatement : IQueryBuilderStatement
{
    protected internal List<WhereClause> WhereExpressions { get; }
}

public interface IFilterableStatement<TEntity> : IFilterableStatement {}