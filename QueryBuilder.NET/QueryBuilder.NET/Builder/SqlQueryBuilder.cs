using System.ComponentModel.DataAnnotations.Schema;
using QueryBuilderDotNet.Statements;
using QueryBuilderDotNet.Statements.Interfaces;

namespace QueryBuilderDotNet.Builder;

public static class SqlQueryBuilder
{
    /// <summary>
    ///     Creates an inserting SQL query. Table name is taken from the model class' <see cref="TableAttribute" />
    /// </summary>
    /// <param name="value">Mode class instance to generate insert query for</param>
    /// <typeparam name="T">Type of model to insert</typeparam>
    /// <returns>An instance of <see cref="IInsertIntoStatement" /></returns>
    public static IInsertIntoStatement Insert<T>(T value)
    {
        return new InsertIntoStatement<T>(value);
    }

    /// <summary>
    ///     Creates an inserting SQL query into the given table.
    /// </summary>
    /// <param name="value">Mode class instance to generate insert query for</param>
    /// <param name="tableName">Table name to insert into</param>
    /// <typeparam name="T">Type of model to insert</typeparam>
    /// <returns>An instance of <see cref="IInsertIntoStatement" /></returns>
    public static IInsertIntoStatement InsertInto<T>(T value, string tableName)
    {
        return new InsertIntoStatement<T>(value, tableName);
    }

    /// <summary>
    ///     Creates deleting SQL query. Table name is taken from the model class' <see cref="TableAttribute" />
    /// </summary>
    /// <typeparam name="T">Type of model to generate query for</typeparam>
    /// <returns>An instance of <see cref="IDeleteStatement{TEntity}" /></returns>
    public static IDeleteStatement<T> Delete<T>()
    {
        return new DeleteStatement<T>();
    }

    /// <summary>
    ///     Creates deleting SQL query for given table name.
    /// </summary>
    /// <returns>An instance of <see cref="IDeleteStatement" /></returns>
    public static IDeleteStatement DeleteFrom(string tableName)
    {
        return new DeleteStatement(tableName);
    }

    /// <summary>
    ///     Creates updating SQL query. Table name is taken from the model class' <see cref="TableAttribute" />
    /// </summary>
    /// <param name="value">Mode class instance to generate insert query for</param>
    /// <typeparam name="T">Type of model to generate query for</typeparam>
    /// <returns>An instance of <see cref="IUpdateStatement{TEntity}" /> </returns>
    public static IUpdateStatement<T> Update<T>(T value)
    {
        return new UpdateStatement<T>(value);
    }

    /// <summary>
    ///     Creates updating SQL query.
    /// </summary>
    /// <param name="value">Mode class instance to generate insert query for</param>
    /// <param name="tableName">Table name to insert into</param>
    /// <typeparam name="T">Type of model to generate query for</typeparam>
    /// <returns>An instance of <see cref="IUpdateStatement{TEntity}" /> </returns>
    public static IUpdateStatement<T> Update<T>(T value, string tableName)
    {
        return new UpdateStatement<T>(value, tableName);
    }
}