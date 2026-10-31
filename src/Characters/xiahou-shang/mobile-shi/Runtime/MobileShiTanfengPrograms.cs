namespace CardGame.Core;

public sealed record MobileShiTanfengTargetSelectedEvent(long ProgramFrameId, string SkillInstanceId,
    string TargetRole, int TargetSeat, string Mode) : IGameEvent;

internal interface IMobileShiTanfengHost
{
    SkillProgramStepOutcome SelectTarget(ProgramSkillFrame frame, SkillProgramEffect effect);
}

// Only the two independent target selections need an adapter. The preselected
// options, native card payments, damage and phase debts remain ordinary nodes.
internal sealed class ResolveMobileShiXiahouShangTanfengDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileShiXiahouShangTanfeng;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileShiTanfengHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectTarget, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "stateId", "condition");
        var role = r.RequiredIdentifier("stateId");
        var condition = r.Condition();
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner ||
            r.RequiredIdentifier("sourceBind") != "tanfeng-mode" || role is not ("initial-target" or "fire-target") ||
            role == "initial-target" && (condition.Kind != SkillProgramConditionKind.Any || condition.Children.Count != 4 ||
                !condition.Children.Select(item => item.OptionId).Order(StringComparer.Ordinal).SequenceEqual(new[] { "both", "cancel", "discard", "fire" }) ||
                condition.Children.Any(item => item.Kind != SkillProgramConditionKind.ChoiceIs || item.SourceBind != "tanfeng-mode")) ||
            role == "fire-target" && (condition.Kind != SkillProgramConditionKind.ChoiceIs ||
                condition.SourceBind != "tanfeng-mode" || condition.OptionId != "both"))
            throw new InvalidOperationException(r.Path + ": Tanfeng targets require the original named four-option choice and its exact second-target branch.");
        return new(Op, SkillProgramEffectTarget.Owner, 0, condition, sourceBind: "tanfeng-mode", stateId: role);
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        effect.StateId == "initial-target"
            ? [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),
                new RequireChoiceOptions("tanfeng-mode", ["discard", "fire", "both", "cancel"]), new SelectSingleTarget()]
            : [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),
                new RequireChoiceOptions("tanfeng-mode", ["discard", "fire", "both", "cancel"]), new ReadSelectedTarget(), new ReplaceSingleTarget()];
}

internal sealed class MobileShiTanfengHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileShiXiahouShangTanfeng;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileShiTanfengHost>().SelectTarget(frame, effect);
}
