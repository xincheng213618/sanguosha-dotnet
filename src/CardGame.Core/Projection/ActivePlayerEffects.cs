using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ActivePlayerEffectExpiry
{
    CurrentTurnEnd, NextSourceTurnStart, PlayPhaseEnd, ConsumedOrTurnEnd,
    AffectedNextTurnEnd, AffectedNextTurn, RuleCleared
}

/// <summary>Public display facts about an issued effect, without frames, bindings or private card identities.</summary>
public sealed record ActivePlayerEffectSnapshot(string SkillId, string SkillName, string Description,
    int SourceSeat, ActivePlayerEffectExpiry Expiry, int? RemainingUses = null, int? SourceSeatDisplayNumber = null);

public sealed partial record PlayerSnapshot
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ActivePlayerEffectSnapshot>? ActiveEffects { get; init; }
}
