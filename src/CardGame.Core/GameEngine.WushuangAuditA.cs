namespace CardGame.Core;

public sealed record WushuangTiaoxinPhaseExpandedEvent(long FrameId, int OwnerSeat, string SkillInstanceId,
    int ActualTurn) : IGameEvent;

internal interface IWushuangTiaoxinExpansionHost
{
    void Record(ProgramSkillFrame frame);
}
internal sealed class RecordWushuangTiaoxinPhaseExpansionDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecordWushuangTiaoxinPhaseExpansion;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler { get; } = new RecordWushuangTiaoxinPhaseExpansionHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, _) => { });
}
internal sealed class RecordWushuangTiaoxinPhaseExpansionHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RecordWushuangTiaoxinPhaseExpansion;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IWushuangTiaoxinExpansionHost>().Record(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
