namespace CardGame.Core;

internal interface IHellActivityProgramHost
{
    SkillProgramStepOutcome ExecuteHellActivity(ProgramSkillFrame frame, SkillProgramEffectOp op);
}
internal abstract class HellActivityOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new HellActivityEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
}
internal sealed class GainHellTurnSlashDescriptor : HellActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GainHellTurnSlash;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class DrawHellChongshengRefillDescriptor : HellActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawHellChongshengRefill;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.SelfDyingResponse)];
}
internal sealed class RecoverHellChongshengDescriptor : HellActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecoverHellChongsheng;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.SelfDyingResponse)];
}
internal sealed class HellActivityEffectHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) =>
        h.GetCapability<IHellActivityProgramHost>().ExecuteHellActivity(f, Op);
}
public sealed record HellChongshengRefillIssuedEvent(long ProgramFrameId, int OwnerSeat, int RequestedCount) : IGameEvent;

public sealed partial class GameEngine
{
}
