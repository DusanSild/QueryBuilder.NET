using QueryBuilder.NET.Builder;
using QueryBuilder.NET.Utils;
using QueryBuilderTests.Models;

namespace QueryBuilderTests;

public class InsertStatementTests
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
    public void TestTableNameWithAttribute()
    {
        var insertStatement = SqlQueryBuilder.Insert(_modelWithTableAttribute);
        
        // assert statement is created
        Assert.That(insertStatement, Is.Not.Null);
        
        // assert that table is taken from the Table attribute
        Assert.That(insertStatement.TableName, Is.EqualTo(ModelWithTableAttribute.TableName));
        
        // assert that ID column name is default
        Assert.That(insertStatement.IdColumnName, Is.EqualTo(QueryBuilderDefaults.IdColumnName));

        // build query
        var dapperQuery = insertStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"INSERT INTO ""{ModelWithTableAttribute.TableName}""", dapperQuery.CommandText);
        
        // assert that query parameters exist
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        // assert that query parameters are correctly filled from instance of model class
        Assert.That(dapperQuery.Parameters.Get<string>("@firstName"), Is.EqualTo(_modelWithTableAttribute.FirstName));
    }
    
    [Test]
    public void TestTableNameWithoutAttribute()
    {
        var tableName = "Notes";
        var insertStatement = SqlQueryBuilder.InsertInto(_modelWithoutTableAttribute, tableName);
        
        // assert statement is created
        Assert.That(insertStatement, Is.Not.Null);
        
        // assert that table is taken from the Table attribute
        Assert.That(insertStatement.TableName, Is.EqualTo(tableName));
        
        // assert that ID column name is default
        Assert.That(insertStatement.IdColumnName, Is.EqualTo(QueryBuilderDefaults.IdColumnName));

        // build query
        var dapperQuery = insertStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"INSERT INTO ""{tableName}""", dapperQuery.CommandText);
        
        // assert that query parameters exist
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        // assert that query parameters are correctly filled from instance of model class
        Assert.That(dapperQuery.Parameters.Get<string>("@note"), Is.EqualTo(_modelWithoutTableAttribute.Note));
    }

    [Test]
    public void TestTableNameWithoutAttribute_WithoutTableName()
    {
        // Without attribute or table name specified via parameter builder throws an exception
        Assert.Throws<ArgumentException>(() => SqlQueryBuilder.Insert(_modelWithoutTableAttribute));
    }
}