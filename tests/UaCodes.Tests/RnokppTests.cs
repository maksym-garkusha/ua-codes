namespace UaCodes.Tests;

// Усі номери тут синтетичні: перші 9 цифр довільні, десята порахована за алгоритмом.
public class RnokppTests
{
    [Theory]
    [InlineData("1234567899")]
    [InlineData("3141592650")]
    [InlineData("2718281821")]
    public void Numbers_with_matching_check_digit_are_valid(string number)
    {
        Assert.True(Rnokpp.IsValid(number));
    }

    [Theory]
    [InlineData("9000000002")]   // зважена сума -9
    [InlineData("1000000000")]   // зважена сума -1, остача 10, контрольна 0
    public void Negative_weighted_sum_is_reduced_to_positive_remainder(string number)
    {
        Assert.True(Rnokpp.IsValid(number));
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("2178281821")]   // дві цифри переставлені місцями
    public void Numbers_with_wrong_check_digit_are_invalid(string number)
    {
        Assert.False(Rnokpp.IsValid(number));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123456789")]
    [InlineData("12345678999")]
    [InlineData("12345 6789")]
    [InlineData("00100227")]     // це ЄДРПОУ
    public void Anything_but_ten_digits_is_invalid(string? number)
    {
        Assert.False(Rnokpp.IsValid(number));
    }
}
