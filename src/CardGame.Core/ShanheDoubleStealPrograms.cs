namespace CardGame.Core;

public static class ShanheDoubleStealPrograms
{
    public const string SkillId = "shanhe:current-double-steal-2337";
}

internal interface IShanheDoubleStealHost
{
    void RepeatCurrentSnatch(ProgramSkillFrame frame);
}

internal sealed class RepeatCurrentSnatchDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RepeatCurrentSnatch;
    public override ISkillProgramEffectHandler Handler { get; } = new RepeatCurrentSnatchHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted), new RequireCardActionActor()];
}

internal sealed class RepeatCurrentSnatchHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RepeatCurrentSnatch;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host)
    {
        host.GetCapability<IShanheDoubleStealHost>().RepeatCurrentSnatch(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
