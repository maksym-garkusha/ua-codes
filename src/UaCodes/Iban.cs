using System.Diagnostics.CodeAnalysis;

namespace UaCodes;

/// <summary>
/// Український IBAN: <c>UA</c>, 2 контрольні цифри, 6 цифр коду банку (МФО) і 19 цифр рахунку,
/// разом 29 символів. Пробіли між групами й малі літери допускаються.
/// </summary>
public static class Iban
{
    /// <summary>Довжина IBAN без пробілів.</summary>
    public const int Length = 29;

    private const int BankCodeStart = 4;
    private const int BankCodeLength = 6;

    /// <summary>Перевіряє форму і контрольне число за ISO 13616 (остача від ділення на 97 дорівнює 1).</summary>
    public static bool IsValid([NotNullWhen(true)] string? iban) =>
        iban is not null && IsValid(iban.AsSpan());

    /// <inheritdoc cref="IsValid(string?)"/>
    public static bool IsValid(ReadOnlySpan<char> iban)
    {
        Span<char> compact = stackalloc char[Length];
        return TryCompact(iban, compact) && Mod97(compact) == 1;
    }

    /// <summary>Прибирає пробіли й переводить у верхній регістр. Контрольне число не перевіряється.</summary>
    /// <returns>29 символів або <c>null</c>, якщо за формою це не український IBAN.</returns>
    public static string? Normalize(string? iban)
    {
        if (iban is null) return null;

        Span<char> compact = stackalloc char[Length];
        return TryCompact(iban, compact) ? new string(compact) : null;
    }

    /// <summary>Код банку (МФО), 6 цифр після контрольних.</summary>
    /// <returns>МФО або <c>null</c>, якщо IBAN не проходить <see cref="IsValid(string?)"/>.</returns>
    public static string? BankCode(string? iban)
    {
        if (iban is null) return null;

        Span<char> compact = stackalloc char[Length];
        if (!TryCompact(iban, compact) || Mod97(compact) != 1) return null;

        return new string(compact.Slice(BankCodeStart, BankCodeLength));
    }

    // Збирає IBAN без пробілів у compact і перевіряє, що це UA і 27 цифр.
    private static bool TryCompact(ReadOnlySpan<char> raw, Span<char> compact)
    {
        int length = 0;
        foreach (char c in raw)
        {
            if (c == ' ') continue;
            if (length == Length) return false;
            compact[length++] = char.ToUpperInvariant(c);
        }

        return length == Length
            && compact[0] == 'U'
            && compact[1] == 'A'
            && !compact[2..].ContainsAnyExceptInRange('0', '9');
    }

    // Перші 4 символи йдуть у кінець, літери стають числами A=10 ... Z=35.
    // Остача рахується по одному символу, тож 31-значне число ніде не тримається цілим.
    private static int Mod97(ReadOnlySpan<char> iban)
    {
        int remainder = 0;
        foreach (char c in iban[4..]) remainder = Append(remainder, c);
        foreach (char c in iban[..4]) remainder = Append(remainder, c);
        return remainder;
    }

    private static int Append(int remainder, char c) =>
        char.IsAsciiDigit(c)
            ? (remainder * 10 + (c - '0')) % 97
            : (remainder * 100 + (c - 'A' + 10)) % 97;
}
