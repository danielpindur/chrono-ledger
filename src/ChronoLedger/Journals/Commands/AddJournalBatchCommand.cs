namespace ChronoLedger.Journals.Commands;

/// <summary>
/// Command to add a journal batch.
/// </summary>
public class AddJournalBatchCommand
{
    /// <summary>
    /// The external identifier of the user who created the journal batch.
    /// </summary>
    public required string ExternalUserId { get; set; }
}