namespace UaCodes.Tests;

public class KatottgTests
{
    [Theory]
    [InlineData("UA74060050020095773", "74", "Чернігівська область")]   // село
    [InlineData("UA71080490000022110", "71", "Черкаська область")]
    [InlineData("UA80000000000093317", "80", "місто Київ")]
    public void Region_is_read_from_real_codes(string code, string regionCode, string regionName)
    {
        Assert.True(Katottg.HasValidFormat(code));
        Assert.Equal(regionCode, Katottg.RegionCode(code));
        Assert.Equal(regionName, Katottg.RegionName(code));
    }

    [Fact]
    public void Code_with_nonexistent_region_is_rejected()
    {
        const string code = "UA99060050020095773";

        Assert.False(Katottg.HasValidFormat(code));
        Assert.Null(Katottg.RegionCode(code));
        Assert.Null(Katottg.RegionName(code));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("UA7406005002009577")]    // 16 цифр
    [InlineData("ua74060050020095773")]
    [InlineData("UA 74060050020095773")]
    public void Anything_but_ua_and_17_digits_is_rejected(string? code)
    {
        Assert.False(Katottg.HasValidFormat(code));
    }
}
