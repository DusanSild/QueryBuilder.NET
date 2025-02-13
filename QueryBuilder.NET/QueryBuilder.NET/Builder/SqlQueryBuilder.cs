using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using QueryBuilder.NET.Statements;
using QueryBuilder.NET.Statements.Interfaces;

namespace QueryBuilder.NET.Builder;

public sealed class SqlQueryBuilder
{
    private readonly List<string> _selectList = [];
    private string _fromTable;
    private readonly List<string> _whereList = [];
    private bool _usePaging;

    /// <summary>
    ///     Creates an inserting SQL query. Table name is taken from the model class' <see cref="TableAttribute" />
    /// </summary>
    /// <param name="value">Mode class instance to generate insert query for</param>
    /// <typeparam name="T">Type of model to insert</typeparam>
    /// <returns>An instance of <see cref="IInsertIntoStatement" /></returns>
    public static IInsertIntoStatement Insert<T>(T value)
    {
        return new InsertIntoStatement<T>(value);
    }

    /// <summary>
    ///     Creates an inserting SQL query into the given table.
    /// </summary>
    /// <param name="value">Mode class instance to generate insert query for</param>
    /// <param name="tableName">Table name to insert into</param>
    /// <typeparam name="T">Type of model to insert</typeparam>
    /// <returns>An instance of <see cref="IInsertIntoStatement" /></returns>
    public static IInsertIntoStatement InsertInto<T>(T value, string tableName)
    {
        return new InsertIntoStatement<T>(value, tableName);
    }

    public static IDeleteStatement<T> Delete<T>()
    {
        return new DeleteStatement<T>();
    }

    public static IDeleteStatement DeleteFrom(string tableName)
    {
        return new DeleteStatement(tableName);
    }

    public SqlQueryBuilder SelectAll(string prefix = "")
    {
        _selectList.Add(string.IsNullOrWhiteSpace(prefix) ? $"{prefix}.*" : "*");
        return this;
    }

    public SqlQueryBuilder Select(string prefix, string columnName, string alias)
    {
        _selectList.Add($"{prefix}.\"{columnName}\" AS \"{alias}\"");
        return this;
    }

    public SqlQueryBuilder Select(string prefix, string columnName)
    {
        _selectList.Add($"{prefix}.\"{columnName}\"");
        return this;
    }

    public SqlQueryBuilder Select(string columnName)
    {
        _selectList.Add($"\"{columnName}\"");
        return this;
    }

    public SqlQueryBuilder From(string table, string alias)
    {
        _fromTable = $"FROM \"{table}\" AS \"{alias}\"";
        return this;
    }

    public SqlQueryBuilder From(string table)
    {
        _fromTable = $"FROM \"{table}\"";
        return this;
    }

    public SqlQueryBuilder Where(string condition)
    {
        _whereList.Add(condition);
        return this;
    }

    public SqlQueryBuilder WithPaging()
    {
        _usePaging = true;
        return this;
    }

    public string Build()
    {
        StringBuilder sb = new();
        sb.Append("SELECT ");
        sb.AppendLine(string.Join($",{Environment.NewLine}", _selectList));
        sb.AppendLine(_fromTable);

        if (_whereList.Count > 0)
        {
            sb.Append("WHERE ");
            sb.AppendLine(string.Join(" AND ", _whereList));
        }

        if (_usePaging)
        {
            sb.AppendLine("OFFSET @offset LIMIT @count");
        }

        return sb.ToString();
    }

    //TODO: REMOVE LATER
    // public static void Test()
    // {
    //     var sqlQueryBuilder = new SqlQueryBuilder()
    //         .SelectAll("ct")
    //         .From("ContractType", "ct")
    //         .Where("ct.Name ILIKE 'test%'")
    //         .WithPaging();
    //
    //     var sqlQuery = sqlQueryBuilder.Build();
    // }
}