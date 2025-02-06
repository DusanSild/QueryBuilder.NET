using System.ComponentModel.DataAnnotations.Schema;

namespace QueryBuilderTests.Models;

[Table(TableName)]
public class ModelWithTableAttribute
{
    public const string TableName = "customers";
    public Guid Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";
    
    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = "";
    
    public int Age { get; set; }
    
    public decimal Salary { get; set; }
}