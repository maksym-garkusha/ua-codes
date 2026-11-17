namespace UaCodes.Tests;

public class EdrpouTests
{
    [Theory]
    [InlineData("00100227")]   // Укренерго, код менший за 30000000
    [InlineData("03346331")]   // Київгаз
    [InlineData("20077720")]   // Нафтогаз
    [InlineData("42750837")]   // код між 30000000 і 60000000, зсунуті ваги
    public void Real_codes_are_valid(string code)
    {
        Assert.True(Edrpou.IsValid(code));
    }

    [Fact]
    public void Check_digit_comes_from_second_pass_when_first_gives_ten()
    {
        // синтетичний код: перша сума за модулем 11 дорівнює 10, друга дає 2
        Assert.True(Edrpou.IsValid("01007912"));
    }

    [Fact]
    public void Check_digit_is_zero_when_both_passes_give_ten()
    {
        Assert.True(Edrpou.IsValid("01435540"));
    }

    [Theory]
    [InlineData("00100228")]   // остання цифра змінена
    [InlineData("03346313")]   // дві цифри переставлені місцями
    [InlineData("42750838")]
    public void Codes_with_wrong_check_digit_are_invalid(string code)
    {
        Assert.False(Edrpou.IsValid(code));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData("3346331")]       // сім цифр, нулі загубилися
    [InlineData(" 00100227")]
    [InlineData("0010022A")]
    public void Anything_but_eight_digits_is_invalid(string? code)
    {
        Assert.False(Edrpou.IsValid(code));
    }

    [Fact]
    public void Span_overload_checks_a_slice_without_copying()
    {
        var line = "код:00100227;".AsSpan();

        Assert.True(Edrpou.IsValid(line.Slice(4, 8)));
    }

    [Theory]
    [InlineData("3346331", "03346331")]
    [InlineData(" 135390 ", "00135390")]
    [InlineData("42750837", "42750837")]
    public void Normalize_restores_leading_zeros(string raw, string expected)
    {
        Assert.Equal(expected, Edrpou.Normalize(raw));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("1234567890")]   // 10 цифр це вже РНОКПП
    [InlineData("35546472.")]
    public void Normalize_returns_null_for_other_things(string? raw)
    {
        Assert.Null(Edrpou.Normalize(raw));
    }
}
