using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.PublishingService.Api.Data.Migrations.Application.OcpDb
{
    public partial class RevocationDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RevocationListId",
                schema: "dbo",
                table: "PublishRequest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevocationReason",
                schema: "dbo",
                table: "PublishRequest",
                type: "nvarchar(32)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RevocationLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevocationLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublishRequest_RevocationListId",
                schema: "dbo",
                table: "PublishRequest",
                column: "RevocationListId");

            migrationBuilder.CreateIndex(
                name: "IX_RevocationList_PublicId",
                table: "RevocationLists",
                column: "PublicId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PublishRequest_RevocationLists_RevocationListId",
                schema: "dbo",
                table: "PublishRequest",
                column: "RevocationListId",
                principalTable: "RevocationLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublishRequest_RevocationLists_RevocationListId",
                schema: "dbo",
                table: "PublishRequest");

            migrationBuilder.DropTable(
                name: "RevocationLists");

            migrationBuilder.DropIndex(
                name: "CIX_PublishRequest_Id",
                schema: "dbo",
                table: "PublishRequest");

            migrationBuilder.DropIndex(
                name: "IX_PublishRequest_RevocationListId",
                schema: "dbo",
                table: "PublishRequest");

            migrationBuilder.DropColumn(
                name: "RevocationListId",
                schema: "dbo",
                table: "PublishRequest");

            migrationBuilder.DropColumn(
                name: "RevocationReason",
                schema: "dbo",
                table: "PublishRequest");
        }
    }
}
