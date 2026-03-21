namespace DevCollab.Domain.Exceptions;

public class DomainValidationException : DomainException
{
    public IDictionary<string, string[]> Errors { get; }

    public DomainValidationException(IDictionary<string, string[]> errors)
        : base("One or more domain validation errors occurred.")
    {
        Errors = errors;
    }
}
