namespace Sample.Contracts;

public record GetRegistrationStatus
{
    public Guid SubmissionId { get; init; }
}