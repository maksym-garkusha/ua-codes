using System.Diagnostics.CodeAnalysis;

namespace UaCodes;

/// <summary>Код ЄДРПОУ юридичної особи: 8 цифр, остання контрольна.</summary>
public static class Edrpou
{
    /// <summary>Кількість цифр у коді.</summary>
    public const int Length = 8;

    private static ReadOnlySpan<int> Weights => [1, 2, 3, 4, 5, 6, 7];

    // для кодів від 30000000 до 60000000 ваги зсунуті
    private static ReadOnlySpan<int> ShiftedWeights => [7, 1, 2, 3, 4, 5, 6];

    /// <summary>
    /// Доводить код до 8 цифр. Excel зберігає код як число і з'їдає нулі на початку,
    /// тому "3346331" у таблиці насправді "03346331". Контрольний розряд не перевіряється.
    /// </summary>
    /// <returns>8 цифр або <c>null</c>, якщо рядок не схожий на код.</returns>
    public static string? Normalize(string? raw)
    {
        if (raw is null) return null;

        var digits = raw.AsSpan().Trim();
        if (digits.IsEmpty || digits.Length > Length || digits.ContainsAnyExceptInRange('0', '9'))
            return null;

        return digits.ToString().PadLeft(Length, '0');
    }

    /// <summary>
    /// Перевіряє, що це рівно 8 цифр і контрольний розряд сходиться.
    /// Код без нулів на початку спершу треба пропустити через <see cref="Normalize"/>.
    /// </summary>
    public static bool IsValid([NotNullWhen(true)] string? code) =>
        code is not null && IsValid(code.AsSpan());

    /// <inheritdoc cref="IsValid(string?)"/>
    public static bool IsValid(ReadOnlySpan<char> code)
    {
        if (code.Length != Length || code.ContainsAnyExceptInRange('0', '9')) return false;

        int number = 0;
        foreach (char c in code)
            number = number * 10 + (c - '0');

        var weights = number < 30000000 || number > 60000000 ? Weights : ShiftedWeights;

        int check = WeightedSum(code, weights, 0) % 11;
        if (check == 10)
        {
            // друга спроба з вагами, більшими на 2
            check = WeightedSum(code, weights, 2) % 11;
            if (check == 10) check = 0;
        }

        return check == code[Length - 1] - '0';
    }

    private static int WeightedSum(ReadOnlySpan<char> code, ReadOnlySpan<int> weights, int shift)
    {
        int sum = 0;
        for (int i = 0; i < Length - 1; i++)
            sum += (code[i] - '0') * (weights[i] + shift);
        return sum;
    }
}
