using System.Linq;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace IsbnKata.Tests;

public class Isbn13 : ValueObject<Isbn13>
{
    public string Value { get; }
    
    private Isbn13(string s) => Value = s;

    public static Result<Isbn13> Create(string s)
    {
        var numberString = RemoveNonNumbers(s);
        return CheckLength(numberString)
            .Bind(CheckSum)
            .Finally(x => x.IsSuccess
                ? new Isbn13(x.Value)
                : Result.Failure<Isbn13>(x.Error));
    }

    private static string RemoveNonNumbers(string s) => 
        Regex.Replace(s, @"[^0-9]", "");

    private static bool HasCorrectLength(string s) =>
        s.Length == 13;

    private static Result<string> CheckLength(string s) =>
        HasCorrectLength(s)
            ? s
            : Result.Failure<string>("incorrect length");

    private static Result<string> CheckSum(string s)
    {
        var numbers = s
            .ToCharArray()
            .Select(x => int.Parse(x.ToString()))
            .ToList();
        
        var checksum = numbers.Last();
        var first12Numbers = numbers.Take(12);

        var sum = first12Numbers
            .Select(MultiplyEvenBy3)
            .Sum();

        var rawActualCheckSum = sum % 10;
        var actualCheckSum = rawActualCheckSum != 0 
            ? 10 - rawActualCheckSum 
            : rawActualCheckSum;

        return actualCheckSum == checksum 
            ? s 
            : Result.Failure<string>("invalid isbn13 checksum");
    }

    private static int MultiplyEvenBy3(int number, int index) => 
        index % 2 != 0 
            ? number * 3 
            : number;

    protected override bool EqualsCore(Isbn13 other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();
}