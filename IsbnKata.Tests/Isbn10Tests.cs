using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Xunit;

namespace IsbnKata.Tests;

public class Isbn10Tests
{
    //
    [Theory]
    [InlineData("0465026567", "0465026567", true)]
    [InlineData("046502656X", "isbn10 invalid checksum (non-x)", false)]
    [InlineData("1", "invalid isbn10 format. 9 numbers followed by number or X", false)]
    [InlineData("043942089X", "043942089X", true)]
    [InlineData("0439420890", "isbn10 invalid checksum (x)", false)]
    public void Isbn10_validation_works(string input, string expected, bool isValid)
    {
        var result = Isbn10.Create(input);

        if (isValid)
        {
            result.Should().BeSuccess();
            ((string)result.Value).Should().Be(expected);
        }
        else
        {
            result.Should().BeFailure();
            result.Error.Should().Be(expected);
        }
    }
}