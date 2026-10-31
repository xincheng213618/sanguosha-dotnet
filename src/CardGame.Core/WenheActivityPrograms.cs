using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IWenheActivityProgramHost
{
    SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op);
}
internal abstract class WenheActivityOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new WenheActivityEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [];
}
internal sealed class ResolveWenheRoundStartDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheRoundStart;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.RoundStarting)]; }
internal sealed class ResolveWenheTurnStartDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheTurnStart;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)]; }
internal sealed class ResolveWenheTurnEndDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheTurnEnd;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding)]; }
internal sealed class ResolveWenheRoundEndDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheRoundEnd;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.RoundEnding)]; }
internal sealed class RewardWenheKillerDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewardWenheKiller;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CharacterDied)]; }
internal sealed class UseWenheChampionMarkDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.UseWenheChampionMark; }
internal sealed class WenheActivityEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) =>
        h.GetCapability<IWenheActivityProgramHost>().ExecuteWenheActivity(f, op);
}
public sealed record WenheRuleFlowReceipt(SkillProgramEffectOp Operation, int RoundNumber, int TurnNumber,
    int Amount, int Index, int Stage)
{
    private readonly IReadOnlyList<int> _seats = Array.Empty<int>();
    public IReadOnlyList<int> Seats { get => _seats; init => _seats = Array.AsReadOnly(value.ToArray()); }
}
public sealed record WenheRuleFlowIssuedEvent(long ProgramFrameId, SkillProgramEffectOp Operation, int RoundNumber,
    int TurnNumber, int OwnerSeat, int Count, int Amount) : IGameEvent;
public sealed record WenheRuleParticipantEvent(long ProgramFrameId, int Index, int Seat) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenheRuleFlowReceipt? WenheRuleFlow { get; init; }
}
