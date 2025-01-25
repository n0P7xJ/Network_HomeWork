using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPoshtaProjectCore_1.Migrations
{
    /// <inheritdoc />
    public partial class Add_tbl_Area : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Area",
                columns: table => new
                {
                    Ref = table.Column<string>(type: "TEXT", nullable: false),
                    AreaName = table.Column<string>(type: "TEXT", nullable: false),
                    AreaDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Area", x => x.Ref);
                });

            migrationBuilder.CreateTable(
                name: "tbl_City",
                columns: table => new
                {
                    Ref = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    AreaRef = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_City", x => x.Ref);
                    table.ForeignKey(
                        name: "FK_tbl_City_tbl_Area_AreaRef",
                        column: x => x.AreaRef,
                        principalTable: "tbl_Area",
                        principalColumn: "Ref",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Warehouse",
                columns: table => new
                {
                    Ref = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CityRef = table.Column<string>(type: "TEXT", nullable: false),
                    TypeOfWarehouse = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Warehouse", x => x.Ref);
                    table.ForeignKey(
                        name: "FK_tbl_Warehouse_tbl_City_CityRef",
                        column: x => x.CityRef,
                        principalTable: "tbl_City",
                        principalColumn: "Ref",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_City_AreaRef",
                table: "tbl_City",
                column: "AreaRef");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Warehouse_CityRef",
                table: "tbl_Warehouse",
                column: "CityRef");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Warehouse");

            migrationBuilder.DropTable(
                name: "tbl_City");

            migrationBuilder.DropTable(
                name: "tbl_Area");
        }
    }
}
