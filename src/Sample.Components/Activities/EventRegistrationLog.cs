namespace Sample.Components.Activities;

public record EventRegistrationLog
{
    public Guid? RegistrationId { get; init; }
    public string? ParticipantEmailAddress { get; init; }
}