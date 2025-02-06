using System.Collections.Specialized;
using System.Text;
using QueryBuilder.NET.Models;
using QueryBuilder.NET.Statements.Interfaces;
using QueryBuilder.NET.Utils;

namespace QueryBuilder.NET.Statements;

public class DeleteStatement(string tableName) : IDeleteStatement, IFilterableStatement
{
    public string TableName { get; } = tableName;
    
    public string IdColumnName { get; } = QueryBuilderDefaults.IdColumnName;
    public DapperQuery BuildQuery()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"""DELETE FROM "{TableName}" """);
        

        return new DapperQuery
        {
            CommandText = builder.ToString()
        };
    }

    public OrderedDictionary WhereExpressions { get; set; } = new OrderedDictionary();
    
    public IFilterableStatement Where(string columnName, object value)
    {
        WhereExpressions.Add(columnName, value);
        throw new NotImplementedException();
    }

    public IFilterableStatement AndWhere(string columnName, object value)
    {
        throw new NotImplementedException();
    }

    public IFilterableStatement OrWhere(string columnName, object value)
    {
        throw new NotImplementedException();
    }
}

public class DeleteStatement<TEntity>(string tableName = "") : IDeleteStatement
{
    public string TableName { get; } = string.IsNullOrWhiteSpace(tableName) ? NamingHelper.GetTableName(typeof(TEntity)) : tableName;

    public string IdColumnName { get; } = QueryBuilderDefaults.IdColumnName;
    public DapperQuery BuildQuery()
    {
        throw new NotImplementedException();
    }
}