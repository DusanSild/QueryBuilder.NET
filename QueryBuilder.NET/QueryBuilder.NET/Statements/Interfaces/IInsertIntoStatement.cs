using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IInsertIntoStatement : IQueryBuilderStatement
{
    public bool IsInsertingIdColumn { get; }
    public IInsertIntoStatement Returning(params string[] columns);
    public IInsertIntoStatement ReturningId(string? idColumnName = null);
    public IInsertIntoStatement InsertingId(string idColumnName = QueryBuilderDefaults.IdColumnName);
}