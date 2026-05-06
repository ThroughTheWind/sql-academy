using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Entities;
using SqlAcademy.IntegrationTests.Testing;

namespace SqlAcademy.IntegrationTests.Transactions;

[Collection(IntegrationTestCollection.Name)]
public sealed class TransactionRollbackTests(SqlServerFixture databaseFixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await databaseFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Explicit_transaction_rollback_does_not_persist_changes()
    {
        await using var dbContext = databaseFixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.Users.Add(new User
        {
            UserName = "rollback-check",
            Email = "rollback-check@sqlacademy.local",
            CreatedUtc = DateTime.UtcNow,
        });

        await dbContext.SaveChangesAsync();
        await transaction.RollbackAsync();

        await using var verificationContext = databaseFixture.CreateDbContext();
        var exists = await verificationContext.Users.AnyAsync(user => user.UserName == "rollback-check");

        Assert.False(exists);
    }
}