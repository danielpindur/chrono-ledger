using System.ComponentModel.DataAnnotations;
using ChronoLedger.Common.Cash;

namespace ChronoLedger.Schema.Cash;

/// <summary>
/// Database object representing a transaction.
/// </summary>
public class TransactionDto
{
    /// <summary>
    /// The unique identifier of the transaction.
    /// </summary>
    [Key]
    public long TransactionId { get; set; }
    
    /// <summary>
    /// The unique identifier of the journal batch that the transaction belongs to.
    /// </summary>
    [Required]
    public long JournalBatchId { get; set; }
    
    /// <summary>
    /// The date of the transaction.
    /// </summary>
    [Required]
    public DateOnly LedgerDate { get; set; }
    
    /// <summary>
    /// The type of the transaction.
    /// </summary>
    public bool IsVisible { get; set; }
    
    /// <summary>
    /// The account that the transaction is associated with.
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }
    
    /// <summary>
    /// The amount of the transaction.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// The currency code of the transaction.
    /// </summary>
    [Required]
    public CurrencyCode CurrencyCodeId { get; set; }
}