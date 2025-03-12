using System.Text;

namespace ChronoLedger.Common.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string source, string compareTo)
    {
        return source.IsNullOrEmpty()
            ? compareTo.IsNullOrEmpty()
            : source.Equals(compareTo, StringComparison.OrdinalIgnoreCase);
    }
    
    public static string ToSnakeCase(this string input)
    {
        if (input.IsNullOrEmpty())
        {
            return input;
        }

        var result = new StringBuilder(input.Length + 10);
        result.Append(char.ToLowerInvariant(input[0]));
        
        for (var i = 1; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c))
            {
                result.Append('_');
                result.Append(char.ToLowerInvariant(c));
            }
            else
            {
                result.Append(c);
            }
        }
        
        return result.ToString();
    }
}