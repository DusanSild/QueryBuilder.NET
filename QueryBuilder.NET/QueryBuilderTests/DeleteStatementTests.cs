using QueryBuilderDotNet.Builder;
using QueryBuilderDotNet.Extensions;
using QueryBuilderDotNet.Utils;
using QueryBuilderTests.Models;

namespace QueryBuilderTests;

public class DeleteStatementTests
{
    private ModelWithAttributes _modelWithAttributes;
    private ModelWithoutAttributes _modelWithoutAttributes;
    
    [SetUp]
    public void Setup()
    {
        _modelWithAttributes = new ModelWithAttributes
        {
            Id = Guid.Empty,
            Age = 45,
            Email = "test@test.com",
            Salary = 24590,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1980, 1, 24, 0,0,0,DateTimeKind.Utc)
        };

        _modelWithoutAttributes = new ModelWithoutAttributes
        {
            Id = 1,
            Note = "Some useful note",
            Created = DateTime.Now,
            Updated = DateTime.Now
        };
    }
    
    [Test]
    public void DeleteModelWithAttribute()
    {
        var deleteStatement = SqlQueryBuilder.Delete<ModelWithAttributes>()
            .Where(model => model.Id, _modelWithAttributes.Id);
        
        // assert statement is created
        Assert.That(deleteStatement, Is.Not.Null);
        
        // assert that table is taken from the Table attribute
        Assert.That(deleteStatement.TableName, Is.EqualTo(ModelWithAttributes.TableName));
        
        // assert that ID column name is default
        Assert.That(deleteStatement.IdColumnName, Is.EqualTo(QueryBuilderDefaults.IdColumnName));

        // build query
        var dapperQuery = deleteStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"DELETE FROM ""{ModelWithAttributes.TableName}""", dapperQuery.CommandText);
        
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        //check the first parameter
        Assert.That(dapperQuery.Parameters.Get<Guid>("@p0"), Is.EqualTo(_modelWithAttributes.Id));
    }

    [Test]
    public void DeleteModelWithoutAttribute()
    {
        var tableName = "TestTable";
        
        var deleteStatement = SqlQueryBuilder.DeleteFrom(tableName)
            .Where<ModelWithoutAttributes, long>(model => model.Id, _modelWithoutAttributes.Id);
        
        // assert statement is created
        Assert.That(deleteStatement, Is.Not.Null);
        
        // assert that table is taken from the Table attribute
        Assert.That(deleteStatement.TableName, Is.EqualTo(tableName));
        
        // assert that ID column name is default
        Assert.That(deleteStatement.IdColumnName, Is.EqualTo(QueryBuilderDefaults.IdColumnName));

        // build query
        var dapperQuery = deleteStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"DELETE FROM {NamingHelper.FormatSqlName(tableName)}", dapperQuery.CommandText);
        
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        //check the first parameter
        Assert.That(dapperQuery.Parameters.Get<long>("@p0"), Is.EqualTo(_modelWithoutAttributes.Id));
    }
}