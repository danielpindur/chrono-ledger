namespace ChronoLedger.Common.Cash;

public class Money : IEquatable<Money>
{
    private readonly decimal _value;
    private readonly CurrencyCode _currencyCode;
    
    private readonly int _hashCode;
    
    public Money(decimal value, CurrencyCode currencyCode)
    {
        _value = value;
        _currencyCode = currencyCode;

        _hashCode = value.GetHashCode() ^
                    StringComparer.InvariantCultureIgnoreCase.GetHashCode(_currencyCode.ToString());
    }
    
    public decimal Value => _value;
    
    public CurrencyCode CurrencyCode => _currencyCode;

    public override bool Equals(object? obj)
    {
        if (obj is not Money money) { return false; }
        
        return Equals(money);
    }

    public bool Equals(Money? other)
    {
        if (other is null) { return false; }

        return _currencyCode.Equals(other._currencyCode) && 
               _value.Equals(other._value);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
    
    public static bool operator ==(Money? left, Money? right)
    {
        if (left is null)
        {
            return right is null;
        }
        
        return left.Equals(right);
    }

    public static bool operator !=(Money left, Money right)
    { 
        return !(left == right);
    }

    public override string ToString()
    { 
        return $"{_value} {_currencyCode}";
    }
}