using QueryBuilderDotNet.Models;

namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IFilterableStatement : IQueryBuilderStatement
{
    protected internal List<WhereClause> WhereExpressions { get; }
}

public interface IFilterableStatement<TEntity> : IFilterableStatement {}