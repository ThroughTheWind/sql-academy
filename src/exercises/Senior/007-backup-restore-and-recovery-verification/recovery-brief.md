# Recovery Brief

- planned change window: enforce a stricter write-path contract on `academy.Orders` after the compatible application build is already deployed
- stated fallback: "restore `LearningDb` if the release goes bad"
- target recovery point objective: 15 minutes
- target recovery time objective: 30 minutes
- main question: does the current backup and restore posture make that fallback believable, or should the release be blocked until recovery verification is stronger?
