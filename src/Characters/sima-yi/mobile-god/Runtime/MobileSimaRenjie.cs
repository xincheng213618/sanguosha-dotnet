using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum MobileSimaRenjieReturnKind { Counterspell, FailedAttack, BorrowedSword, Qizheng }
public sealed record MobileSimaResponseRequirement(long Id, long UseFrameId, int ResponderSeat,
    CardKind RequiredKind, int TargetIndex, int SuccessfulResponses, bool Settled = false);
public sealed partial record CardUseFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileSimaResponseRequirement? MobileSimaResponse { get; init; } }
[JsonDerivedType(typeof(MobileSimaRenjieFrame), "mobile-sima-renjie")]
public abstract partial record ResolutionFrame;
public sealed record MobileSimaRenjieFrame(long Id, long ParentFrameId, long UseFrameId, CardConversionSource Source,
    string ContentHash, MobileSimaRenjieReturnKind ReturnKind, string OccurrenceKey, int RoundNumber,
    int CandidateIndex = -1, int ChainDepth = -1, long RequirementId = 0, bool Applied = false,
    ResolutionFrameStep Step = ResolutionFrameStep.ResolvingEffect)
    : ResolutionFrame(Id, ResolutionFrameKind.MobileSimaRenjie, Step), IProgramChildResolutionFrame;
public sealed record MobileSimaResponseRequiredEvent(MobileSimaResponseRequirement Requirement) : IGameEvent;
public sealed record MobileSimaRenjieStartedEvent(long FrameId, long ParentFrameId, long UseFrameId, CardConversionSource Source,
    MobileSimaRenjieReturnKind ReturnKind, string OccurrenceKey, int RoundNumber) : IGameEvent;
public sealed record MobileSimaRenjieAppliedEvent(long FrameId, int OwnerSeat, int RoundNumber, bool Gained) : IGameEvent;
