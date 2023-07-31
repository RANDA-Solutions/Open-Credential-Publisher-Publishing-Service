using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.PublishingService.Api.Data.Migrations.Application.OcpDb
{
    public partial class ExpandingKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyType",
                schema: "dbo",
                table: "SigningKey",
                type: "nvarchar(32)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivateKey",
                schema: "dbo",
                table: "SigningKey",
                type: "nvarchar(2048)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                schema: "dbo",
                table: "SigningKey",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StoredInKeyVault",
                schema: "dbo",
                table: "SigningKey",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Pathway",
                schema: "dbo",
                table: "PublishRequest",
                type: "nvarchar(128)",
                nullable: false,
                defaultValue: "1.0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyType",
                schema: "dbo",
                table: "SigningKey");

            migrationBuilder.DropColumn(
                name: "PrivateKey",
                schema: "dbo",
                table: "SigningKey");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                schema: "dbo",
                table: "SigningKey");

            migrationBuilder.DropColumn(
                name: "StoredInKeyVault",
                schema: "dbo",
                table: "SigningKey");

            migrationBuilder.DropColumn(
                name: "Pathway",
                schema: "dbo",
                table: "PublishRequest");
        }
    }
}
