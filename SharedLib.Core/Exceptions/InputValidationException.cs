using System.ComponentModel.DataAnnotations;

namespace SharedLib.Core.Exceptions;

public class InputValidationException : Exception
{
    public InputValidationException(IEnumerable<ValidationResult> validationResults, string? message = null) : base(
        message ?? "Input validation errors")
    {
        ValidationResults = validationResults;
    }

    public IEnumerable<ValidationResult> ValidationResults { get; set; }
}