using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RundownDbService.Migrations
{
    /// <inheritdoc />
    public partial class AddRundownControlRoomRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControlRoomId",
                table: "Rundowns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rundowns_ControlRoomId",
                table: "Rundowns",
                column: "ControlRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rundowns_ControlRooms_ControlRoomId",
                table: "Rundowns",
                column: "ControlRoomId",
                principalTable: "ControlRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rundowns_ControlRooms_ControlRoomId",
                table: "Rundowns");

            migrationBuilder.DropIndex(
                name: "IX_Rundowns_ControlRoomId",
                table: "Rundowns");

            migrationBuilder.DropColumn(
                name: "ControlRoomId",
                table: "Rundowns");
        }
    }
}
