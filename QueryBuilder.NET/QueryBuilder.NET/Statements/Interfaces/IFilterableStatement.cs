using System.Collections.Specialized;

namespace QueryBuilder.NET.Statements.Interfaces;

public interface IFilterableStatement
{
    protected OrderedDictionary WhereExpressions { get; set; }
    
    public IFilterableStatement Where(string columnName, object value);
    
    public IFilterableStatement AndWhere(string columnName, object value);
    
    public IFilterableStatement OrWhere(string columnName, object value);
}