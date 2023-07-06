using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sample.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sample");

            migrationBuilder.CreateTable(
                name: "inbox_state",
                schema: "sample",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    consumer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lock_id = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    receive_count = table.Column<int>(type: "integer", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_state", x => x.id);
                    table.UniqueConstraint("AK_inbox_state_message_id_consumer_id", x => new { x.message_id, x.consumer_id });
                });

            migrationBuilder.CreateTable(
                name: "job_attempt_saga",
                schema: "sample",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<int>(type: "integer", nullable: false),
                    job_id = table.Column<Guid>(type: "uuid", nullable: false),
                    retry_attempt = table.Column<int>(type: "integer", nullable: false),
                    service_address = table.Column<string>(type: "text", nullable: true),
                    instance_address = table.Column<string>(type: "text", nullable: true),
                    started = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    faulted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status_check_token_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_attempt_saga", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "job_saga",
                schema: "sample",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<int>(type: "integer", nullable: false),
                    submitted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    service_address = table.Column<string>(type: "text", nullable: true),
                    job_timeout = table.Column<TimeSpan>(type: "interval", nullable: true),
                    job = table.Column<string>(type: "text", nullable: true),
                    job_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    retry_attempt = table.Column<int>(type: "integer", nullable: false),
                    started = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    faulted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    job_slot_wait_token = table.Column<Guid>(type: "uuid", nullable: true),
                    job_retry_delay_token = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_saga", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "job_type_saga",
                schema: "sample",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<int>(type: "integer", nullable: false),
                    active_job_count = table.Column<int>(type: "integer", nullable: false),
                    concurrent_job_limit = table.Column<int>(type: "integer", nullable: false),
                    override_job_limit = table.Column<int>(type: "integer", nullable: true),
                    override_limit_expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    active_jobs = table.Column<string>(type: "text", nullable: true),
                    instances = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_type_saga", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message",
                schema: "sample",
                columns: table => new
                {
                    sequence_number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enqueue_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sent_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    headers = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    inbox_message_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inbox_consumer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    outbox_id = table.Column<Guid>(type: "uuid", nullable: true),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    message_type = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    initiator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    request_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    destination_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    response_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    fault_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_message", x => x.sequence_number);
                });

            migrationBuilder.CreateTable(
                name: "outbox_state",
                schema: "sample",
                columns: table => new
                {
                    outbox_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lock_id = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_state", x => x.outbox_id);
                });

            migrationBuilder.CreateTable(
                name: "registration_state",
                schema: "sample",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    participant_email_address = table.Column<string>(type: "text", nullable: true),
                    participant_license_number = table.Column<string>(type: "text", nullable: true),
                    participant_category = table.Column<string>(type: "text", nullable: true),
                    participant_license_expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    registration_id = table.Column<Guid>(type: "uuid", nullable: true),
                    card_number = table.Column<string>(type: "text", nullable: true),
                    event_id = table.Column<string>(type: "text", nullable: true),
                    race_id = table.Column<string>(type: "text", nullable: true),
                    current_state = table.Column<string>(type: "text", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    retry_attempt = table.Column<int>(type: "integer", nullable: true),
                    schedule_retry_token = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registration_state", x => x.correlation_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inbox_state_delivered",
                schema: "sample",
                table: "inbox_state",
                column: "delivered");

            migrationBuilder.CreateIndex(
                name: "IX_job_attempt_saga_job_id_retry_attempt",
                schema: "sample",
                table: "job_attempt_saga",
                columns: new[] { "job_id", "retry_attempt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_enqueue_time",
                schema: "sample",
                table: "outbox_message",
                column: "enqueue_time");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_expiration_time",
                schema: "sample",
                table: "outbox_message",
                column: "expiration_time");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_inbox_message_id_inbox_consumer_id_sequence_~",
                schema: "sample",
                table: "outbox_message",
                columns: new[] { "inbox_message_id", "inbox_consumer_id", "sequence_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_outbox_id_sequence_number",
                schema: "sample",
                table: "outbox_message",
                columns: new[] { "outbox_id", "sequence_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_outbox_state_created",
                schema: "sample",
                table: "outbox_state",
                column: "created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_state",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "job_attempt_saga",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "job_saga",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "job_type_saga",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "outbox_message",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "outbox_state",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "registration_state",
                schema: "sample");
        }
    }
}
