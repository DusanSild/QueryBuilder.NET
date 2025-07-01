# QueryBuilder.NET

## This is a prerelase unfinished version

QueryBuilder.NET is a simple SQL query generator based on **Dapper** and **Dapper.ColumnMapper** that helps building sql queries in a fluent builder-like style.

In this early version:
- only Insert, Delete and Update commands are implemented
- only PostgreSQL syntax is supported
- only primitive straigh-forward WHERE clauses are supported

## How it works

Let's say you have some model class, for eg. Person:

```
public class Person
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";    
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = "";
    public int Age { get; set; }
    public decimal Salary { get; set; }
}
```

If you want to create CRUD operations for it using Dapper, you start writing SQL query like this:
```
string insertQuery = $@"
                    INSERT INTO ""Persons""
                    (
                        ""FirstName"",
                        ""LastName"",
                        ""DateOfBirth"",
                        ""Email"",
                        ""Age"",
                        ""Salary""
                    )
                    VALUES
                    (
                        @firstName,
                        @lastName,
                        @dateOfBirth,
                        @email,
                        @age,
                        @salary
                    )
                    RETURNING ""Id""; ";
```

It looks pretty easy, but as your model classes grow and become more complicated, you have to edit these queries and keep them up to date.
In larger projects this is a rutine job, that is very error prone and boring. You don't want to focus on commas and quotes in SQL queries, when you have deadline upon you.

And with QueryBuilder.NET you can write just this:
```
//person is instance of model you want to save
var dapperQuery = SqlQueryBuilder.InsertInto(person, "Persons").ReturningId().BuildQuery();
```

It will generate sql command exactly like the one above and also dynamic parameters filled from the instance you provided.
Then to execute it:
```
var insertedId = await connection.ExecuteScalarAsync<Guid>(dapperQuery.CommandText, dapperQuery.Parameters);
```
