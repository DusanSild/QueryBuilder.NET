namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IReturningStatement<out T> where T : IQueryBuilderStatement
{
    public T Returning(params string[] columns);
    public T ReturningId(string? idColumnName = null);
}