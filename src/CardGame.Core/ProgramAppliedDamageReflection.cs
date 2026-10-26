namespace CardGame.Core;

internal interface IAppliedDamageReflectionHost
{
    SkillProgramStepOutcome ReflectAppliedDamage(ProgramSkillFrame frame);
}

internal sealed class ReflectAppliedDamageDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReflectAppliedDamage;
    public override ISkillProgramEffectHandler Handler { get; } = new ReflectAppliedDamageHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Damage,
        static (_, context) => context.ReflectAppliedDamage());

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader),
            0, reader.Condition());
        RequireAlways(effect, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}

internal sealed class ReflectAppliedDamageHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ReflectAppliedDamage;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IAppliedDamageReflectionHost>().ReflectAppliedDamage(frame);
}
