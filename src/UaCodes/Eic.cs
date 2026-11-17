using System.Diagnostics.CodeAnalysis;

namespace UaCodes;

/// <summary>
/// EIC, код об'єкта енергоринку за схемою ENTSO-E: 16 символів (цифри, латинські
/// великі літери, дефіс), останній контрольний.
/// </summary>
public static class Eic
{
    /// <summary>Довжина коду.</summary>
    public const int Length = 16;

    /// <summary>Перевіряє тільки форму: 16 символів із цифр, латинських великих літер і дефіса.</summary>
    public static bool HasValidFormat([NotNullWhen(true)] string? code) =>
        code is not null && HasValidFormat(code.AsSpan());

    /// <inheritdoc cref="HasValidFormat(string?)"/>
    public static bool HasValidFormat(ReadOnlySpan<char> code)
    {
        if (code.Length != Length) return false;

        foreach (char c in code)
        {
            bool allowed = char.IsAsciiDigit(c) || char.IsAsciiLetterUpper(c) || c == '-';
            if (!allowed) return false;
        }
        return true;
    }

    /// <summary>Перевіряє форму і контрольний символ.</summary>
    public static bool IsValid([NotNullWhen(true)] string? code) =>
        code is not null && IsValid(code.AsSpan());

    /// <inheritdoc cref="IsValid(string?)"/>
    public static bool IsValid(ReadOnlySpan<char> code)
    {
        if (!HasValidFormat(code)) return false;

        // перші 15 символів множаться на ваги 16, 15, ... 2
        int sum = 0;
        for (int i = 0; i < Length - 1; i++)
            sum += Value(code[i]) * (Length - i);

        int check = 36 - (sum - 1) % 37;
        return check == Value(code[Length - 1]);
    }

    /// <summary>Тип об'єкта за третім символом.</summary>
    /// <returns>Тип або <c>null</c>, якщо код не проходить <see cref="IsValid(string?)"/> чи третій символ невідомий.</returns>
    public static EicKind? Kind(string? code)
    {
        if (!IsValid(code)) return null;

        return code[2] switch
        {
            'X' => EicKind.MarketParticipant,
            'Y' => EicKind.Area,
            'Z' => EicKind.MeteringPoint,
            'W' => EicKind.Resource,
            'T' => EicKind.Tieline,
            'V' => EicKind.Location,
            'A' => EicKind.Substation,
            _ => null,
        };
    }

    // 0-9 це 0..9, A-Z це 10..35, дефіс 36
    private static int Value(char c)
    {
        if (char.IsAsciiDigit(c)) return c - '0';
        if (c == '-') return 36;
        return c - 'A' + 10;
    }
}
