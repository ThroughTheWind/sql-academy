using System.Diagnostics;

namespace SqlAcademy.Persistence.Diagnostics;

public static class PersistenceDiagnostics
{
    public const string ActivitySourceName = "SqlAcademy.Persistence";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}