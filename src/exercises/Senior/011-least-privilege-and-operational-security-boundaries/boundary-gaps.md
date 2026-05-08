# Boundary Gaps

## What Still Blocks A Credible Least-Privilege Story

- the current API and worker runtime paths do not distinguish application permissions from full database administration
- automatic startup migrations make local learning deterministic, but they also blur the boundary between deployment authority and request-serving runtime authority
- the design-time factory still assumes privileged fallback access instead of forcing explicit migration credentials
- the current operator-admin surfaces do not yet show who owns credential rotation, separation of duties, or when a privileged default must stop being reused
