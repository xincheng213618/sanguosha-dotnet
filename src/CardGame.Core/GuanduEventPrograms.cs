using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IGuanduEventHost
{
    SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation);
}
internal abstract class GuanduEventDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new GuanduEventHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [];
}
internal sealed class ResolveGuanduRepeatDescriptor : GuanduEventDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveGuanduRepeat;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted)]; }
internal sealed class RewardGuanduDamageDescriptor : GuanduEventDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewardGuanduDamage;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)]; }
internal sealed class ResolveGuanduArroganceDescriptor : GuanduEventDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveGuanduArrogance;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding)]; }
internal sealed class GuanduEventHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IGuanduEventHost>().Execute(f, operation);
}
internal sealed class GuanduEventCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "guandu:battle-event";
    public void ValidateTrigger(SkillProgramTrigger t)
    {
        if (t.Id != "tenth-use" || t.Window != SkillProgramTriggerWindow.CardUseCommitted ||
            t.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || !t.IncludeResponseUses || t.Optional ||
            t.Condition.Kind != SkillProgramTriggerConditionKind.Always || t.CardKinds.Count != 0 || t.CardCategories.Count != 0 ||
            t.Effects is not [{ Op: SkillProgramEffectOp.ResolveGuanduRepeat, Target: SkillProgramEffectTarget.Owner,
                Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException("The Guandu tenth-card event requires its exact mandatory actual-use contract.");
    }
}
public sealed record GuanduActualCardActionEvent(long ActionId, long UseFrameId, CardActionType Type,
    int ActorSeat, CardKind Kind, int ActualTurnNumber, ActualDiscardRecoveryPhaseKey? Phase, int UseOrdinal) : IGameEvent
{
    public bool CountsAsUse { get; init; }
}
public sealed record GuanduDamageRewardedEvent(long UseFrameId, int SourceSeat) : IGameEvent;
public sealed record GuanduSlowAdvanceEarnedEvent(ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
public sealed record GuanduSlowAdvanceConsumedEvent(long DrawFrameId, int OwnerSeat, int Credits) : IGameEvent;
public sealed record GuanduHoldLineUsedEvent(int RoundNumber, int OwnerSeat, int ActualTurnNumber) : IGameEvent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ExcludedDiscardPhaseToken { get; init; }
}
public sealed record GuanduHoldLineConsumedEvent(ActualDiscardRecoveryPhaseKey Phase, int Credits) : IGameEvent;
public sealed record GuanduEventFlowIssuedEvent(long ProgramFrameId, SkillProgramEffectOp Operation,
    int OwnerSeat, int ActualTurnNumber, long? UseFrameId) : IGameEvent;
public sealed record GuanduEventFlowReceipt(SkillProgramEffectOp Operation, int ActualTurnNumber, long? UseFrameId);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GuanduEventFlowReceipt? GuanduEventFlow { get; init; }
}

public sealed partial class GameEngine
{
}
