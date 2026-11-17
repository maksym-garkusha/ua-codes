namespace UaCodes;

/// <summary>Тип об'єкта, який позначає EIC. Визначається третім символом коду.</summary>
public enum EicKind
{
    /// <summary><c>X</c>: учасник ринку.</summary>
    MarketParticipant,

    /// <summary><c>Y</c>: область або зона ринку.</summary>
    Area,

    /// <summary><c>Z</c>: точка обліку.</summary>
    MeteringPoint,

    /// <summary><c>W</c>: ресурс (генерація, споживання, накопичення).</summary>
    Resource,

    /// <summary><c>T</c>: міждержавна лінія.</summary>
    Tieline,

    /// <summary><c>V</c>: локація.</summary>
    Location,

    /// <summary><c>A</c>: підстанція.</summary>
    Substation,
}
