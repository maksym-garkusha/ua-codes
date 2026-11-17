using System.Diagnostics.CodeAnalysis;

namespace UaCodes;

/// <summary>
/// Код за Кодифікатором адміністративно-територіальних одиниць та територій територіальних
/// громад (КАТОТТГ): <c>UA</c> і 17 цифр, перші дві з них код області. Контрольного розряду
/// в коді немає, тому перевіряються тільки форма і те, що такий код області існує.
/// </summary>
public static class Katottg
{
    /// <summary>Довжина коду разом із <c>UA</c>.</summary>
    public const int Length = 19;

    /// <summary>Перевіряє, що це <c>UA</c> і 17 цифр, а перші дві цифри є кодом області.</summary>
    public static bool HasValidFormat([NotNullWhen(true)] string? code) =>
        code is not null && HasValidFormat(code.AsSpan());

    /// <inheritdoc cref="HasValidFormat(string?)"/>
    public static bool HasValidFormat(ReadOnlySpan<char> code) =>
        code.Length == Length
        && code.StartsWith("UA")
        && !code[2..].ContainsAnyExceptInRange('0', '9')
        && NameOf(code.Slice(2, 2)) is not null;

    /// <summary>Дві цифри коду області.</summary>
    /// <returns>Код області або <c>null</c>, якщо код не проходить <see cref="HasValidFormat(string?)"/>.</returns>
    public static string? RegionCode(string? code) =>
        HasValidFormat(code) ? code.Substring(2, 2) : null;

    /// <summary>Назва області, Автономної Республіки Крим або міста зі спеціальним статусом.</summary>
    /// <returns>Назва українською або <c>null</c>, якщо код не проходить <see cref="HasValidFormat(string?)"/>.</returns>
    public static string? RegionName(string? code) =>
        HasValidFormat(code) ? NameOf(code.AsSpan(2, 2)) : null;

    private static string? NameOf(ReadOnlySpan<char> region) => region switch
    {
        "01" => "Автономна Республіка Крим",
        "05" => "Вінницька область",
        "07" => "Волинська область",
        "12" => "Дніпропетровська область",
        "14" => "Донецька область",
        "18" => "Житомирська область",
        "21" => "Закарпатська область",
        "23" => "Запорізька область",
        "26" => "Івано-Франківська область",
        "32" => "Київська область",
        "35" => "Кіровоградська область",
        "44" => "Луганська область",
        "46" => "Львівська область",
        "48" => "Миколаївська область",
        "51" => "Одеська область",
        "53" => "Полтавська область",
        "56" => "Рівненська область",
        "59" => "Сумська область",
        "61" => "Тернопільська область",
        "63" => "Харківська область",
        "65" => "Херсонська область",
        "68" => "Хмельницька область",
        "71" => "Черкаська область",
        "73" => "Чернівецька область",
        "74" => "Чернігівська область",
        "80" => "місто Київ",
        "85" => "місто Севастополь",
        _ => null,
    };
}
