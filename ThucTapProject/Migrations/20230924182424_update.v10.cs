using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThucTapProject.Migrations
{
    /// <inheritdoc />
    public partial class updatev10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComment",
                table: "OrderDetail",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComment",
                table: "OrderDetail");
        }
    }
}
