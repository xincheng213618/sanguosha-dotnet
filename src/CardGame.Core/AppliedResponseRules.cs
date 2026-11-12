using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ResponseRelationMatchKind { Any, ExactCardName, EffectiveColor }
public sealed record ResponseRelationCardMatch(ResponseRelationMatchKind Kind, CardKind? ExactNameKind = null, bool? IsRed = null);
[Flags]
public enum AppliedResponseCaptureKind { NativeUse = 1, ActionlessNativeUse = 2, Counterspell = 4 }
public enum AppliedResponseRemainingExpiry { DeclaredBoundaries, IssuedTurnNumberChanges }

public sealed class AppliedResponseRelationSpec
{
    public AppliedResponseRelationSpec(ResponseRule rule, ResponseRelationCardMatch match, AppliedResponseCaptureKind captureKinds,
        IReadOnlyList<SkillStateBoundarySpec>? until = null,
        AppliedResponseRemainingExpiry remainingExpiry = AppliedResponseRemainingExpiry.DeclaredBoundaries,
        bool actorMustBeTurnOwner = false, bool selfProvidedCounterspellOnly = false,
        bool requireLivingParticipantsAtCapture = false)
    {
        ArgumentNullException.ThrowIfNull(rule);
        ArgumentNullException.ThrowIfNull(match);
        if (!Enum.IsDefined(match.Kind) || !Enum.IsDefined(remainingExpiry) || captureKinds == 0 ||
            (captureKinds & ~(AppliedResponseCaptureKind.NativeUse | AppliedResponseCaptureKind.ActionlessNativeUse | AppliedResponseCaptureKind.Counterspell)) != 0 ||
            match.Kind == ResponseRelationMatchKind.ExactCardName && match.ExactNameKind is null ||
            match.Kind != ResponseRelationMatchKind.ExactCardName && match.ExactNameKind is not null ||
            match.Kind != ResponseRelationMatchKind.EffectiveColor && match.IsRed is not null ||
            remainingExpiry == AppliedResponseRemainingExpiry.DeclaredBoundaries && until is not { Count: > 0 })
            throw new InvalidOperationException("An applied response relation requires a declared matcher, capture scope and remaining lifetime.");
        Rule = rule; Match = match; CaptureKinds = captureKinds;
        Until = Array.AsReadOnly((until ?? []).ToArray()); RemainingExpiry = remainingExpiry;
        ActorMustBeTurnOwner = actorMustBeTurnOwner; SelfProvidedCounterspellOnly = selfProvidedCounterspellOnly;
        RequireLivingParticipantsAtCapture = requireLivingParticipantsAtCapture;
    }
    public ResponseRule Rule { get; }
    public ResponseRelationCardMatch Match { get; }
    public AppliedResponseCaptureKind CaptureKinds { get; }
    public IReadOnlyList<SkillStateBoundarySpec> Until { get; }
    public AppliedResponseRemainingExpiry RemainingExpiry { get; }
    public bool ActorMustBeTurnOwner { get; }
    public bool SelfProvidedCounterspellOnly { get; }
    public bool RequireLivingParticipantsAtCapture { get; }
}

public sealed record AppliedResponseRelation(long Sequence, string RelationId, long ProducerFrameId, int InstructionIndex,
    int IssuanceSlot, CardConversionSource Source, string GameplayHash, int ActorSeat, int? ResponderSeat,
    int IssuedTurnNumber, AppliedResponseRelationSpec Spec, SkillStateLifetime? Lifetime);

public sealed record NativeCounterspellResponseNode(int ChainDepth, CardActionContext Action);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AppliedResponseRulesCaptured { get; init; }
}

internal interface IAppliedResponseRelationBoundaries : IProgramRuntimeOperations
{
    void ProcessBoundary(SkillStateBoundary boundary);
}
internal interface IAppliedResponseRelationStore : IProgramRuntimeOperations
{
    IReadOnlyList<AppliedResponseRelation> Relations { get; }
    AppliedResponseRelation Add(AppliedResponseRelation relation);
}
internal interface IAppliedResponseRules : IProgramRuntimeOperations
{
    AppliedResponseRelation Grant(ProgramSkillFrame frame, string relationId, int actorSeat, int? responderSeat,
        AppliedResponseRelationSpec spec, int issuanceSlot = 0);
    AppliedResponseRelation? GetMatchingRelation(string relationId, int actorSeat, int responderSeat,
        CardKind incomingKind, bool? effectiveIsRed);
    void CaptureNativeUse(CardUseFrame use, CardUseDeclaredEvent? declaration = null);
    void CaptureCounterspell(NullificationWindowFrame window, CardActionContext acceptedAction, int? issuedChainDepth = null);
    void IssueResponseRule(long frameId, ResponseRule rule);
    void RemoveResponseRulesIssuedBy(long frameId, CardConversionSource source);
    NativeResponseQuery? CreateResponseQuery(long incomingFrameId, int opponentSeat, int responderSeat,
        CardKind incoming, CardKind required, IReadOnlyList<int> targets, int? providerSeat = null, int counterspellDepth = 0);
    NativeResponseQuery? CreateCounterspellQuery(NullificationWindowFrame window, int opponentSeat, int responderSeat, int? providerSeat = null);
    ResponseDecision GetResponseDecision(NativeResponseQuery query);
    bool HasUnconditionalResponseProhibition(long frameId);
    bool IsResponseForbidden(long frameId, int responderSeat, CardKind requiredKind, int? opponentSeat = null);
    bool IsCounterspellResponseForbidden(NullificationWindowFrame window, int responderSeat, int? providerSeat = null);
    void Apply(ProgramSkillFrame frame, ProgramAppliedResponseRelation declaration);
}

public sealed record ProgramAppliedResponseRelation(string RelationId, ProgramParticipantReference ActorRef,
    ProgramParticipantReference? ResponderRef, AppliedResponseRelationSpec Spec, string? SourceBind = null,
    ResponseRelationMatchKind? SourceMatchKind = null);
