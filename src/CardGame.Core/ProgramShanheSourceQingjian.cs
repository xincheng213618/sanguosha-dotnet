using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheSourceQingjianReceipt(long BatchId, int InstructionIndex,
    IReadOnlyList<int> OriginalCardIds, IReadOnlyList<int> GivenCardIds, bool ReturningChild = false,
    int PaidCardId = 0, int RecipientSeat = -1, long Before = 0, long After = 0);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceQingjianReceipt? ShanheSourceQingjian { get; init; }
}

public sealed record ShanheSourceQingjianGivenEvent(long FrameId, long OriginalBatchId, int OwnerSeat,
    int RecipientSeat, int CardId, long Before, long After) : IGameEvent;

internal interface IShanheSourceQingjianHost
{
    SkillProgramStepOutcome DistributeOriginalGains(ProgramSkillFrame frame);
}
internal sealed class DistributeShanheSourceQingjianGainsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DistributeShanheSourceQingjianGains;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler { get; } = new DistributeShanheSourceQingjianGainsHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.DistributeOwnedCards, static (_, _) => { });
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsGained)];
}
internal sealed class DistributeShanheSourceQingjianGainsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DistributeShanheSourceQingjianGains;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheSourceQingjianHost>().DistributeOriginalGains(frame);
}
