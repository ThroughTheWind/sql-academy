# Optional Solution Outline

One valid Lab 01 solution adds `sortBy=authorUserName` by extending the EF Core sort mapping to order on the author user name with an `Id` tie-breaker, then adds one focused assertion in [PostsEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs).

One valid Lab 02 solution adds `sortBy=side` by extending the Dapper sort whitelist to map that option to `t.Side`, preserving `ORDER BY {sortColumn} {sortDirection}, t.Id DESC`, then adds one focused assertion in [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs).

If either lab pushes you toward full-entity materialization or raw SQL fragments built from user input, back up. The intended shape keeps projection in EF Core and whitelist-based sorting in Dapper.