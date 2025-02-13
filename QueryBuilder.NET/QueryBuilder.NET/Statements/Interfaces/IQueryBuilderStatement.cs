using QueryBuilder.NET.Models;

namespace QueryBuilder.NET.Statements.Interfaces;

public interface IQueryBuilderStatement
{
    public string TableName { get; }
    
    string IdColumnName { get; }

    public DapperQuery BuildQuery();
}