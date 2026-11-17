namespace UaCodes.Tests;

public class IbanTests
{
    // приклад із роз'яснень НБУ щодо структури IBAN
    private const string NbuExample = "UA213223130000026007233566001";

    [Theory]
    [InlineData(NbuExample)]
    [InlineData("UA21 3223 1300 0002 6007 2335 6600 1")]
    [InlineData("ua213223130000026007233566001")]
    [InlineData("UA253052990000026007000000001")]   // синтетичний, контрольні цифри пораховані
    public void Correct_ibans_are_valid(string iban)
    {
        Assert.True(Iban.IsValid(iban));
    }

    [Theory]
    [InlineData("UA213223130000026007233566002")]   // остання цифра змінена
    [InlineData("UA213223130000026007233656001")]   // дві цифри переставлені місцями
    [InlineData("UA123223130000026007233566001")]   // контрольні цифри переставлені
    public void Ibans_with_wrong_check_number_are_invalid(string iban)
    {
        Assert.False(Iban.IsValid(iban));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("DE89370400440532013000")]          // правильний німецький IBAN
    [InlineData("UA21322313000002600723356600")]    // 28 символів
    [InlineData("UA21322313000002600723356600A")]
    [InlineData("УА213223130000026007233566001")]   // кирилиця на початку
    public void Anything_but_ukrainian_iban_is_invalid(string? iban)
    {
        Assert.False(Iban.IsValid(iban));
    }

    [Fact]
    public void Normalize_removes_spaces_and_uppercases()
    {
        Assert.Equal(NbuExample, Iban.Normalize(" ua21 3223 1300 0002 6007 2335 6600 1 "));
    }

    [Fact]
    public void Normalize_returns_null_for_foreign_iban()
    {
        Assert.Null(Iban.Normalize("DE89370400440532013000"));
    }

    [Fact]
    public void Bank_code_is_six_digits_after_check_digits()
    {
        Assert.Equal("322313", Iban.BankCode("UA21 3223 1300 0002 6007 2335 6600 1"));
    }

    [Fact]
    public void Bank_code_is_null_when_check_number_fails()
    {
        Assert.Null(Iban.BankCode("UA213223130000026007233566002"));
    }
}
