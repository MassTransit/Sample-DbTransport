namespace Sample.Components.StateMachines;

using Contracts;
using MassTransit;


public class RegistrationStateMachine :
    MassTransitStateMachine<RegistrationState>
{
    public RegistrationStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => RegistrationStatusRequested, x =>
        {
            x.ReadOnly = true;
            x.OnMissingInstance(m => m.Fault());
        });

        Schedule(() => RetryDelayExpired, saga => saga.ScheduleRetryToken, x =>
        {
            x.Received = r =>
            {
                r.CorrelateById(context => context.Message.RegistrationId);
                r.ConfigureConsumeTopology = false;
            };
        });

        Initially(
            When(EventRegistrationReceived)
                .Initialize()
                .InitiateProcessing()
                .TransitionTo(Received));

        During(Received,
            When(EventRegistrationCompleted)
                .Registered()
                .TransitionTo(Registered),
            When(LicenseVerificationFailed)
                .InvalidLicense()
                .TransitionTo(Suspended),
            When(PaymentFailed)
                .PaymentFailed()
                .TransitionTo(Suspended));

        During(Suspended,
            When(EventRegistrationReceived)
                .Initialize()
                .InitiateProcessing()
                .TransitionTo(Received));

        During(WaitingToRetry,
            When(RetryDelayExpired.Received)
                .RetryProcessing()
                .TransitionTo(Received));

        DuringAny(
            When(RegistrationStatusRequested)
                .RespondAsync(x => x.Init<RegistrationStatus>(new
                    {
                        SubmissionId = x.Saga.CorrelationId,
                        x.Saga.ParticipantEmailAddress,
                        x.Saga.ParticipantLicenseNumber,
                        x.Saga.ParticipantLicenseExpirationDate,
                        x.Saga.RegistrationId,
                        x.Saga.EventId,
                        x.Saga.RaceId,
                        Status = x.Saga.CurrentState
                    })
                ));

        // could easily be configured via options
        const int retryCount = 5;
        var retryDelay = TimeSpan.FromSeconds(10);

        WhenEnter(Suspended, x => x
            .If(context => context.Saga.RetryAttempt < retryCount,
                retry => retry
                    .Schedule(RetryDelayExpired, context => new RetryDelayExpired(context.Saga.CorrelationId), _ => retryDelay)
                    .TransitionTo(WaitingToRetry)
            )
        );
    }

    //
    // ReSharper disable UnassignedGetOnlyAutoProperty
    // ReSharper disable MemberCanBePrivate.Global
    public State Received { get; } = null!;
    public State Registered { get; } = null!;
    public State WaitingToRetry { get; } = null!;
    public State Suspended { get; } = null!;

    public Event<RegistrationReceived> EventRegistrationReceived { get; } = null!;
    public Event<GetRegistrationStatus> RegistrationStatusRequested { get; } = null!;
    public Event<RegistrationCompleted> EventRegistrationCompleted { get; } = null!;
    public Event<RegistrationLicenseVerificationFailed> LicenseVerificationFailed { get; } = null!;
    public Event<RegistrationPaymentFailed> PaymentFailed { get; } = null!;

    public Schedule<RegistrationState, RetryDelayExpired> RetryDelayExpired { get; } = null!;
}


static class RegistrationStateMachineBehaviorExtensions
{
    public static EventActivityBinder<RegistrationState, RegistrationReceived> Initialize(
        this EventActivityBinder<RegistrationState, RegistrationReceived> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.ParticipantEmailAddress = context.Message.ParticipantEmailAddress;
            context.Saga.ParticipantLicenseNumber = context.Message.ParticipantLicenseNumber;
            context.Saga.ParticipantCategory = context.Message.ParticipantCategory;

            context.Saga.EventId = context.Message.EventId;
            context.Saga.RaceId = context.Message.RaceId;

            LogContext.Info?.Log("Processing: {0} ({1})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);
        });
    }

    public static EventActivityBinder<RegistrationState, RegistrationReceived> InitiateProcessing(
        this EventActivityBinder<RegistrationState, RegistrationReceived> binder)
    {
        return binder.PublishAsync(context => context.Init<ProcessRegistration>(context.Message));
    }

    public static EventActivityBinder<RegistrationState, RetryDelayExpired> RetryProcessing(
        this EventActivityBinder<RegistrationState, RetryDelayExpired> binder)
    {
        return binder
            .Then(context => context.Saga.RetryAttempt++)
            .PublishAsync(context => context.Init<ProcessRegistration>(new
            {
                SubmissionId = context.Saga.CorrelationId,
                context.Saga.ParticipantEmailAddress,
                context.Saga.ParticipantLicenseNumber,
                context.Saga.ParticipantCategory,
                context.Saga.CardNumber,
                context.Saga.EventId,
                context.Saga.RaceId,
                __Header_Registration_RetryAttempt = context.Saga.RetryAttempt
            }));
    }

    public static EventActivityBinder<RegistrationState, RegistrationCompleted> Registered(
        this EventActivityBinder<RegistrationState, RegistrationCompleted> binder)
    {
        return binder.Then(context =>
        {
            LogContext.Info?.Log("Registered: {0} ({1})", context.Message.SubmissionId, context.Saga.ParticipantEmailAddress);

            context.Saga.ParticipantLicenseExpirationDate = context.GetVariable<DateTime>("ParticipantLicenseExpirationDate");
            context.Saga.RegistrationId = context.GetVariable<Guid>("RegistrationId");
        });
    }

    public static EventActivityBinder<RegistrationState, RegistrationLicenseVerificationFailed> InvalidLicense(
        this EventActivityBinder<RegistrationState, RegistrationLicenseVerificationFailed> binder)
    {
        return binder.Then(context =>
        {
            LogContext.Info?.Log("Invalid License: {0} ({1}) - {2}", context.Message.SubmissionId, context.Saga.ParticipantLicenseNumber,
                context.Message.ExceptionInfo?.Message);

            context.Saga.Reason = "Invalid License";
        });
    }

    public static EventActivityBinder<RegistrationState, RegistrationPaymentFailed> PaymentFailed(
        this EventActivityBinder<RegistrationState, RegistrationPaymentFailed> binder)
    {
        return binder.Then(context =>
        {
            LogContext.Info?.Log("Payment Failed: {0} ({1}) - {2}", context.Message.SubmissionId, context.Saga.ParticipantEmailAddress,
                context.Message.ExceptionInfo?.Message);

            context.Saga.Reason = "Payment Failed";
        });
    }
}