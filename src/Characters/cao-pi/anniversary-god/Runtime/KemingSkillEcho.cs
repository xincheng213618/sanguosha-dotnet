using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record KemingEchoEntitlement(long IssueFrameId, CardConversionSource Source, string GameplayHash);
public sealed record KemingProducedName(CardKind Name, bool IsDraw, long ProducerId, long Sequence);
public sealed record KemingSourceCapture(long InvocationId, long ParentFrameId, int ActorSeat, string SkillKey,
    string OccurrenceKey, string SkillInstanceId, ActualDiscardRecoveryPhaseKey Phase, bool Closed = false)
{
    private IReadOnlyList<KemingEchoEntitlement> _entitlements = Array.AsReadOnly(Array.Empty<KemingEchoEntitlement>());
    private IReadOnlyList<KemingProducedName> _names = Array.AsReadOnly(Array.Empty<KemingProducedName>());
    public IReadOnlyList<KemingEchoEntitlement> Entitlements { get => _entitlements; init => _entitlements = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<KemingProducedName> Names { get => _names; init => _names = Array.AsReadOnly(value.ToArray()); }
}
public enum KemingSkillEchoStage { Choosing, Drawing, Children, Finished }
public enum KemingEchoReturnKind { ProgramCompletion, CardWindow, CixiongCompletion }
public sealed record KemingEchoReturn(KemingEchoReturnKind Kind, bool SourceCompleted = true, bool Activated = false,
    bool TargetDiscarded = false, int? DiscardedCardId = null, int SourceDrawCount = 0);
[JsonDerivedType(typeof(KemingSkillEchoFrame), "keming-skill-echo")]
public abstract partial record ResolutionFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public KemingSourceCapture? KemingCapture { get; init; } }
public sealed record KemingSkillEchoFrame(long Id, long ParentFrameId, KemingSourceCapture Capture, KemingEchoReturn Return,
    int Cursor = 0, KemingSkillEchoStage Stage = KemingSkillEchoStage.Choosing,
    ResolutionFrameStep Step = ResolutionFrameStep.ResolvingEffect, NativeDrawOutcome? Outcome = null)
    : ResolutionFrame(Id, ResolutionFrameKind.KemingSkillEcho, Step);
public sealed partial record CardsMovedTriggerWindowFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public long? ResumeKemingEchoFrameId { get; init; } }
public sealed record KemingEchoReservedEvent(long IssueFrameId, long InvocationId, long ParentFrameId, int ActorSeat,
    string SkillKey, string OccurrenceKey) : IGameEvent;
public sealed record KemingEchoStartedEvent(long FrameId, long ParentFrameId, long InvocationId, int Count) : IGameEvent;
public sealed record KemingEchoFinishedEvent(long FrameId, long ParentFrameId, long InvocationId) : IGameEvent;
public sealed record KemingSourceCardUsedEvent(long InvocationId, long SourceFrameId, long ActionId) : IGameEvent;
