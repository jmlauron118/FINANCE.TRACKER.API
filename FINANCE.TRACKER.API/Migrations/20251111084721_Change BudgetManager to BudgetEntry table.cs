using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FINANCE.TRACKER.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBudgetManagertoBudgetEntrytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BudgetManager");

            //migrationBuilder.CreateTable(
            //    name: "BudgetEntries",
            //    columns: table => new
            //    {
            //        BudgetEntryId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BudgetCagetoryId = table.Column<int>(type: "int", nullable: false),
            //        ExpenseCategoryId = table.Column<int>(type: "int", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        DateUsed = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedBy = table.Column<int>(type: "int", nullable: false),
            //        DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BudgetEntries", x => x.BudgetEntryId);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BudgetEntries");

            //migrationBuilder.CreateTable(
            //    name: "BudgetManager",
            //    columns: table => new
            //    {
            //        BudgetManagerId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        BudgetCagetoryId = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        DateUsed = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ExpenseCategoryId = table.Column<int>(type: "int", nullable: true),
            //        UpdatedBy = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BudgetManager", x => x.BudgetManagerId);
            //    });
        }
    }
}
