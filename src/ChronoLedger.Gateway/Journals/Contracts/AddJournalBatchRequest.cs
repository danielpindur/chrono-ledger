using System.ComponentModel.DataAnnotations;

namespace ChronoLedger.Gateway.Journals.Contracts;

public class AddJournalBatchRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public required string ExternalUserId { get; set; }
}