namespace QueryBuilder.NET.Statements.Interfaces;

public interface IDeleteStatement : IQueryBuilderStatement
{
    public string IdColumnName { get; }
}