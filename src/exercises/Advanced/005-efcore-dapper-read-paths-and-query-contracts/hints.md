# Hints

## Start From The Contract

- write down the current filters, sort options, pagination behavior, and projection columns before you edit code
- restate the expected SQL shape in plain English before you argue about the library choice

## EF Core Lab

- keep `AsNoTracking()` in place for the read path
- extend the sorting whitelist instead of reshaping the query after materialization
- add one focused endpoint test for the new contract instead of broadening the whole test file

## Dapper Lab

- change the sort whitelist, not the SQL text from raw user input
- keep `t.Id DESC` as the deterministic tie-breaker even when you add another sort column
- extend one focused trade test so the contract change stays observable