using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RundownDbService.Migrations
{
    /// <inheritdoc />
    public partial class newRelationToRundown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateVideos");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Videos",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Rundown",
                columns: table => new
                {
                    RundownId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RundownName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RundownType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RundownDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArchivedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rundown", x => x.RundownId);
                });

            migrationBuilder.CreateTable(
                name: "RundownVideos",
                columns: table => new
                {
                    RundownsRundownId = table.Column<int>(type: "int", nullable: false),
                    VideoObjectsVideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RundownVideos", x => new { x.RundownsRundownId, x.VideoObjectsVideoId });
                    table.ForeignKey(
                        name: "FK_RundownVideos_Rundown_RundownsRundownId",
                        column: x => x.RundownsRundownId,
                        principalTable: "Rundown",
                        principalColumn: "RundownId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RundownVideos_Videos_VideoObjectsVideoId",
                        column: x => x.VideoObjectsVideoId,
                        principalTable: "Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RundownVideos_VideoObjectsVideoId",
                table: "RundownVideos",
                column: "VideoObjectsVideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RundownVideos");

            migrationBuilder.DropTable(
                name: "Rundown");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Videos");

            migrationBuilder.CreateTable(
                name: "TemplateVideos",
                columns: table => new
                {
                    TemplatesId = table.Column<int>(type: "int", nullable: false),
                    VideoObjectsVideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateVideos", x => new { x.TemplatesId, x.VideoObjectsVideoId });
                    table.ForeignKey(
                        name: "FK_TemplateVideos_Templates_TemplatesId",
                        column: x => x.TemplatesId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateVideos_Videos_VideoObjectsVideoId",
                        column: x => x.VideoObjectsVideoId,
                        principalTable: "Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVideos_VideoObjectsVideoId",
                table: "TemplateVideos",
                column: "VideoObjectsVideoId");
        }
    }
}
