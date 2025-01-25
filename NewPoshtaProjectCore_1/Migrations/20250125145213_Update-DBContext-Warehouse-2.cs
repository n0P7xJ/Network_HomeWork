using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPoshtaProjectCore_1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBContextWarehouse2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "tbl_Warehouse",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SettlementType",
                table: "tbl_Warehouse",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "tbl_Warehouse");

            migrationBuilder.DropColumn(
                name: "SettlementType",
                table: "tbl_Warehouse");
        }
    }
}
