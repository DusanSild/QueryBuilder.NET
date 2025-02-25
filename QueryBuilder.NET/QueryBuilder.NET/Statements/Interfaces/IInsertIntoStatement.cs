using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IInsertIntoStatement : IQueryBuilderStatement
{
    public bool InsertIdColumn { get; }
    public IInsertIntoStatement Returning(params string[] columns);
    public IInsertIntoStatement ReturningId();
    public IInsertIntoStatement InsertingId(string idColumnName = QueryBuilderDefaults.IdColumnName);
}