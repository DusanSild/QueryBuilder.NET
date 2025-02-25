using QueryBuilderDotNet.Builder;
using QueryBuilderDotNet.Extensions;
using QueryBuilderDotNet.Utils;
using QueryBuilderTests.Models;

namespace QueryBuilderTests;

public class DeleteStatementTests
{
    private ModelWithTableAttribute _modelWithTableAttribute;
    private ModelWithoutTableAttribute _modelWithoutTableAttribute;
    
    [SetUp]
    public void Setup()
    {
        _modelWithTableAttribute = new ModelWithTableAttribute
        {
            Id = Guid.Empty,
            Age = 45,
            Email = "test@test.com",
            Salary = 24590,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1980, 1, 24, 0,0,0,DateTimeKind.Utc)
        };

        _modelWithoutTableAttribute = new ModelWithoutTableAttribute
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
        var deleteStatement = SqlQueryBuilder.Delete<ModelWithTableAttribute>()
            .Where(model => model.Id, _modelWithTableAttribute.Id);
        
        // assert statement is created
        Assert.That(deleteStatement, Is.Not.Null);
        
        // assert that table is taken from the Table attribute
        Assert.That(deleteStatement.TableName, Is.EqualTo(ModelWithTableAttribute.TableName));
        
        // assert that ID column name is default
        Assert.That(deleteStatement.IdColumnName, Is.EqualTo(QueryBuilderDefaults.IdColumnName));

        // build query
        var dapperQuery = deleteStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"DELETE FROM ""{ModelWithTableAttribute.TableName}""", dapperQuery.CommandText);
        
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        //check the first parameter
        Assert.That(dapperQuery.Parameters.Get<Guid>("@p0"), Is.EqualTo(_modelWithTableAttribute.Id));
    }

    [Test]
    public void DeleteModelWithoutAttribute()
    {
        var tableName = "TestTable";
        
        var deleteStatement = SqlQueryBuilder.DeleteFrom(tableName)
            .Where<ModelWithoutTableAttribute, long>(model => model.Id, _modelWithoutTableAttribute.Id);
        
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
        Assert.That(dapperQuery.Parameters.Get<long>("@p0"), Is.EqualTo(_modelWithoutTableAttribute.Id));
    }
}