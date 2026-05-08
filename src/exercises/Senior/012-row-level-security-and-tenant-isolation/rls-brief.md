# RLS Brief

Product wants to keep a shared SQL Server database while onboarding multiple customer tenants faster. The current assumption is that application filters on user-facing queries are enough because the code already knows who the caller is.

Security and operations disagree. They point out that a shared-database tenant model needs a durable tenant key in the schema, a repeatable way to stamp each SQL connection with tenant context, and an explicit rule for reporting, support, background jobs, and maintenance work that sometimes needs broader access.

Your job is to decide whether SQL Server row-level security is the right next boundary here or whether the current platform still lacks the prerequisites that would make RLS credible instead of performative.