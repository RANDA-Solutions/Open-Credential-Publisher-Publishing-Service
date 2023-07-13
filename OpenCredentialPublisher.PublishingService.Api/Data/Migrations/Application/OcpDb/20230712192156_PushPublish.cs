using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.PublishingService.Api.Data.Migrations.Application.OcpDb
{
    public partial class PushPublish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUri",
                schema: "dbo",
                table: "PublishRequest",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PushAfterPublish",
                schema: "dbo",
                table: "PublishRequest",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PushUri",
                schema: "dbo",
                table: "PublishRequest",
                type: "nvarchar(256)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUri",
                schema: "dbo",
                table: "PublishRequest");

            migrationBuilder.DropColumn(
                name: "PushAfterPublish",
                schema: "dbo",
                table: "PublishRequest");

            migrationBuilder.DropColumn(
                name: "PushUri",
                schema: "dbo",
                table: "PublishRequest");
        }
    }
}
