namespace Sample.Components.Activities;

public record LicenseVerificationArguments
{
    public string? ParticipantLicenseNumber { get; init; }

    public string? EventType { get; init; }
    public string? ParticipantCategory { get; init; }
}