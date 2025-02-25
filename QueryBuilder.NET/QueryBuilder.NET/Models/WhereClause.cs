namespace QueryBuilderDotNet.Models;

public struct WhereClause
{
    public WhereClause(string columnName, object? value, LogicalOperators logicalOperator = LogicalOperators.And)
    {
        Column = columnName;
        Value = value;
        LogicalOperator = logicalOperator;
    }

    public string Column { get; set; }
    public LogicalOperators LogicalOperator { get; set; } = LogicalOperators.And;
    public object? Value { get; set; }
}