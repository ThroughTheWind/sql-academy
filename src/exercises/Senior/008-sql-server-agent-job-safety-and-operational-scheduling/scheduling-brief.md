# Scheduling Brief

- proposed goal: move more recurring operational work into SQL Server Agent before the next release window
- main concern: the team is treating every periodic task as if it has the same safety, retry, and observability needs
- decision question: which tasks are truly database-local recurring work, which belong in the app-hosted worker, and which should stay explicit release or operator steps?
