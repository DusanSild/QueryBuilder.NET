using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueryBuilderTests.Models;

[Table(TableName)]
public class ModelWithAttributes
{
    public const string TableName = "customers";
    
    [Key]
    public Guid Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = "";

    [Column("last_name")]
    public string LastName { get; set; } = "";
    
    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }

    [Column("email")]
    public string Email { get; set; } = "";
    
    [Column("age")]
    public int Age { get; set; }
    
    [Column("salary")]
    public decimal Salary { get; set; }
}