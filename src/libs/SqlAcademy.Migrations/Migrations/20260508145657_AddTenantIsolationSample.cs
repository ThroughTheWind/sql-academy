using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlAcademy.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIsolationSample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.CreateTable(
                name: "TenantOrders",
                schema: "academy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantOrders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantOrders_TenantId_CreatedUtc",
                schema: "academy",
                table: "TenantOrders",
                columns: new[] { "TenantId", "CreatedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantOrders_TenantId_OrderNumber",
                schema: "academy",
                table: "TenantOrders",
                columns: new[] { "TenantId", "OrderNumber" },
                unique: true);

            migrationBuilder.Sql(
                """
IF DATABASE_PRINCIPAL_ID(N'rls_policy_admin') IS NULL
BEGIN
    CREATE ROLE [rls_policy_admin];
END
""");

            migrationBuilder.Sql(
                """
CREATE OR ALTER FUNCTION security.fn_tenant_order_access(@TenantId INT)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS fn_tenant_order_access_result
    WHERE TRY_CONVERT(BIT, SESSION_CONTEXT(N'RlsBypass')) = 1
       OR IS_MEMBER(N'rls_policy_admin') = 1
       OR @TenantId = TRY_CONVERT(INT, SESSION_CONTEXT(N'TenantId'));
""");

            migrationBuilder.Sql(
                """
IF NOT EXISTS (
    SELECT 1
    FROM sys.security_policies
    WHERE name = N'TenantOrderIsolationPolicy'
      AND schema_id = SCHEMA_ID(N'security'))
BEGIN
    CREATE SECURITY POLICY security.TenantOrderIsolationPolicy
    ADD FILTER PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders,
    ADD BLOCK PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders AFTER INSERT,
    ADD BLOCK PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders AFTER UPDATE
    WITH (STATE = ON);
END
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
IF EXISTS (
    SELECT 1
    FROM sys.security_policies
    WHERE name = N'TenantOrderIsolationPolicy'
      AND schema_id = SCHEMA_ID(N'security'))
BEGIN
    DROP SECURITY POLICY security.TenantOrderIsolationPolicy;
END
""");

            migrationBuilder.Sql(
                """
IF OBJECT_ID(N'security.fn_tenant_order_access', N'IF') IS NOT NULL
BEGIN
    DROP FUNCTION security.fn_tenant_order_access;
END
""");

            migrationBuilder.DropTable(
                name: "TenantOrders",
                schema: "academy");
        }
    }
}
