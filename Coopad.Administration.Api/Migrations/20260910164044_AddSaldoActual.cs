using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coopad.Administration.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSaldoActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SaldoActual",
                table: "CashFlowProjections",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaldoActual",
                table: "CashFlowProjections");
        }
    }
}
