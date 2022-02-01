using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Xunit;

namespace IsbnKata.Tests;

public class Isbn10Tests
{
    [Theory]
    [InlineData("0465026567", "0465026567", true)]
    [InlineData("046502656X", "isbn10 invalid checksum (non-x)", false)]
    public void Isbn10_validation_works(string input, string expected, bool isValid)
    {
        var result = Isbn10.Create(input);

        if (isValid)
        {
            result.Should().BeSuccess();
            result.Value.Should().Be(expected);
        }
        else
        {
            result.Should().BeFailure();
            result.Error.Should().Be(expected);
        }
    }
}