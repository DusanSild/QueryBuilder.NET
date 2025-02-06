using Dapper;

namespace QueryBuilder.NET.Models;

public struct DapperQuery
{
    public string CommandText { get; set; }
    public DynamicParameters Parameters { get; set; }
}