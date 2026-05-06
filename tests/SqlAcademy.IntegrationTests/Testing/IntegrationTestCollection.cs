namespace SqlAcademy.IntegrationTests.Testing;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<SqlServerFixture>
{
    public const string Name = "sql-academy-integration";
}