using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.PublishingService.Api.Data.Migrations.Application.OcpDb
{
    public partial class OcpInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ClrPublishLog",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrPublishLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublishRequest",
                schema: "dbo",
                columns: table => new
                {
                    RequestId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestIdentity = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false),
                    ContainsPdf = table.Column<bool>(nullable: true),
                    PackageSignedTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: true),
                    PublishState = table.Column<string>(type: "nvarchar(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishRequest", x => x.RequestId)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AccessKey",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false),
                    Expired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessKey_PublishRequest_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "dbo",
                        principalTable: "PublishRequest",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    ContainerId = table.Column<string>(type: "varchar(256)", nullable: true),
                    FileName = table.Column<string>(type: "varchar(256)", nullable: false),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_PublishRequest_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "dbo",
                        principalTable: "PublishRequest",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SigningKey",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    KeyName = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    VaultKeyIdentifier = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    IssuerId = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false),
                    Expired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SigningKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SigningKey_PublishRequest_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "dbo",
                        principalTable: "PublishRequest",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
               name: "CIX_PublishRequest_Id",
               table: "PublishRequest",
               schema: "dbo",
               column: "Id").Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessKey_RequestId",
                schema: "dbo",
                table: "AccessKey",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_File_RequestId",
                schema: "dbo",
                table: "File",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SigningKey_RequestId",
                schema: "dbo",
                table: "SigningKey",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessKey",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClrPublishLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "File",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SigningKey",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PublishRequest",
                schema: "dbo");
        }
    }
}
