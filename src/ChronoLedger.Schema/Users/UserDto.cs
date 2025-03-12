namespace ChronoLedger.Schema.Users;

public class UserDto
{
    public Guid UserId { get; set; }
    
    public required string ExternalUserId { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
}