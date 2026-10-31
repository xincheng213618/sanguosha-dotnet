namespace CardGame.Core;

public enum DrawPhaseObligationStage { Starting, Drawing, DrawChildren, EndObservers, SkipObservers, ConsumeMarker, Finished, RequestingDraw = 27570 }
public sealed record DrawPhaseObligationFrame(long Id, int OwnerSeat, int ActualTurnNumber,
    bool IsExtra, int DelayedEffects, DrawPhaseObligationStage Stage, long? ParentFrameId = null,
    bool NormalDrawReplaced = false, int DrawAdjustment = 0, int? FrozenBaseDrawCount = null,
    bool Skipped = false, int DrawCount = 0, long? ActiveWindowFrameId = null)
    : ResolutionFrame(Id, ResolutionFrameKind.DrawPhaseObligation, ResolutionFrameStep.ResolvingEffect)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<long>? InheritedMovementBatchIds { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public ActualDiscardRecoveryPhaseKey? DiscardRecoveryPhase { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceChunlaoJudgmentDrawReceipt? ShanheSourceChunlaoJudgmentDraw { get; init; }
}
public sealed record DrawPhaseSkippedEvent(long FrameId, int OwnerSeat, int ActualTurnNumber, bool IsExtra) : IGameEvent;
public sealed record ExtraDrawPhaseStartedEvent(long FrameId, long ParentFrameId, int OwnerSeat,
    int ActualTurnNumber, PlayerMarkerKind Marker, int SourceSeat, string SkillId, string SkillInstanceId) : IGameEvent;
public sealed record ActualDrawPhaseCompletedEvent(long FrameId, int OwnerSeat, int ActualTurnNumber,
    bool IsExtra, bool Skipped, int DrawCount) : IGameEvent;
