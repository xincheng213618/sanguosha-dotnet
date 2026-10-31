using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheSourceZhaoranArmedEvent(long FrameId, CardConversionSource Source,
    int ActualTurn, int PlayInstance) : IGameEvent;
public sealed record ShanheSourceZhaoranSuitLostEvent(long ArmFrameId, int OwnerSeat, string SkillInstanceId,
    int ActualTurn, int PlayInstance, long BatchId, int MovementIndex, int MovementSequence, Suit Suit) : IGameEvent;
public sealed record ShanheSourceZhaoranSuitClaimedEvent(long FrameId, long ArmFrameId, int OwnerSeat,
    long BatchId, int MovementIndex, Suit Suit) : IGameEvent;

internal interface IShanheSourceZhaoranHost
{
    void Arm(ProgramSkillFrame frame);
    void ClaimSuit(ProgramSkillFrame frame);
}

internal abstract class ShanheSourceZhaoranDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler => new ShanheSourceZhaoranHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Reveal, static (_, _) => { });
}
internal sealed class ArmShanheSourceZhaoranDescriptor : ShanheSourceZhaoranDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ArmShanheSourceZhaoran;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.PlayPhaseStarting)];
}
internal sealed class ClaimShanheSourceZhaoranSuitDescriptor : ShanheSourceZhaoranDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimShanheSourceZhaoranSuit;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsMoved)];
}
internal sealed class ShanheSourceZhaoranHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        var capability = host.GetCapability<IShanheSourceZhaoranHost>();
        if (op == SkillProgramEffectOp.ArmShanheSourceZhaoran) capability.Arm(frame);
        else capability.ClaimSuit(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
