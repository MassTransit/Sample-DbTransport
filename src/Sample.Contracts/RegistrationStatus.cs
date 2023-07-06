namespace Sample.Contracts;

public record RegistrationStatus :
    RegistrationDetail
{
    public string? Status { get; init; }
    public DateTime? ParticipantLicenseExpirationDate { get; init; }
    public Guid? RegistrationId { get; init; }
}