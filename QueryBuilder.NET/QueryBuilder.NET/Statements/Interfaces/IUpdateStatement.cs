using System.Linq.Expressions;

namespace QueryBuilderDotNet.Statements.Interfaces;

public interface IUpdateStatement<T> : IFilterableStatement, IReturningStatement<IUpdateStatement<T>>
{
    public IUpdateStatement<T> Updating(Expression<Func<T, object>> selector);
}