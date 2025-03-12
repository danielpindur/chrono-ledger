using ChronoLedger.Common.Cash;

namespace ChronoLedger.Schema.Journals;

public class JournalEntryDto
{
    public long JournalEntryId { get; set; }
    
    public long JournalBatchId { get; set; }
    
    public DateOnly LedgerDate { get; set; }
    
    public bool IsVisible { get; set; }
    
    public Guid AccountId { get; set; }
    
    public decimal Amount { get; set; }
    
    public CurrencyCode CurrencyCode { get; set; }
    
    public string Notes { get; set; }
}