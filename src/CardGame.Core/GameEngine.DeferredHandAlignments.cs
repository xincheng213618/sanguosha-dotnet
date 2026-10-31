using System.Text.Json.Serialization;

namespace CardGame.Core;
public enum DeferredHandAlignmentDueKind
{
    SourceCurrentTurnEnd = 1300,
    TargetNextActualTurnEnd = 1301
}

public sealed record DeferredHandAlignment(long Id, CardConversionSource Source, int TargetSeat, int CreatedTurn, DeferredHandAlignmentDueKind DueKind, string ContinuationId);
public sealed record DeferredHandAlignmentScheduledEvent(DeferredHandAlignment Alignment) : IGameEvent;
public sealed record DeferredHandAlignmentConsumedEvent(DeferredHandAlignment Alignment, bool Applied) : IGameEvent;
public sealed record DeferredHandAlignmentCancelledEvent(long Id) : IGameEvent;
public sealed record DeferredHandAlignmentResolvedEvent(long Id, int OwnerHandCount, int TargetHandCount, int DrawCount, int DiscardCount) : IGameEvent;
public sealed record DeferredHandAlignmentResolution(long DueId, int TargetSeat, int OwnerHandCount, int TargetHandCount, int DrawCount, int DiscardCount);
public sealed record DeferredTurnEndFrame(long Id, int OwnerSeat, int TurnNumber, bool Skipped, IReadOnlyList<long> DueIds, int ItemIndex = 0, DeferredHandAlignment? Current = null) : ResolutionFrame(Id, ResolutionFrameKind.DeferredTurnEnd, ResolutionFrameStep.ResolvingEffect)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AfterTurnEndedWindow? AfterTurnEnded { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DeferredTurnEndPrelude? Prelude { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SoleTargetQuarantineReturnCursor? SoleTargetQuarantineReturns { get; init; }
}
