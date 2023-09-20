namespace Document.Infrastructure.DTOs.DocumentType;

public class DocumentTypeReadDTO
{
    public int ID { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}