namespace ChronoLedger.Schema.Journals;

public class JournalBatch
{
    public long JournalBatchId { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    
    public string JournalsJson { get; set; }
}