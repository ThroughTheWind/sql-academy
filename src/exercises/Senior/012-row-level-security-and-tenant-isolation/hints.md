# Hints

## Hint 1

Do not start with the predicate function. Start with the question: which schema contract would a real tenant boundary need before the function could mean anything?

## Hint 2

Look at `starter-policy.sql` and ask which component owns the tenant identity: the table, the session, or the operator bypass. If one of those is vague, the design is still weak.

## Hint 3

The interesting question is no longer “is there any sample at all?” The better question is “why does this sample work for `academy.TenantOrders`, and what would still have to change before you copied it onto a core academy table?”