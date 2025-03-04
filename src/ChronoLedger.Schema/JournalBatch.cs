namespace ChronoLedger.Schema;

public class JournalBatch
{
    public int JournalBatchId { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
    
    public string JournalsJson { get; set; }
}