namespace CardGame.Core;

internal interface IMobileGuoHuaiHost { void Prepare(ProgramSkillFrame frame); }

internal sealed class PrepareMobileGuoHuaiJingceDescriptor : ProgramOperationDescriptorBase, IActualPlayPhaseUseLedgerOperation
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileGuoHuaiJingce;
    public override ISkillProgramEffectHandler Handler { get; } = new PrepareMobileGuoHuaiJingceHandler();
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.PlayEnding)];
}

internal sealed class PrepareMobileGuoHuaiJingceHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileGuoHuaiJingce;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IMobileGuoHuaiHost>().Prepare(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
