using QueryBuilder.NET.Utils;

namespace QueryBuilder.NET.Statements.Interfaces;

public interface IInsertIntoStatement : IQueryBuilderStatement
{
    public bool InsertIdColumn { get; }
    public string IdColumnName { get; }
    public IInsertIntoStatement Returning(params string[] columns);
    public IInsertIntoStatement ReturningId();
    public IInsertIntoStatement InsertingId(string idColumnName = QueryBuilderDefaults.IdColumnName);
}