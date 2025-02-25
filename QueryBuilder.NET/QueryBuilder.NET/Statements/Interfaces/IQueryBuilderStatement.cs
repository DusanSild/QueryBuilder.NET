using QueryBuilderDotNet.Models;

namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IQueryBuilderStatement
{
    public string TableName { get; }
    
    string IdColumnName { get; }

    public DapperQuery BuildQuery();
}