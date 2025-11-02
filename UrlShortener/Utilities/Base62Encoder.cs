using System.Text;

namespace UrlShortener.Utilities;

public static class Base62Encoder
{
    private static readonly string _chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly int _base = _chars.Length;

    public static string Encode(long number)
    {
        if (number == 0) return _chars[0].ToString();

        if (number < 0)
        {
            number = -number;
        }

        var result = new StringBuilder();
        while (number > 0)
        {
            long remainder = number % _base;
            if (remainder < 0)
            {
                remainder += _base;
            }
            result.Insert(0, _chars[(int)remainder]);
            number /= _base;
        }
        return result.ToString();
    }

    public static long Decode(string encoded)
    {
        if (string.IsNullOrEmpty(encoded))
            throw new ArgumentException("Encoded string cannot be null or empty", nameof(encoded));

        long result = 0;
        long multiplier = 1;
        foreach (char c in encoded.Reverse())
        {
            int index = _chars.IndexOf(c);
            if (index == -1)
                throw new ArgumentException($"Invalid character '{c}' in encoded string", nameof(encoded));
            
            result += multiplier * index;
            multiplier *= _base;
        }
        return result;
    }
}