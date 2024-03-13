using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThucTapProject.Migrations
{
    /// <inheritdoc />
    public partial class updatev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accountt_Decentralization_DecentralizationId",
                table: "Accountt");

            migrationBuilder.AlterColumn<int>(
                name: "DecentralizationId",
                table: "Accountt",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountt_Decentralization_DecentralizationId",
                table: "Accountt",
                column: "DecentralizationId",
                principalTable: "Decentralization",
                principalColumn: "DecentralizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accountt_Decentralization_DecentralizationId",
                table: "Accountt");

            migrationBuilder.AlterColumn<int>(
                name: "DecentralizationId",
                table: "Accountt",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accountt_Decentralization_DecentralizationId",
                table: "Accountt",
                column: "DecentralizationId",
                principalTable: "Decentralization",
                principalColumn: "DecentralizationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
