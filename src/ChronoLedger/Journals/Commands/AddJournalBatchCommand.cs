using ChronoLedger.Common.Cash;

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

    /// <summary>
    /// The date of the ledger for the journal batch.
    /// </summary>
    public required DateTime LedgerDate { get; set; }
    
    /// <summary>
    /// The journal entries to add to the journal batch.
    /// </summary>
    public required List<JournalEntry> JournalEntries { get; set; }
    
    public class JournalEntry
    {
        /// <summary>
        /// The external identifier of the account to which the journal entry is posted.
        /// </summary>
        public required string ExternalAccountId { get; set; }
        
        /// <summary>
        /// The amount and currency of the journal entry.
        /// </summary>
        public required Money Amount { get; set; }
        
        /// <summary>
        /// Narrative for the journal entry.
        /// </summary>
        public string? Note { get; set; }
    }
}