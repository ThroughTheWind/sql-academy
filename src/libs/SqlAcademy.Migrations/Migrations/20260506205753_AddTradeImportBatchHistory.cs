using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlAcademy.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTradeImportBatchHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeImportBatches",
                schema: "academy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessedUtc = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    DryRun = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedCount = table.Column<int>(type: "int", nullable: false),
                    ValidatedCount = table.Column<int>(type: "int", nullable: false),
                    DuplicateCount = table.Column<int>(type: "int", nullable: false),
                    ReadyToPublishCount = table.Column<int>(type: "int", nullable: false),
                    ImportedCount = table.Column<int>(type: "int", nullable: false),
                    RejectedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeImportBatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeImportBatchRows",
                schema: "academy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradeImportBatchId = table.Column<int>(type: "int", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InstrumentSymbol = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    Side = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TradedUtc = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    TradeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeImportBatchRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeImportBatchRows_TradeImportBatches_TradeImportBatchId",
                        column: x => x.TradeImportBatchId,
                        principalSchema: "academy",
                        principalTable: "TradeImportBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeImportBatchRows_Trades_TradeId",
                        column: x => x.TradeId,
                        principalSchema: "academy",
                        principalTable: "Trades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeImportBatches_ProcessedUtc",
                schema: "academy",
                table: "TradeImportBatches",
                column: "ProcessedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TradeImportBatchRows_TradeId",
                schema: "academy",
                table: "TradeImportBatchRows",
                column: "TradeId",
                filter: "[TradeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TradeImportBatchRows_TradeImportBatchId_RowNumber",
                schema: "academy",
                table: "TradeImportBatchRows",
                columns: new[] { "TradeImportBatchId", "RowNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeImportBatchRows",
                schema: "academy");

            migrationBuilder.DropTable(
                name: "TradeImportBatches",
                schema: "academy");
        }
    }
}
