namespace CardGame.Core;

internal interface IShanheSourceLiuSandaoHost
{
    SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation);
}

internal abstract class ShanheSourceLiuSandaoDescriptor(SkillProgramEffectOp operation,
    SkillProgramTriggerWindow window) : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => operation;
    public override ISkillProgramEffectHandler Handler { get; } = new ShanheSourceLiuSandaoHandler(operation);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(window)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnSkills, static (_, _) => { });
}

internal sealed class AdjustShanheSourceHuchenDrawDescriptor() : ShanheSourceLiuSandaoDescriptor(
    SkillProgramEffectOp.AdjustShanheSourceHuchenDraw, SkillProgramTriggerWindow.DrawPhaseStarting);
internal sealed class DiscardShanheSourceZhanshenWeaponsDescriptor() : ShanheSourceLiuSandaoDescriptor(
    SkillProgramEffectOp.DiscardShanheSourceZhanshenWeapons, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow);
internal sealed class ShanheSourceLiuSandaoHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IShanheSourceLiuSandaoHost>().Resolve(frame, operation);
}
