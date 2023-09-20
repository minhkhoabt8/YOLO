using System.ComponentModel.DataAnnotations;

namespace Document.Infrastructure.DTOs.DocumentType;

public class DocumentTypeWriteDTO
{
    [Required]
    public string Code { get; set; }
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}