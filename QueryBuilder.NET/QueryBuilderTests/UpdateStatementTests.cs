using QueryBuilderDotNet.Builder;
using QueryBuilderDotNet.Extensions;
using QueryBuilderDotNet.Utils;
using QueryBuilderTests.Models;

namespace QueryBuilderTests;

public class UpdateStatementTests
{
    private ModelWithAttributes _modelAttributes;
    private ModelWithoutAttributes _modelWithoutAttributes;
    
    [SetUp]
    public void Setup()
    {
        _modelAttributes = new ModelWithAttributes
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
    public void TestTableNameWithAttribute()
    {
        var updateStatement = SqlQueryBuilder.Update(_modelAttributes)
            .Where<ModelWithoutAttributes, long>(m => m.Id, _modelWithoutAttributes.Id);
        
        // assert statement is created
        Assert.That(updateStatement, Is.Not.Null);
        
        // assert that table is taken from the Table attribute
        Assert.That(updateStatement.TableName, Is.EqualTo(ModelWithAttributes.TableName));
        
        // assert that ID column name is default
        Assert.That(updateStatement.IdColumnName, Is.EqualTo(QueryBuilderDefaults.IdColumnName));

        // build query
        var dapperQuery = updateStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"UPDATE ""{ModelWithAttributes.TableName}"" SET", dapperQuery.CommandText);
        
        // assert that command contains a WHERE clause for ID filtering
        StringAssert.Contains("WHERE \"Id\" =", dapperQuery.CommandText);
        
        // assert that query parameters exist
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        // assert that query parameters are correctly filled from instance of model class
        Assert.That(dapperQuery.Parameters.Get<string>("@firstName"), Is.EqualTo(_modelAttributes.FirstName));
    }
    
    [Test]
    public void TestTableNameWithoutAttribute()
    {
        var tableName = "Notes";
        var updateStatement = SqlQueryBuilder.Update(_modelWithoutAttributes, tableName)
            .Where<ModelWithoutAttributes, long>(m => m.Id, _modelWithoutAttributes.Id);
        
        // assert statement is created
        Assert.That(updateStatement, Is.Not.Null);
        
        // assert that table is taken from the Table attribute
        Assert.That(updateStatement.TableName, Is.EqualTo(tableName));
        
        // assert that ID column name is default
        Assert.That(updateStatement.IdColumnName, Is.EqualTo(QueryBuilderDefaults.IdColumnName));

        // build query
        var dapperQuery = updateStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"UPDATE ""{tableName}"" SET ", dapperQuery.CommandText);
        
        // assert that command contains a WHERE clause for ID filtering
        StringAssert.Contains("WHERE \"Id\" =", dapperQuery.CommandText);
        
        // assert that query parameters exist
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        // assert that query parameters are correctly filled from instance of model class
        Assert.That(dapperQuery.Parameters.Get<string>("@note"), Is.EqualTo(_modelWithoutAttributes.Note));
    }
    
    [Test]
    public void TestUpdatingColumns()
    {
        var updateStatement = SqlQueryBuilder.Update(_modelAttributes).Updating(m => new { m.Age, m.Email })
            .Where<ModelWithoutAttributes, long>(m => m.Id, _modelWithoutAttributes.Id);
        
        // assert statement is created
        Assert.That(updateStatement, Is.Not.Null);

        // build query
        var dapperQuery = updateStatement.BuildQuery();
        
        // assert that command was generated 
        Assert.That(dapperQuery.CommandText, Is.Not.Null);
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains($@"UPDATE ""{ModelWithAttributes.TableName}"" SET", dapperQuery.CommandText);
        
        // assert that command contains update statements for age and email
        StringAssert.Contains(@"""age"" = @age", dapperQuery.CommandText);
        StringAssert.Contains(@"""email"" = @email", dapperQuery.CommandText);
        
        // assert that command does not contain update statements for other properties
        StringAssert.DoesNotContain("\"first_name\"", dapperQuery.CommandText);
        StringAssert.DoesNotContain("\"last_name\"", dapperQuery.CommandText);
        StringAssert.DoesNotContain("\"date_of_birth\"", dapperQuery.CommandText);
        StringAssert.DoesNotContain("\"salary\"", dapperQuery.CommandText);
        
        // assert that command contains a WHERE clause for ID filtering
        StringAssert.Contains("WHERE \"Id\" =", dapperQuery.CommandText);
        
        // assert that query parameters exist
        Assert.That(dapperQuery.Parameters, Is.Not.Null);
        
        // assert that query parameters are correctly filled from instance of model class
        Assert.That(dapperQuery.Parameters.Get<string>("@email"), Is.EqualTo(_modelAttributes.Email));
    }

    [Test]
    public void TestTableNameWithoutAttribute_WithoutTableName()
    {
        // Without attribute or table name specified via parameter builder throws an exception
        Assert.Throws<ArgumentException>(() => SqlQueryBuilder.Update(_modelWithoutAttributes));
    }

    [Test]
    public void TestCustomIdColumnName()
    {
        var dapperQuery = SqlQueryBuilder.Update(_modelAttributes).ReturningId("SomeId").BuildQuery();
        
        // assert that command contains INSERT INTO table name
        StringAssert.Contains(@"RETURNING ""SomeId""", dapperQuery.CommandText);
    }
}