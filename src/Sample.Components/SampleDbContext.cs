namespace Sample.Components;

using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


public class SampleDbContext :
    SagaDbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new JobTypeSagaMap(false);
            yield return new JobSagaMap(false);
            yield return new JobAttemptSagaMap(false);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("sample");

        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
        #pragma warning disable EF1001
            if (entity is EntityType { IsImplicitlyCreatedJoinEntityType: true })
                continue;

            entity.SetTableName(entity.DisplayName());
        }

        ChangeEntityNames(modelBuilder);
    }

    static void ChangeEntityNames(ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (!string.IsNullOrWhiteSpace(tableName))
                entity.SetTableName(tableName.ToSnakeCase());

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
        }
    }
}