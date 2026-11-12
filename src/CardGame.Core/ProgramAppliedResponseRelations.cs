using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramAppliedResponseRelation? AppliedResponseRelation { get; internal init; }
}

// A relation declares future native capture. Its source-card match is frozen
// when this instruction executes, separately from the native use snapshot.
internal sealed class ApplyResponseRelationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyResponseRelation;
    public override ISkillProgramEffectHandler Handler { get; } = new ApplyResponseRelationHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, _) => { });
    public override IReadOnlyList<SkillStateBoundarySpec> LifetimeBoundaries(SkillProgramEffect effect) => effect.AppliedResponseRelation?.Spec.Until ?? [];
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "relationId", "actorRef", "responderRef", "sourceBind", "match", "cardKind", "isRed",
            "captureKinds", "until", "remainingExpiry", "actorMustBeTurnOwner", "selfProvidedCounterspellOnly",
            "requireLivingParticipantsAtCapture", "minimum", "forbid", "audience", "excludeActor", "incomingKinds",
            "requiredKinds", "while", "matchUseKind", "skillName", "condition");
        ProgramParticipantReference Reference(string name)
        {
            var reference = r.RequiredParticipantReference(name);
            return reference.Kind is ProgramParticipantRef.Owner or ProgramParticipantRef.SelectedTarget or
                ProgramParticipantRef.EventSource or ProgramParticipantRef.EventTarget ? reference :
                throw new InvalidOperationException("An applied response relation can freeze its owner, selected target or original event participants.");
        }
        var kind = r.RequiredEnum<ResponseRelationMatchKind>("match");
        var bind = r.OptionalIdentifier("sourceBind");
        if (bind is not null && (kind == ResponseRelationMatchKind.Any || r.Has("cardKind") || r.Has("isRed")) ||
            bind is null && (kind == ResponseRelationMatchKind.ExactCardName != r.Has("cardKind") ||
                kind != ResponseRelationMatchKind.EffectiveColor && r.Has("isRed")))
            throw new InvalidOperationException("An applied response matcher uses either its frozen source card or declared constant facts.");
        var match = bind is not null ? new ResponseRelationCardMatch(ResponseRelationMatchKind.Any) :
            new(kind, r.Has("cardKind") ? r.RequiredEnum<CardKind>("cardKind") : null,
                r.Has("isRed") ? r.RequiredBool("isRed") : null);
        var captures = r.RequiredEnumArray<AppliedResponseCaptureKind>("captureKinds");
        if (captures.Count == 0 || captures.Any(value => value is not (AppliedResponseCaptureKind.NativeUse or
            AppliedResponseCaptureKind.ActionlessNativeUse or AppliedResponseCaptureKind.Counterspell)))
            throw new InvalidOperationException("A response relation requires named native capture scopes.");
        var rule = new ResponseRule(r.Has("minimum") ? r.RequiredInt("minimum") : 1,
            r.Has("forbid") && r.RequiredBool("forbid"), incomingKinds: r.OptionalEnumArray<CardKind>("incomingKinds"),
            requiredKinds: r.OptionalEnumArray<CardKind>("requiredKinds"), @while: r.Has("while") ? r.RequiredBoolExpression("while") : null,
            matchUseKind: r.Has("matchUseKind") && r.RequiredBool("matchUseKind"), skillName: r.OptionalIdentifier("skillName"),
            audience: r.Has("audience") ? r.RequiredEnum<ResponseRuleAudience>("audience") : ResponseRuleAudience.All,
            excludeActor: r.Has("excludeActor") && r.RequiredBool("excludeActor"));
        if (rule.Minimum == 1 && !rule.Forbid) throw new InvalidOperationException("An applied response relation must change the required count or prohibit responses.");
        var until = r.Has("until") ? SkillStateDescriptor.ParseUntil(r.Node.GetProperty("until"), r.Path + ".until") : [];
        var spec = new AppliedResponseRelationSpec(rule, match, captures.Aggregate((left, right) => left | right), until,
            r.Has("remainingExpiry") ? r.RequiredEnum<AppliedResponseRemainingExpiry>("remainingExpiry") : AppliedResponseRemainingExpiry.DeclaredBoundaries,
            r.Has("actorMustBeTurnOwner") && r.RequiredBool("actorMustBeTurnOwner"),
            r.Has("selfProvidedCounterspellOnly") && r.RequiredBool("selfProvidedCounterspellOnly"),
            r.Has("requireLivingParticipantsAtCapture") && r.RequiredBool("requireLivingParticipantsAtCapture"));
        var responder = r.Has("responderRef") ? Reference("responderRef") : null;
        if (responder is null && until.Any(boundary => boundary.Participant == SkillStateSubject.Target))
            throw new InvalidOperationException("A target lifetime requires an explicit response-relation recipient.");
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition())
        {
            AppliedResponseRelation = new(r.RequiredIdentifier("relationId"), r.Has("actorRef") ? Reference("actorRef") : new(ProgramParticipantRef.Owner),
                responder, spec, bind, bind is null ? null : kind)
        };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)
    {
        var declaration = effect.AppliedResponseRelation!;
        return Array.AsReadOnly(ParticipantResources(declaration.ActorRef, declaration.ResponderRef)
            .Concat(declaration.SourceBind is { } bind ? new ProgramResourceOperation[] { new ReadSingleCardSet(bind) } : []).ToArray());
    }
}
internal sealed class ApplyResponseRelationHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyResponseRelation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IAppliedResponseRules>().Apply(frame, effect.AppliedResponseRelation!);
        return SkillProgramStepOutcome.Continue;
    }
}
