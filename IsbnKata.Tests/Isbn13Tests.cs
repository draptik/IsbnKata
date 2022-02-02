using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Xunit;

namespace IsbnKata.Tests;

public class Isbn13Tests
{
    [Theory]
    [InlineData("978-0-465-02656-2", "9780465026562", true)]
    [InlineData("978-0-465-02656-3", "invalid isbn13 checksum", false)]
    public void Isbn13_validation_works(string input, string expected, bool isValid)
    {
        var result = Isbn13.Create(input);

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