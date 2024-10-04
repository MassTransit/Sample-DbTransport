namespace Sample.Components;

using Contracts;
using MassTransit;


public static class PartitionKeyExtensions
{
    public static void ConfigurePartitionKeyFormatters(this IBusFactoryConfigurator cfg)
    {
        cfg.SendTopology.UsePartitionKeyFormatter<GetRegistrationStatus>(p => p.Message.SubmissionId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<ProcessRegistration>(p => p.Message.SubmissionId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<RegistrationCompleted>(p => p.Message.SubmissionId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<RegistrationDetail>(p => p.Message.SubmissionId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<RegistrationLicenseVerificationFailed>(p => p.Message.SubmissionId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<RegistrationPaymentFailed>(p => p.Message.SubmissionId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<RegistrationReceived>(p => p.Message.SubmissionId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<RetryDelayExpired>(p => p.Message.RegistrationId.ToString("N"));
        cfg.SendTopology.UsePartitionKeyFormatter<RegistrationStatus>(p => p.Message.SubmissionId.ToString("N"));
    }
}