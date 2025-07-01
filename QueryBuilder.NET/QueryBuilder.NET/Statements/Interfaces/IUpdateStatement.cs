using System.Linq.Expressions;

namespace QueryBuilderDotNet.Statements.Interfaces;

/// <summary>
///     Base interface for a class that represents an <i>UPDATE</i> statement
/// </summary>
/// <typeparam name="TEntity">Type of the entity to use for the statement</typeparam>
public interface IUpdateStatement<TEntity> : IFilterableStatement, IReturningStatement<IUpdateStatement<TEntity>>
{
    /// <summary>
    ///     Adds selected columns to update statement. If not used, all columns are updated.
    /// </summary>
    /// <param name="selector">Selector that allows selecting properties that should be updated</param>
    /// <returns>Enriched statement</returns>
    public IUpdateStatement<TEntity> Updating(Expression<Func<TEntity, object>> selector);
}