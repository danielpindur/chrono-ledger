using System.ComponentModel.DataAnnotations;

namespace ChronoLedger.Schema.Cash;

/// <summary>
/// Database object representing a transaction note.
/// </summary>
public class TransactionNoteDto
{
    /// <summary>
    /// The unique identifier of the transaction the note is associated with.
    /// </summary>
    [Key]
    public long TransactionId { get; set; }
    
    /// <summary>
    /// The note associated with the transaction.
    /// </summary>
    [Required]
    public required string Note { get; set; }
}