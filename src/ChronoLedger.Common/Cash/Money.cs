namespace ChronoLedger.Common.Cash;

public class Money : IEquatable<Money>
{
    private readonly decimal _amount;
    private readonly CurrencyCode _currencyCode;
    
    private readonly int _hashCode;
    
    public Money(decimal amount, CurrencyCode currencyCode)
    {
        _amount = amount;
        _currencyCode = currencyCode;

        _hashCode = amount.GetHashCode() ^
                    StringComparer.InvariantCultureIgnoreCase.GetHashCode(_currencyCode.ToString());
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Money money) { return false; }
        
        return Equals(money);
    }

    public bool Equals(Money? other)
    {
        if (other is null) { return false; }

        return _currencyCode.Equals(other._currencyCode) && 
               _amount.Equals(other._amount);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
}