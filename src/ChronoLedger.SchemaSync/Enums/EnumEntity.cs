namespace ChronoLedger.SchemaSync.Enums;

public abstract class EnumEntity<TEnum> where TEnum : Enum
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
}