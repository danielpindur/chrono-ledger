namespace ChronoLedger.Schema.Accounts;

public class AccountDto
{
    public Guid AccountId { get; set; }
    
    public string ExternalAccountId { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
}