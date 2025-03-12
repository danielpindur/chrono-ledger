using System.ComponentModel.DataAnnotations;

namespace ChronoLedger.Schema.Users;

/// <summary>
/// Database object representing a user.
/// </summary>
public class UserDto
{
    /// <summary>
    /// The unique internal identifier of the user.
    /// </summary>
    [Key]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The unique external identifier of the user - allows for integration with external systems.
    /// </summary>
    [Required]
    public required string ExternalUserId { get; set; }
    
    /// <summary>
    /// The date and time when the user was created.
    /// </summary>
    [Required]
    public DateTime CreatedDateTime { get; set; }
}