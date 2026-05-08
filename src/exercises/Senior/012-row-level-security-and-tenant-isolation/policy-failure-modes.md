# Policy Failure Modes

- a predicate function can make debugging harder because missing rows may look like data loss when the connection is simply carrying the wrong tenant context
- filter predicates help reads, but block predicates can break writes immediately if inserts, updates, or batch imports do not carry the expected tenant identity
- background jobs and reporting tools often need broader access, so an implicit bypass is weaker than an explicit role such as `rls_policy_admin`
- if the tenant key is not indexed and part of the real access pattern, the security policy can become a performance problem as well as a security feature
- startup migrations, data fixes, and support scripts need a deliberate plan for policy state, execution identity, and rollback instead of assuming the default path will still work
- a convincing sample can still become a false generalization if the team assumes every existing academy table is now tenant-ready without adding the same schema and runtime contracts