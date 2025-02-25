namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IDeleteStatement : IFilterableStatement
{
}

public interface IDeleteStatement<TEntity> : IFilterableStatement<TEntity> {}