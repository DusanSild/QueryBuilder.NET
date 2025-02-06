namespace QueryBuilderTests.Models;

public class ModelWithoutTableAttribute
{
    public long Id { get; set; }

    public string Note { get; set; } = "";
    
    public DateTimeOffset Created { get; set; }
    
    public DateTimeOffset Updated { get; set; }
}