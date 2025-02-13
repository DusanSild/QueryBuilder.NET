using System.Text;
using Dapper;
using QueryBuilder.NET.Models;
using QueryBuilder.NET.Statements.Interfaces;
using QueryBuilder.NET.Utils;

namespace QueryBuilder.NET.Statements;

public class DeleteStatement : DeleteStatementBase
{
    public DeleteStatement(string tableName)
    {
        TableName = tableName;
    }
}

public class DeleteStatement<TEntity> : DeleteStatementBase, IDeleteStatement<TEntity>
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
        if (WhereExpressions.Count > 0)
        {
            bool first = true;
            int index = 0;
            foreach (var expression in WhereExpressions)
            {
                if (first)
                {
                    first = false;
                    builder.Append("WHERE ");
                }
                else
                {
                    builder.Append($"{expression.LogicalOperator.ToString().ToUpper()} ");
                }
                
                var paramName = $"@p{index++}";
                dynamicParameters.Add(paramName, expression.Value);
                builder.Append($"{NamingHelper.FormatSqlName(expression.Column)} = {paramName} ");
            }
        }

        builder.Append(';');
        
        return new DapperQuery
        {
            CommandText = builder.ToString(),
            Parameters = dynamicParameters
        };
    }
}