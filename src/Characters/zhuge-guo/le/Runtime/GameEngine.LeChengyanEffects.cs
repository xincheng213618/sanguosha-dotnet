using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record LeChengyanEffectPlan(long ProgramFrameId, CardConversionSource Source, string GameplayHash,
    long ActionId, int ShownCardId, CardKind EffectKind)
{
    public bool Started { get; init; }
    public long? TranslatedWindowId { get; init; }
    public int? TranslatedTargetIndex { get; init; }
    public int SlashCursor { get; init; }
    public bool Finished { get; init; }
    public int BorrowedCursor { get; init; }
    public bool AwaitingVictim { get; init; }
    public bool AwaitingBorrowedEffect { get; init; }
    public int? AuxiliaryVictimSeat { get; init; }
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record CardUseFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public LeChengyanEffectPlan? LeChengyanEffect { get; init; } }
public sealed record LeChengyanEffectStartedEvent(long UseId, long ActionId, CardKind OriginalKind, CardKind EffectKind, int TargetCount) : IGameEvent;
