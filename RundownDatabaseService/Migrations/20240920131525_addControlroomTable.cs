using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RundownDbService.Migrations
{
    /// <inheritdoc />
    public partial class addControlroomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControlRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hardwares",
                columns: table => new
                {
                    HardwareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DnsName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubnetMask = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gateway = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MacAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardwares", x => x.HardwareId);
                });

            migrationBuilder.CreateTable(
                name: "ControlRoomHardwares",
                columns: table => new
                {
                    ControlRoomsId = table.Column<int>(type: "int", nullable: false),
                    HardwaresHardwareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlRoomHardwares", x => new { x.ControlRoomsId, x.HardwaresHardwareId });
                    table.ForeignKey(
                        name: "FK_ControlRoomHardwares_ControlRooms_ControlRoomsId",
                        column: x => x.ControlRoomsId,
                        principalTable: "ControlRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ControlRoomHardwares_Hardwares_HardwaresHardwareId",
                        column: x => x.HardwaresHardwareId,
                        principalTable: "Hardwares",
                        principalColumn: "HardwareId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ControlRoomHardwares_HardwaresHardwareId",
                table: "ControlRoomHardwares",
                column: "HardwaresHardwareId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControlRoomHardwares");

            migrationBuilder.DropTable(
                name: "ControlRooms");

            migrationBuilder.DropTable(
                name: "Hardwares");
        }
    }
}
