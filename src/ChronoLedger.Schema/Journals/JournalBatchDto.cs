using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChronoLedger.Schema.Users;

namespace ChronoLedger.Schema.Journals;

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
    [ForeignKey(nameof(UserDto))]
    public Guid CreatedByUserId { get; set; }
    
    // public string JournalsJson { get; set; }
}