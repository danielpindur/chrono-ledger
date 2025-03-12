using System.ComponentModel.DataAnnotations;

namespace ChronoLedger.Schema.Cash;

/// <summary>
/// Database object representing a journal batch.
/// </summary>
public class JournalBatchDto
{
    /// <summary>
    /// The unique identifier of the journal batch.
    /// </summary>
    [Key]
    public long JournalBatchId { get; set; }
    
    /// <summary>
    /// The date and time when the journal batch was created.
    /// </summary>
    [Required]
    public DateTime CreatedDateTime { get; set; }
    
    /// <summary>
    /// The user who created the journal batch.
    /// </summary>
    [Required]
    public Guid CreatedByUserId { get; set; }
    
    /// <summary>
    /// The JSON representation of the journals in the batch.
    /// </summary>
    [Required]
    public string JournalsJson { get; set; }
}