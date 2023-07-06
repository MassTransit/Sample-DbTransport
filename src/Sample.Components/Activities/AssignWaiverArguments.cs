namespace Sample.Components.Activities;

public record AssignWaiverArguments
{
    public string? ParticipantEmailAddress { get; init; }
    public string? EventId { get; init; }
}