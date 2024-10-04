using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Api.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "global_concurrent_job_limit",
                schema: "sample",
                table: "job_type_saga",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "properties",
                schema: "sample",
                table: "job_type_saga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cron_expression",
                schema: "sample",
                table: "job_saga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "end_date",
                schema: "sample",
                table: "job_saga",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "incomplete_attempts",
                schema: "sample",
                table: "job_saga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "job_state",
                schema: "sample",
                table: "job_saga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "last_progress_limit",
                schema: "sample",
                table: "job_saga",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "last_progress_sequence_number",
                schema: "sample",
                table: "job_saga",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "last_progress_value",
                schema: "sample",
                table: "job_saga",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "next_start_date",
                schema: "sample",
                table: "job_saga",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "start_date",
                schema: "sample",
                table: "job_saga",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "time_zone_id",
                schema: "sample",
                table: "job_saga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_job_attempt_saga_job_saga_job_id",
                schema: "sample",
                table: "job_attempt_saga",
                column: "job_id",
                principalSchema: "sample",
                principalTable: "job_saga",
                principalColumn: "correlation_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_outbox_message_inbox_state_inbox_message_id_inbox_consumer_id",
                schema: "sample",
                table: "outbox_message",
                columns: new[] { "inbox_message_id", "inbox_consumer_id" },
                principalSchema: "sample",
                principalTable: "inbox_state",
                principalColumns: new[] { "message_id", "consumer_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_outbox_message_outbox_state_outbox_id",
                schema: "sample",
                table: "outbox_message",
                column: "outbox_id",
                principalSchema: "sample",
                principalTable: "outbox_state",
                principalColumn: "outbox_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_attempt_saga_job_saga_job_id",
                schema: "sample",
                table: "job_attempt_saga");

            migrationBuilder.DropForeignKey(
                name: "FK_outbox_message_inbox_state_inbox_message_id_inbox_consumer_id",
                schema: "sample",
                table: "outbox_message");

            migrationBuilder.DropForeignKey(
                name: "FK_outbox_message_outbox_state_outbox_id",
                schema: "sample",
                table: "outbox_message");

            migrationBuilder.DropColumn(
                name: "global_concurrent_job_limit",
                schema: "sample",
                table: "job_type_saga");

            migrationBuilder.DropColumn(
                name: "properties",
                schema: "sample",
                table: "job_type_saga");

            migrationBuilder.DropColumn(
                name: "cron_expression",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "end_date",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "incomplete_attempts",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "job_state",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "last_progress_limit",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "last_progress_sequence_number",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "last_progress_value",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "next_start_date",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "start_date",
                schema: "sample",
                table: "job_saga");

            migrationBuilder.DropColumn(
                name: "time_zone_id",
                schema: "sample",
                table: "job_saga");
        }
    }
}
