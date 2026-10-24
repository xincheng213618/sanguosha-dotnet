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
    [ProgramCapability]
    private sealed class GuanduEventCapability(GameEngine engine) : IGuanduEventHost
    { public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => engine.ExecuteGuanduEvent(f, op); }
    private GuanduBattleEvent? CurrentGuanduEvent => GuanduRules?.CurrentEvent;
    private void RecordGuanduCardAction(CardActionContext action, SkillProgramTriggerWindow window) =>
        GuanduRules?.RecordCardAction(new(action.ActionId, action.Type, action.ActorSeat, action.EffectiveKind), window);
    private bool CanUseGuanduRoundConversion(IndexedSkillProgramInstance instance, SkillProgramViewAs rule, CharacterState owner) =>
        GuanduRules?.CanUseRoundConversion(instance.SkillId, rule.Id, owner.Seat) ??
            (instance.SkillId != "guandu:battle-event" || rule.Id != "hold-line");
    private void ConsumeGuanduRoundConversion(CardConversionSource source)
    {
        if (GuanduRules is { } rules) rules.ConsumeRoundConversion(source);
        else if (source.SkillId == "guandu:battle-event" && source.BindingId == "hold-line")
            throw new InvalidOperationException("The activity conversion feature is not installed.");
    }
    private CardUseFrame? GuanduDamageRewardUse(int owner, ProgramSkillWindowContext context) =>
        context.SourceSeat == owner && CurrentDamageAttempt is { IsSourceLess: false } damage && damage.SourceSeat == owner &&
        LifecycleCardUse(damage.ResolutionId) is { Action.Type: CardActionType.Use } use &&
        GuanduRules?.DamageRewardAvailable(use.Id) == true ? use : null;
    private CardUseFrame? GuanduTenthUse(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } c ||
            LifecycleCardUse(c.ParentCardUseFrameId) is not { Action: { } action, ExtraCardEffect: null } use ||
            action.ActionId != c.CardActionId || action.ActorSeat != owner || action.Type != CardActionType.Use || !ExtraEffectCardKind(use.CardKind) ||
            use.CardKind is CardKind.Dodge or CardKind.Nullification) return null;
        return GuanduRules?.IsTenthAction(action.ActionId) == true ? use : null;
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46000)]
    private bool AllowsGuanduEvent(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != "guandu:battle-event") return true;
        var context = gate.WindowContext;
        return GuanduRules?.AllowsEvent(new(gate.SkillId, gate.Candidate.BindingId, gate.OwnerSeat,
            context is not null, context?.Amount ?? 0,
            context is not null && GuanduTenthUse(gate.OwnerSeat, context) is not null,
            context is not null && GuanduDamageRewardUse(gate.OwnerSeat, context) is not null)) ?? false;
    }
    private SkillProgramStepOutcome ExecuteGuanduEvent(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var frame = GetActiveProgramFrame(input.Id);
        if (frame.WindowContext is null || frame.GuanduEventFlow is not null)
            throw new InvalidOperationException("A Guandu event lost its exact once-issued program.");
        return (GuanduRules ?? throw new InvalidOperationException("The activity event feature is not installed."))
            .ExecuteEvent(frame.Id, frame.OwnerSeat, op);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 46000)]
    private bool ResumeGuanduEventFlow(long id) => GuanduRules?.ResumeEvent(id) ?? false;
    [ProgramNativeDrawReturn(SkillProgramEffectOp.RewardGuanduDamage)]
    private void ReturnGuanduEventDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.GuanduEventFlow is not { Operation: SkillProgramEffectOp.RewardGuanduDamage } ||
            outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("A Guandu damage reward lost its exact native draw.");
        CompleteProgramDrawCards(f.Id, f.OwnerSeat, null, SkillProgramCardSetVisibility.Private, new("guandu.grain-shortage.draw"), 0, outcome.ActualDrawIds);
        AdvanceRuntimeProgram(f.Id);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46000)]
    private bool RetainGuanduEventFlow(ProgramSkillFrame f) => f.GuanduEventFlow is not null;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 46000)]
    private void AssertGuanduEventFlow(ProgramSkillFrame f, ProgramExecutionPlan plan)
    {
        if (f.GuanduEventFlow is not { } r) return;
        if (!IsGuanduActivity || f.SkillId != "guandu:battle-event" || r.ActualTurnNumber != _turnNumber ||
            plan.GetPausedInstruction(f.InstructionIndex).Effect.Op != r.Operation ||
            ProgramEventHistory<GuanduEventFlowIssuedEvent>().Count(e => e.ProgramFrameId == f.Id &&
                e.OwnerSeat == f.OwnerSeat && e.Operation == r.Operation && e.ActualTurnNumber == r.ActualTurnNumber && e.UseFrameId == r.UseFrameId) != 1 ||
            (r.Operation == SkillProgramEffectOp.RewardGuanduDamage
                ? r.UseFrameId is not > 0 || ProgramEventHistory<GuanduDamageRewardedEvent>().Count(e => e.UseFrameId == r.UseFrameId && e.SourceSeat == f.OwnerSeat) != 1
                : r.Operation != SkillProgramEffectOp.ResolveGuanduArrogance || r.UseFrameId is not null))
            throw new InvalidOperationException("A Guandu event lost its exact instruction and once-issued payment.");
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10, group: 10)]
    private bool IsGuanduArroganceDying() => _resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault() is
        { GuanduEventFlow.Operation: SkillProgramEffectOp.ResolveGuanduArrogance } f && ActiveDying is { ResumesProgramSkill: true } d &&
        d.ParentFrameId == f.Id && d.VictimSeat == f.OwnerSeat;

    [ProgramCardLegality(ProgramCardLegalityStage.ConversionUsageBan, 30)]
    private bool GuanduRoundConversionUsageUnavailable(ProgramConfiguredViewAsQuery query) =>
        !CanUseGuanduRoundConversion(query.Instance, query.Rule, query.Owner);
}
