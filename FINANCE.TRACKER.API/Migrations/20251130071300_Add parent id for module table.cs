using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FINANCE.TRACKER.API.Migrations
{
    /// <inheritdoc />
    public partial class Addparentidformoduletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "ParentId",
            //    table: "Modules",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "ParentId",
            //    table: "Modules");
        }
    }
}
