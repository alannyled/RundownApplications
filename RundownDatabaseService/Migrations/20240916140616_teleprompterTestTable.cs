using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RundownDbService.Migrations
{
    /// <inheritdoc />
    public partial class teleprompterTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RundownVideos_Rundown_RundownsRundownId",
                table: "RundownVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rundown",
                table: "Rundown");

            migrationBuilder.RenameTable(
                name: "Rundown",
                newName: "Rundowns");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rundowns",
                table: "Rundowns",
                column: "RundownId");

            migrationBuilder.CreateTable(
                name: "TeleprompterTests",
                columns: table => new
                {
                    TeleprompterTestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeleprompterTests", x => x.TeleprompterTestId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RundownVideos_Rundowns_RundownsRundownId",
                table: "RundownVideos",
                column: "RundownsRundownId",
                principalTable: "Rundowns",
                principalColumn: "RundownId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RundownVideos_Rundowns_RundownsRundownId",
                table: "RundownVideos");

            migrationBuilder.DropTable(
                name: "TeleprompterTests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rundowns",
                table: "Rundowns");

            migrationBuilder.RenameTable(
                name: "Rundowns",
                newName: "Rundown");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rundown",
                table: "Rundown",
                column: "RundownId");

            migrationBuilder.AddForeignKey(
                name: "FK_RundownVideos_Rundown_RundownsRundownId",
                table: "RundownVideos",
                column: "RundownsRundownId",
                principalTable: "Rundown",
                principalColumn: "RundownId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
