using System.ComponentModel.DataAnnotations;

namespace ChronoLedger.Schema.Accounts;

/// <summary>
/// Database object representing an account.
/// </summary>
public class AccountDto
{
    /// <summary>
    /// The unique internal identifier of the account.
    /// </summary>
    [Key]
    public Guid AccountId { get; set; }
    
    /// <summary>
    /// The unique external identifier of the account - allows for integration with external systems.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string ExternalAccountId { get; set; }
    
    /// <summary>
    /// The date and time when the account was created.
    /// </summary>
    [Required]
    public DateTime CreatedDateTime { get; set; }
}