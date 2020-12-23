using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.PublishingService.Api.Data.Migrations.Application.OcpDb
{
    public partial class AddProcessingState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProcessingState",
                schema: "dbo",
                table: "PublishRequest",
                type: "nvarchar(64)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessingState",
                schema: "dbo",
                table: "PublishRequest");
        }
    }
}
