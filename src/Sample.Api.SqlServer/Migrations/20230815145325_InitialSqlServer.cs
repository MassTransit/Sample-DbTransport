using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Api.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    consumer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lock_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    received = table.Column<DateTime>(type: "datetime2", nullable: false),
                    receive_count = table.Column<int>(type: "int", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    consumed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    current_state = table.Column<int>(type: "int", nullable: false),
                    job_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    retry_attempt = table.Column<int>(type: "int", nullable: false),
                    service_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    instance_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    started = table.Column<DateTime>(type: "datetime2", nullable: true),
                    faulted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_check_token_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    current_state = table.Column<int>(type: "int", nullable: false),
                    submitted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    service_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_timeout = table.Column<TimeSpan>(type: "time", nullable: true),
                    job = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    attempt_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    retry_attempt = table.Column<int>(type: "int", nullable: false),
                    started = table.Column<DateTime>(type: "datetime2", nullable: true),
                    completed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    faulted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_slot_wait_token = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    job_retry_delay_token = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    current_state = table.Column<int>(type: "int", nullable: false),
                    active_job_count = table.Column<int>(type: "int", nullable: false),
                    concurrent_job_limit = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    override_job_limit = table.Column<int>(type: "int", nullable: true),
                    override_limit_expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    active_jobs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    instances = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    enqueue_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sent_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    headers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inbox_message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    inbox_consumer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    outbox_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content_type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    message_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    initiator_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    source_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    destination_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    response_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    fault_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    expiration_time = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    outbox_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lock_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    participant_email_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    participant_license_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    participant_category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    participant_license_expiration_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    registration_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    card_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    event_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    race_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    current_state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    retry_attempt = table.Column<int>(type: "int", nullable: true),
                    schedule_retry_token = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                name: "IX_outbox_message_inbox_message_id_inbox_consumer_id_sequence_number",
                schema: "sample",
                table: "outbox_message",
                columns: new[] { "inbox_message_id", "inbox_consumer_id", "sequence_number" },
                unique: true,
                filter: "[inbox_message_id] IS NOT NULL AND [inbox_consumer_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_outbox_id_sequence_number",
                schema: "sample",
                table: "outbox_message",
                columns: new[] { "outbox_id", "sequence_number" },
                unique: true,
                filter: "[outbox_id] IS NOT NULL");

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
