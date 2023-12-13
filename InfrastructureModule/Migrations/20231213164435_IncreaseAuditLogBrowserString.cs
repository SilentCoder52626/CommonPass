using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureModule.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseAuditLogBrowserString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "browser",
                table: "audit_logs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 13, 22, 29, 35, 551, DateTimeKind.Local).AddTicks(4594),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 13, 22, 7, 45, 880, DateTimeKind.Local).AddTicks(5436));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "browser",
                table: "audit_logs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 13, 22, 7, 45, 880, DateTimeKind.Local).AddTicks(5436),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 13, 22, 29, 35, 551, DateTimeKind.Local).AddTicks(4594));
        }
    }
}
