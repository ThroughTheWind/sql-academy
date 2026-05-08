# Hints

1. Separate true database-local recurring work from application-bound orchestration before you talk about schedules.
2. Ask what happens if the same job starts twice before the first run finishes.
3. A release gate often belongs to the operator and the change window, not to a blind recurring schedule.
