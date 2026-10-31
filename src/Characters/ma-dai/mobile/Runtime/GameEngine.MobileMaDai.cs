namespace CardGame.Core;

internal interface IMobileMaDaiProgramHost
{
    SkillProgramStepOutcome ReduceDamageTargetMaximum(ProgramSkillFrame frame);
}

internal sealed class ReduceMobileMaDaiDamageTargetMaximumHpDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReduceMobileMaDaiDamageTargetMaximumHp;
    public override ISkillProgramEffectHandler Handler { get; } = new ReduceMobileMaDaiDamageTargetMaximumHpHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChangeMaximumHp, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition());
        if (effect.Condition.Kind != SkillProgramConditionKind.JudgmentOutcomeMatchesSuits)
            throw new InvalidOperationException(reader.Path + ": mobile Ma Dai requires its finalized non-heart judgment.");
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
}

internal sealed class ReduceMobileMaDaiDamageTargetMaximumHpHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ReduceMobileMaDaiDamageTargetMaximumHp;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileMaDaiProgramHost>().ReduceDamageTargetMaximum(frame);
}

internal static class MobileMaDaiComposition
{
    internal const string Skill = "mobile:ma-dai-qianxi",JudgmentReason = "skill.mobile.ma-dai-qianxi";
    [ProgramCatalogValidation(72909)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.SelectMany(t => t.Effects).Concat(program.Activations.SelectMany(a => a.Effects))
                .Any(e => e.Op == SkillProgramEffectOp.ReduceMobileMaDaiDamageTargetMaximumHp)) return;
        if (program.Id != Skill || program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.Modifiers.Count != 0 ||
            program.Triggers is not [var trigger] || trigger.Id != "judge-nearby-slash-damage" ||
            trigger.Window != SkillProgramTriggerWindow.BeforeDamageApplied || trigger.Subject != SkillProgramTriggerSubject.DamageSource ||
            !trigger.Optional || trigger.UsageScope is not null || trigger.UsageLimit is not null ||
            trigger.Condition.Kind != SkillProgramTriggerConditionKind.All || trigger.Condition.Children is not
                [{ Kind: SkillProgramTriggerConditionKind.DirectCardUseDamage }, { Kind: SkillProgramTriggerConditionKind.DamageCardIsSlash }] ||
            trigger.Effects is not [
                { Op: SkillProgramEffectOp.StartJudgment, Target: SkillProgramEffectTarget.Owner, JudgmentReason: JudgmentReason, ResultBind: "qianxi-card", OutcomeBind: "qianxi-outcome", Visibility: SkillProgramCardSetVisibility.Public },
                { Op: SkillProgramEffectOp.PreventCurrentDamage } prevent,
                { Op: SkillProgramEffectOp.ReduceMobileMaDaiDamageTargetMaximumHp } reduce,
                { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "qianxi-card", Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true }] ||
            !NonHeart(prevent.Condition) || !NonHeart(reduce.Condition))
            throw new InvalidOperationException(path + ": mobile Ma Dai requires original direct Slash damage, an owner judgment, non-heart prevention before target maximum loss, and original judgment cleanup.");
    }
    private static bool NonHeart(SkillProgramCondition condition) => condition.Kind == SkillProgramConditionKind.JudgmentOutcomeMatchesSuits &&
        condition.SourceBind == "qianxi-outcome" && condition.Suits.Count == 3 &&
        condition.Suits.ToHashSet().SetEquals([Suit.Spade, Suit.Club, Suit.Diamond]);
}

public sealed record MobileMaDaiMaximumReducedEvent(long ProgramFrameId, long BeforeDamageFrameId, long DamageOwnerFrameId,
    long JudgmentFrameId, int OwnerSeat, int TargetSeat, int MaximumBefore, int MaximumAfter) : IGameEvent;
