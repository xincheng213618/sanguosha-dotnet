using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record NativeSlashTargetDuelTarget(int TargetIndex, int TargetSeat, CardUseEffectSource? Source);
public enum NativeSlashTargetDuelStage { Counterspell, Responding, Canceled }
public sealed record NativeSlashTargetDuelSlice(int Ordinal, int TargetIndex, int TargetSeat,
    CardUseEffectSource Source, string GameplayHash, CardAttackState OriginalAttack,
    NativeSlashTargetDuelStage Stage);

// The original Use owns this immutable target policy and its current native child.
// Original CardKind/Action/materials and the native multi-target cursor never change.
public sealed record NativeSlashTargetDuelPlan(long UseId, long ActionId, int ActorSeat,
    int ProviderSeat, CardKind OriginalKind)
{
    private readonly IReadOnlyList<NativeSlashTargetDuelTarget> _targets = Array.AsReadOnly(Array.Empty<NativeSlashTargetDuelTarget>());
    public IReadOnlyList<NativeSlashTargetDuelTarget> Targets
    { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeSlashTargetDuelSlice? Slice { get; init; }
    public int IssuedCount { get; init; }
}

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool NativeSlashTargetDuelPlanFrozen { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeSlashTargetDuelPlan? NativeSlashTargetDuel { get; init; }
}

public sealed record NativeSlashTargetDuelIssuedEvent(long UseId, long ActionId, int Ordinal, int TargetIndex,
    int ActorSeat, int ProviderSeat, int TargetSeat, CardKind OriginalKind,
    CardUseEffectSource Source, string GameplayHash) : IGameEvent;
public sealed record NativeSlashTargetDuelStartedEvent(long UseId, long ActionId, int TargetIndex,
    int ActorSeat, int TargetSeat) : IGameEvent;
public sealed record NativeSlashTargetDuelCanceledEvent(long UseId, long ActionId,
    int TargetIndex, int TargetSeat) : IGameEvent;
