-- Runnable lab companion: this policy shape now matches the dedicated
-- academy.TenantOrders sample plus the app-side session-context plumbing.

EXEC sys.sp_set_session_context @key = N'TenantId', @value = 101;
EXEC sys.sp_set_session_context @key = N'RlsBypass', @value = 0;
GO

CREATE SCHEMA security;
GO

CREATE OR ALTER FUNCTION security.fn_tenant_order_access(@TenantId INT)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS fn_tenant_access_result
    WHERE TRY_CONVERT(BIT, SESSION_CONTEXT(N'RlsBypass')) = 1
       OR IS_MEMBER(N'rls_policy_admin') = 1
       OR @TenantId = TRY_CONVERT(INT, SESSION_CONTEXT(N'TenantId'))
GO

CREATE SECURITY POLICY security.TenantOrderIsolationPolicy
ADD FILTER PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders,
ADD BLOCK PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders AFTER INSERT,
ADD BLOCK PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders AFTER UPDATE
WITH (STATE = ON);
GO

-- Review prompts:
-- 1. Which HTTP or worker boundary is responsible for TenantId now?
-- 2. Why is RlsBypass still explicit instead of falling back to broad runtime privilege?
-- 3. Which sample rows disappear when you switch from TenantId 101 to 202?
-- 4. Which focused tests prove both EF Core and Dapper are carrying session context?