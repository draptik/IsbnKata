using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace IsbnKata;

public class Isbn10 : ValueObject<Isbn10>
{
    public string Value { get; }

    private Isbn10(string input) => Value = input;

    public static Result<Isbn10> Create(string s)
    {
        var onlyValidStrings = Regex.Replace(s, @"[^0-9|X|x]", "");
        var result = CheckXOnlyAtLastPosition(onlyValidStrings)
            .Bind(CheckSum)
            .Finally(x => x.IsSuccess
                ? new Isbn10(x.Value)
                : Result.Failure<Isbn10>(x.Error));

        return result;
    }

    private static Result<string> CheckXOnlyAtLastPosition(string s) =>
        Regex.Match(s, @"\d{9}(?:\d|X|x)").Success 
            ? s 
            : Result.Failure<string>("invalid isbn10 format. 9 numbers followed by number or X");

    private static Result<string> CheckSum(string s)
    {
        var oneToNine = s[..9];
        var actualChecksum = s[9..];

        var sum = oneToNine
            .ToCharArray()
            .Select(x => int.Parse((string)x.ToString()))
            .Select((x, i) => (i + 1) * x)
            .Sum();

        if (sum % 11 == 10)
        {
            return actualChecksum.ToUpperInvariant() == "X" 
                ? s 
                : Result.Failure<string>("isbn10 invalid checksum (x)");
        }

        return (sum % 11).ToString() == actualChecksum 
            ? s 
            : Result.Failure<string>("isbn10 invalid checksum (non-x)");
    }
    
    protected override bool EqualsCore(Isbn10 other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();
    
    public static implicit operator string(Isbn10 x) => x.Value;
}