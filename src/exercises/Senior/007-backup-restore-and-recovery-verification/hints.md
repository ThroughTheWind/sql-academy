# Hints

1. Start with the RPO before you talk about the restore commands. If the newest log backup is already too old, the release posture is weaker than it sounds.
2. Treat `RESTORE VERIFYONLY` as useful but incomplete evidence. Ask what it does not prove.
3. A believable restore fallback ends with a restored system doing useful work, not with backup files merely existing on disk.
