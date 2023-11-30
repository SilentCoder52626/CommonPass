using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureModule.Migrations
{
    /// <inheritdoc />
    public partial class AddNameInAccountDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 30, 23, 24, 10, 297, DateTimeKind.Local).AddTicks(2445),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 11, 27, 23, 42, 46, 726, DateTimeKind.Local).AddTicks(743));

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "AccountDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "AccountDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 27, 23, 42, 46, 726, DateTimeKind.Local).AddTicks(743),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 11, 30, 23, 24, 10, 297, DateTimeKind.Local).AddTicks(2445));
        }
    }
}
