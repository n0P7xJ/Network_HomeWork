using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPoshtaProjectCore_1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Warehouse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Warehouse",
                columns: table => new
                {
                    Ref = table.Column<string>(type: "TEXT", nullable: false),
                    CityRef = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "IX_tbl_Warehouse_CityRef",
                table: "tbl_Warehouse",
                column: "CityRef");
        }
    }
}
