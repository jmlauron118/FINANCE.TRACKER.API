using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FINANCE.TRACKER.API.Migrations
{
    /// <inheritdoc />
    public partial class RecreateModuleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Modules",
            //    columns: table => new
            //    {
            //        ModuleId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ModuleName = table.Column<string>(type: "nvarchar(50)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(150)", nullable: true),
            //        ModulePage = table.Column<string>(type: "nvarchar(50)", nullable: true),
            //        Icon = table.Column<string>(type: "nvarchar(25)", nullable: true),
            //        SortOrder = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedBy = table.Column<int>(type: "int", nullable: false),
            //        DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Modules", x => x.ModuleId);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
