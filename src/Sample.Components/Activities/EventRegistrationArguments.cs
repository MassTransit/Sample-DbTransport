namespace Sample.Components.Activities;

public record EventRegistrationArguments
{
    public string? ParticipantEmailAddress { get; init; }

    public string? ParticipantLicenseNumber { get; init; }
    public DateTime? ParticipantLicenseExpirationDate { get; init; }

    public string? ParticipantCategory { get; init; }

    public string? EventId { get; init; }
    public string? RaceId { get; init; }
}