
namespace CardGame.Core;

internal interface IShanheSchoolTrickHost
{
    void RepeatCurrentDamageTrick(ProgramSkillFrame frame);
}

internal sealed class RepeatCurrentDamageTrickDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RepeatCurrentDamageTrick;
    public override ISkillProgramEffectHandler Handler { get; } = new RepeatCurrentDamageTrickHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted), new RequireCardActionActor()];
}

internal sealed class RepeatCurrentDamageTrickHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RepeatCurrentDamageTrick;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host)
    {
        host.GetCapability<IShanheSchoolTrickHost>().RepeatCurrentDamageTrick(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
