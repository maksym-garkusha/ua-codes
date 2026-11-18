namespace UaCodes.Tests;

public class EicTests
{
    [Theory]
    [InlineData("10X1001C--00001X")]   // Укренерго
    [InlineData("21X0000000013279")]   // Укртрансгаз
    [InlineData("24X-PJSC-DOENE-K")]   // з дефісами
    [InlineData("10Y1001A1001A869")]   // код зони
    public void Real_codes_are_valid(string code)
    {
        Assert.True(Eic.IsValid(code));
    }

    [Theory]
    [InlineData("62X1580751879965")]   // з реєстру НКРЕКП, контрольний символ не сходиться
    [InlineData("56X93000011558OL")]   // літера O замість нуля
    public void Codes_with_wrong_check_character_keep_valid_format(string code)
    {
        Assert.True(Eic.HasValidFormat(code));
        Assert.False(Eic.IsValid(code));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("62z000000000123Y")]   // мала літера, синтетичний код
    [InlineData("62X863954335972")]    // 15 символів
    [InlineData("10Y1001А1001A869")]   // кирилична А замість латинської
    public void Codes_with_wrong_format_are_rejected(string? code)
    {
        Assert.False(Eic.HasValidFormat(code));
    }

    [Theory]
    [InlineData("10X1001C--00001X", EicKind.MarketParticipant)]
    [InlineData("10Y1001A1001A869", EicKind.Area)]
    // далі синтетичні коди, контрольний символ порахований
    [InlineData("62Z000000000123Y", EicKind.MeteringPoint)]
    [InlineData("62W0000000001232", EicKind.Resource)]
    [InlineData("62T0000000001237", EicKind.Tieline)]
    [InlineData("62V000000000123G", EicKind.Location)]
    [InlineData("62A000000000123E", EicKind.Substation)]
    public void Kind_is_read_from_third_character(string code, EicKind expected)
    {
        Assert.Equal(expected, Eic.Kind(code));
    }

    [Theory]
    [InlineData("62B0000000001230")]   // правильний контрольний символ, але B не означає жодного типу
    [InlineData("62X1580751879965")]
    public void Kind_is_null_for_unknown_letter_or_invalid_code(string code)
    {
        Assert.Null(Eic.Kind(code));
    }
}
