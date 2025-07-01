using System.Text;
using Dapper;
using QueryBuilderDotNet.Extensions;
using QueryBuilderDotNet.Models;
using QueryBuilderDotNet.Statements.Interfaces;
using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Statements;

public sealed class DeleteStatement : DeleteStatementBase
{
    public DeleteStatement(string tableName)
    {
        TableName = tableName;
    }
}

public sealed class DeleteStatement<TEntity> : DeleteStatementBase, IDeleteStatement<TEntity>
{
    public DeleteStatement()
    {
        TableName = NamingHelper.GetTableName(typeof(TEntity));
    }
}

public abstract class DeleteStatementBase : IDeleteStatement
{
    public string TableName { get; protected set; } = "";
    
    public string IdColumnName { get; protected set;  } = QueryBuilderDefaults.IdColumnName;
    
    public List<WhereClause> WhereExpressions { get; protected set; } = new();
    
    public DapperQuery BuildQuery()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"""DELETE FROM {NamingHelper.FormatSqlName(TableName)} """);

        var dynamicParameters = new DynamicParameters();
        builder.AppendWhereClauses(WhereExpressions, dynamicParameters);

        builder.Append(';');
        
        return new DapperQuery
        {
            CommandText = builder.ToString(),
            Parameters = dynamicParameters
        };
    }
}