namespace CardGame.Core;

// 燕尔 draws two pairs, the owner first and then the last-hand loser.
// The owner's matching pair relaxes 织纴 until their next turn.
// The loser's matching pair recovers that player by one HP.
internal sealed class YanerResolvePairBenefitDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.YanerResolvePairBenefit;
    public override ISkillProgramEffectHandler Handler { get; } = new YanerResolvePairBenefitHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (effect, context) => context.Draw(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition", "skillIds", "stateId");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(),
            skillIds: r.RequiredIdentifierArray("skillIds"),
            stateId: r.RequiredIdentifier("stateId"));
        RequireAlways(effect, r.Path);
        if (effect.SkillIds.Count != 1)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.skillIds: yaner targets exactly one paired skill.");
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsMoved)];
}

public sealed class YanerResolvePairBenefitHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.YanerResolvePairBenefit;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IPanShuProgramHost>().YanerResolvePairBenefit(f, e);
}
