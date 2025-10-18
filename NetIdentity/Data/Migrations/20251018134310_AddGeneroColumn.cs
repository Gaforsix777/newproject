using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneroColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "genero",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "genero",
                table: "AspNetUsers");
        }
    }
}
