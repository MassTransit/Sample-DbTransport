namespace Sample.Contracts;

using MassTransit;


public record RegistrationPaymentFailed
{
    public Guid SubmissionId { get; init; }

    public ExceptionInfo? ExceptionInfo { get; init; }
}