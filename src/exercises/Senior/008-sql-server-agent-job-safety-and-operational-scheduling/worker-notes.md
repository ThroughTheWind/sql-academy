# Worker Notes

## Current Worker Boundary

- `SqlAcademy.Worker` registers `DatabaseTelemetryWorker` with `AddHostedService<DatabaseTelemetryWorker>()`
- the worker uses the application host, dependency injection, configuration, and shared observability setup
- `DatabaseTelemetryWorker` reads `Worker:PollIntervalSeconds` from configuration and enforces a minimum interval of 5 seconds

## Why That Matters

- this polling loop already lives inside the application deployment and telemetry boundary
- moving it to SQL Server Agent would change who owns failure visibility, configuration, and runtime dependency wiring
- a recurring schedule is not automatically a better fit for SQL Server Agent just because the underlying work touches the database
