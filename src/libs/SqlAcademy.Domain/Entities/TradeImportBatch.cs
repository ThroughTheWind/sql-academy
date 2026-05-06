namespace SqlAcademy.Domain.Entities;

public sealed class TradeImportBatch
{
    public int Id { get; set; }

    public DateTime ProcessedUtc { get; set; }

    public bool DryRun { get; set; }

    public int SubmittedCount { get; set; }

    public int ValidatedCount { get; set; }

    public int DuplicateCount { get; set; }

    public int ReadyToPublishCount { get; set; }

    public int ImportedCount { get; set; }

    public int RejectedCount { get; set; }

    public List<TradeImportBatchRow> Rows { get; } = [];
}