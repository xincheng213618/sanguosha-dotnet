namespace CardGame.Core;

internal interface IShanheTeamGrowthHost
{
    void GrantOpeningMaximumHp(ProgramSkillFrame frame);
}

internal sealed class GrantShanheTeamOpeningMaximumHpDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantShanheTeamOpeningMaximumHp;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.GameStarting)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChangeMaximumHp,
        static (_, context) => context.PublicControlValue(6));
    public override ISkillProgramEffectHandler Handler { get; } = new ShanheTeamGrowthHandler();
}

internal sealed class ShanheTeamGrowthHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantShanheTeamOpeningMaximumHp;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IShanheTeamGrowthHost>().GrantOpeningMaximumHp(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
