using QueryBuilderDotNet.Utils;

namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IInsertIntoStatement : IQueryBuilderStatement, IReturningStatement<IInsertIntoStatement>
{
    public bool IsInsertingIdColumn { get; }
    public IInsertIntoStatement InsertingId(string idColumnName = QueryBuilderDefaults.IdColumnName);
}