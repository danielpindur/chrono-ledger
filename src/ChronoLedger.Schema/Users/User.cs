namespace ChronoLedger.Schema.Users;

public class User
{
    public Guid UserId { get; set; }
    
    public string ExternalUserId { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
}