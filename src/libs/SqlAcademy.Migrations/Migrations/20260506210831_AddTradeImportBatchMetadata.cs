using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlAcademy.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTradeImportBatchMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                schema: "academy",
                table: "TradeImportBatches",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                schema: "academy",
                table: "TradeImportBatches",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeImportBatches_CorrelationId",
                schema: "academy",
                table: "TradeImportBatches",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeImportBatches_Source_ProcessedUtc",
                schema: "academy",
                table: "TradeImportBatches",
                columns: new[] { "Source", "ProcessedUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TradeImportBatches_CorrelationId",
                schema: "academy",
                table: "TradeImportBatches");

            migrationBuilder.DropIndex(
                name: "IX_TradeImportBatches_Source_ProcessedUtc",
                schema: "academy",
                table: "TradeImportBatches");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                schema: "academy",
                table: "TradeImportBatches");

            migrationBuilder.DropColumn(
                name: "Source",
                schema: "academy",
                table: "TradeImportBatches");
        }
    }
}
