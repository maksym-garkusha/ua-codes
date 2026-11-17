using System.Diagnostics.CodeAnalysis;

namespace UaCodes;

/// <summary>
/// Реєстраційний номер облікової картки платника податків (РНОКПП): 10 цифр, остання контрольна.
/// Бібліотека тільки перевіряє контрольний розряд і нічого не дістає з номера.
/// </summary>
public static class Rnokpp
{
    /// <summary>Кількість цифр у номері.</summary>
    public const int Length = 10;

    private static ReadOnlySpan<int> Weights => [-1, 5, 7, 9, 4, 6, 10, 5, 7];

    /// <summary>Перевіряє, що це рівно 10 цифр і контрольний розряд сходиться.</summary>
    public static bool IsValid([NotNullWhen(true)] string? number) =>
        number is not null && IsValid(number.AsSpan());

    /// <inheritdoc cref="IsValid(string?)"/>
    public static bool IsValid(ReadOnlySpan<char> number)
    {
        if (number.Length != Length || number.ContainsAnyExceptInRange('0', '9')) return false;

        int sum = 0;
        for (int i = 0; i < Length - 1; i++)
            sum += (number[i] - '0') * Weights[i];

        // перша вага від'ємна, тож сума буває від'ємною (9000000002 дає -9),
        // а оператор % у C# тоді повертає від'ємну остачу
        int remainder = (sum % 11 + 11) % 11;

        return remainder % 10 == number[Length - 1] - '0';
    }
}
