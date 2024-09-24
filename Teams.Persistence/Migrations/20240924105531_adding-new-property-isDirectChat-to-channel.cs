using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teams.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addingnewpropertyisDirectChattochannel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDirectChat",
                table: "Channels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDirectChat",
                table: "Channels");
        }
    }
}
