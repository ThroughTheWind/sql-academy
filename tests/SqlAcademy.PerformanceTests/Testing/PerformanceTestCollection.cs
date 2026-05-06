namespace SqlAcademy.PerformanceTests.Testing;

[CollectionDefinition(Name)]
public sealed class PerformanceTestCollection : ICollectionFixture<SqlServerFixture>
{
    public const string Name = "sql-academy-performance";
}