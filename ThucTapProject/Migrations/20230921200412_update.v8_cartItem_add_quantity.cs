using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThucTapProject.Migrations
{
    /// <inheritdoc />
    public partial class updatev8_cartItem_add_quantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CartItem");
        }
    }
}
