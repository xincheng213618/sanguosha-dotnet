namespace CardGame.Core;

public sealed record ProgramCurrentResponseRule(ResponseRule Rule, ProgramParticipantReference? ActorRef = null,
    ProgramParticipantReference? OpponentRef = null, ProgramParticipantReference? ResponderRef = null);

internal interface ICurrentResponseRuleProgramHost
{
    void IssueCurrentResponseRule(ProgramSkillFrame frame, ProgramCurrentResponseRule declaration);
}

internal sealed class IssueCurrentResponseRuleDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.IssueCurrentResponseRule;
    public override ISkillProgramEffectHandler Handler { get; } = new IssueCurrentResponseRuleHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ProhibitCurrentResponse,
        static (_, context) => context.ProhibitCurrentResponse());
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "minimum", "forbid", "audience", "incomingKinds", "requiredKinds", "while",
            "matchUseKind", "actorRef", "opponentRef", "responderRef", "condition");
        var rule = new ResponseRule(minimum: r.Has("minimum") ? r.RequiredInt("minimum") : 1,
            forbid: r.Has("forbid") && r.RequiredBool("forbid"), incomingKinds: r.OptionalEnumArray<CardKind>("incomingKinds"),
            requiredKinds: r.OptionalEnumArray<CardKind>("requiredKinds"), condition: r.Has("while") ? r.RequiredBoolExpression("while") : null,
            matchUseKind: r.Has("matchUseKind") && r.RequiredBool("matchUseKind"),
            audience: r.Has("audience") ? r.RequiredEnum<ResponseRuleAudience>("audience") : ResponseRuleAudience.Targets);
        if (rule.Minimum == 1 && !rule.Forbid)
            throw new InvalidOperationException("An issued response rule must change the required count or prohibit responses.");
        ProgramParticipantReference? Reference(string name)
        {
            if (!r.Has(name)) return null;
            var reference = r.RequiredParticipantReference(name);
            return reference.Kind is ProgramParticipantRef.Owner or ProgramParticipantRef.EventSource or ProgramParticipantRef.EventTarget
                ? reference : throw new InvalidOperationException("A response rule can freeze its owner or original action participants.");
        }
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition())
        { CurrentResponseRule = new(rule, Reference("actorRef"), Reference("opponentRef"), Reference("responderRef")) };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireContext(ProgramContextCapability.CardAction), new RequireTriggerWindows(
            [SkillProgramTriggerWindow.CardUseCommitted, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramTriggerWindow.SlashBeforeResponse])];
}

internal sealed class IssueCurrentResponseRuleHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.IssueCurrentResponseRule;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host)
    {
        host.GetCapability<ICurrentResponseRuleProgramHost>().IssueCurrentResponseRule(frame, effect.CurrentResponseRule!);
        return SkillProgramStepOutcome.Continue;
    }
}
