using System.Text;
using Dapper;
using QueryBuilderDotNet.Models;
using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Extensions;

internal static class StringBuilderExtensions
{
    internal static StringBuilder AppendWhereClauses(this StringBuilder builder, List<WhereClause> source, DynamicParameters dynamicParameters)
    {
        if (source.Count < 1)
        {
            return builder;
        }

        bool first = true;
        int index = 0;
        foreach (var expression in source)
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
        
        return builder;
    }
}