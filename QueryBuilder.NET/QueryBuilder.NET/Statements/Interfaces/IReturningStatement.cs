namespace QueryBuilderDotNet.Statements.Interfaces;

/// <summary>
///     Base interface for statements that can have a <i>RETURNING</i> clause 
/// </summary>
/// <typeparam name="T">Type of the child interface</typeparam>
public interface IReturningStatement<out T> where T : IQueryBuilderStatement
{
    /// <summary>
    /// Adds provided columns to RETURNING clause
    /// </summary>
    /// <param name="columns">Columns to add</param>
    /// <returns>Enriched statement</returns>
    public T Returning(params string[] columns);
    
    /// <summary>
    /// Adds ID columns to RETURNING clause
    /// </summary>
    /// <param name="idColumnName">Custom name of the ID column. Default value "Id" is used if not provided.</param>
    /// <returns>Enriched statement</returns>
    public T ReturningId(string? idColumnName = null);
}