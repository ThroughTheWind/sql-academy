# Hints

1. Start with the current `Select(post => new PostListItem(...))` shape before inventing an unsafe experiment.
2. If you cannot inspect the generated SQL directly from the current service, reproduce the same LINQ shape in a temporary diagnostic query and capture it there.
3. A convincing N+1 experiment usually involves materializing a parent list first and then performing one extra query or relationship access per row.
4. The goal is not to leave a broken refactor in the repository. The goal is to prove what would make the path regress and how you would catch it.