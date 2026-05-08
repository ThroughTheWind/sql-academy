# Failover Brief

- planned maintenance window: patch the primary SQL Server host before the next high-order-volume period
- stated fallback: "fail over the write path to the warm secondary if the primary becomes unavailable during maintenance"
- target recovery point objective: 5 minutes
- target recovery time objective: 10 minutes
- main question: does the current failover posture make that fallback believable, or should the window be blocked until the HA or DR path is verified more concretely?
