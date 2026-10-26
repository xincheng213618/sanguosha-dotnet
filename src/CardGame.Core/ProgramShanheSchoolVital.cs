namespace CardGame.Core;

internal interface IShanheSchoolVitalHost
{
    void SetSchoolHpToMaximum(ProgramSkillFrame frame);
}

internal sealed class SetShanheSchoolHpToMaximumDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SetShanheSchoolHpToMaximum;
    public override ISkillProgramEffectHandler Handler { get; } = new SetShanheSchoolHpToMaximumHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.RecoverTo,
        static (_, context) => context.RecoverTo(new(SkillProgramEffectOp.RecoverTo, SkillProgramEffectTarget.Owner,
            0, new(SkillProgramConditionKind.Always, 0, []),
            numberExpression: SkillProgramNumberExpression.IntegerConstant, minimumValue: 20, clampToMaxHp: true)));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target");
        if (reader.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.target: school HP adjustment requires owner.");
        return new(Op, SkillProgramEffectTarget.Owner, 0, new(SkillProgramConditionKind.Always, 0, []));
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}

internal sealed class SetShanheSchoolHpToMaximumHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.SetShanheSchoolHpToMaximum;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IShanheSchoolVitalHost>().SetSchoolHpToMaximum(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
