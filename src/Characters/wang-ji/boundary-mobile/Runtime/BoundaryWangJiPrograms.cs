using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum BoundaryWangJiStage { Inspect, DiscardChildren, Charge, Drawing, DrawChildren }
public sealed record BoundaryWangJiReceipt(CardConversionSource Source, string GameplayHash, SkillProgramEffectOp Op,
    long WindowId, long ActionId, ActualDiscardRecoveryPhaseKey? Phase)
{
    public BoundaryWangJiStage Stage { get; init; }
    public int Cursor { get; init; }
    public int Requested { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public NativeDrawOutcome? Draw { get; init; }
    public BoundaryWangJiDiscardedEvent? Payment { get; init; }
    private IReadOnlyList<int> _targets = [],_viewed = [];
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> Viewed { get => _viewed; init => _viewed = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryWangJiReceipt? BoundaryWangJi { get; init; } }
public sealed record BoundaryWangJiInitializedEvent(long FrameId, int OwnerSeat) : IGameEvent;
public sealed record BoundaryWangJiTurnEvent(int TurnNumber, int OwnerSeat, int Ordinal, int InitialCharge) : IGameEvent;
public sealed record BoundaryWangJiRecipientEvent(long FrameId, CardConversionSource Source, string GameplayHash, ActualDiscardRecoveryPhaseKey Phase, int TargetSeat) : IGameEvent;
public sealed record BoundaryWangJiSpentEvent(long FrameId, CardConversionSource Source, string GameplayHash, ActualDiscardRecoveryPhaseKey Phase, int TargetSeat) : IGameEvent;
public sealed record BoundaryWangJiUseEvent(long FrameId, int OwnerSeat, int TurnOrdinal, long ActionId) : IGameEvent;
public sealed record BoundaryWangJiStartedEvent(long FrameId, CardConversionSource Source, string GameplayHash, SkillProgramEffectOp Op, long WindowId, long ActionId, ActualDiscardRecoveryPhaseKey? Phase) : IGameEvent;
public sealed record BoundaryWangJiInspectionEvent(long FrameId, int Index, int TargetSeat, int Count) : IGameEvent;
public sealed record BoundaryWangJiDiscardedEvent(long FrameId, int Index, int TargetSeat, int CardId, CardKind Kind, CardLocation From, CardLocation To, int Sequence) : IGameEvent;
public sealed record BoundaryWangJiDrawReturnedEvent(long FrameId, int Requested, int Actual, long? AttemptFrameId) : IGameEvent;
public sealed record BoundaryWangJiPeakFrozenEvent(long FrameId, int OwnerSeat, int TurnOrdinal, int Amount) : IGameEvent;
public sealed record BoundaryWangJiChargeGrantedEvent(long FrameId, int RecipientSeat, string SkillId) : IGameEvent;

internal interface IBoundaryWangJiHost { SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation); }
internal sealed class BoundaryWangJiHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{ public SkillProgramEffectOp Op => op; public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryWangJiHost>().Execute(frame, op); }
internal abstract class BoundaryWangJiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler => new BoundaryWangJiHandler(Op);
    public override ProgramOperationInteraction Interaction => Op is SkillProgramEffectOp.ResolveBoundaryWangJiQizhi or SkillProgramEffectOp.ResolveBoundaryWangJiJinqu ? ProgramOperationInteraction.Choice : ProgramOperationInteraction.Automatic;
    public override ProgramFactRequirements FactRequirements => new(Activated: ProgramTrackedFact.ActualDiscardRecoveryPhase, Triggered: ProgramTrackedFact.ActualDiscardRecoveryPhase | ProgramTrackedFact.NativeLogicalDiscard);
    public override ProgramDependencyKinds TracksActualTurnCursorReturn => ProgramDependencyKinds.Trigger;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.State.CanContinueAfterOwnerDeath(context.Frame);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, context) => context.PublicControlValue(2));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => Op switch {
        SkillProgramEffectOp.InitializeBoundaryWangJi => [new RequireTriggerWindow(SkillProgramTriggerWindow.GameStarting)],
        SkillProgramEffectOp.ResolveBoundaryWangJiQizhi => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseTargetsFinalized)], _ => [] };
}
internal sealed class InitializeBoundaryWangJiDescriptor : BoundaryWangJiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.InitializeBoundaryWangJi; }
internal sealed class SpendBoundaryWangJiQizhiDescriptor : BoundaryWangJiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.SpendBoundaryWangJiQizhi; }
internal sealed class ResolveBoundaryWangJiQizhiDescriptor : BoundaryWangJiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryWangJiQizhi; }
internal sealed class ResolveBoundaryWangJiJinquDescriptor : BoundaryWangJiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryWangJiJinqu; }
internal interface IBoundaryWangJiSelection : IProgramSelectionQuery { bool CanSpend(int owner, int target); }
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 54400, SkillProgramEffectOp.SpendBoundaryWangJiQizhi)]
internal sealed class BoundaryWangJiTargetFilter : IProgramSelectionTargetFilter
{ public bool Allows(ProgramSelectionContext context, int targetSeat, IProgramSelectionQueries queries) => queries.Get<IBoundaryWangJiSelection>().CanSpend(context.OwnerSeat, targetSeat); }

internal sealed class BoundaryWangJiCompletedContract : IProgramCommittedUseContract
{
    public string SkillId => "boundary:jinqu-mobile";
    public bool AllowsSingleActionInstance => true;
    public void ValidateTrigger(SkillProgramTrigger trigger)
    {
        if (trigger is not { Id: "paired-use-charge", Window: SkillProgramTriggerWindow.CardUseCompleted, OwnerRelation: SkillProgramCardActionOwnerRelation.Observer,
            IncludeResponseUses: true, SingleActionInstance: true, Optional: false, Effects: [{ Op: SkillProgramEffectOp.ResolveBoundaryWangJiJinqu }] })
            throw new InvalidOperationException("Mobile Jinqu counts each completed true basic/instant-trick use once.");
    }
}

internal static class BoundaryWangJiComposition
{
    [ProgramCatalogValidation(54400)]
    private static void Validate(string path, SkillProgram program)
    {
        foreach (var effect in program.Activations.SelectMany(activation => activation.Effects).Concat(program.Triggers.SelectMany(trigger => trigger.Effects)))
            if ((effect.Op is SkillProgramEffectOp.InitializeBoundaryWangJi or SkillProgramEffectOp.SpendBoundaryWangJiQizhi or SkillProgramEffectOp.ResolveBoundaryWangJiQizhi) && program.Id != "boundary:qizhi-mobile" ||
                effect.Op == SkillProgramEffectOp.ResolveBoundaryWangJiJinqu && program.Id != "boundary:jinqu-mobile")
                throw new InvalidOperationException(path + ": mobile Wang Ji operations belong to their exact printed skills.");
        if (program.Id == "boundary:qizhi-mobile" && (program.Activations is not [{ Id: "charge-draw", MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1,
                TargetKind: SkillProgramTargetKind.AnyLiving, UsesPerTurn: null, UsesPerPhase: null, UsesPerGame: null, Effects: [{ Op: SkillProgramEffectOp.SpendBoundaryWangJiQizhi }] }] ||
            program.Triggers is not [{ Id: "initial-charge", Window: SkillProgramTriggerWindow.GameStarting, Optional: false, Effects: [{ Op: SkillProgramEffectOp.InitializeBoundaryWangJi }] },
                { Id: "inspect-recipients", Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, Optional: false, Effects: [{ Op: SkillProgramEffectOp.ResolveBoundaryWangJiQizhi }] }]))
            throw new InvalidOperationException(path + ": mobile Qizhi needs initial 1/2 charge, a phase-recipient activation and the real target-designation inspection.");
        if (program.Id == "boundary:jinqu-mobile" && (program.Activations.Count != 0 || program.Triggers is not [var completed,
                { Id: "ending-peak", Window: SkillProgramTriggerWindow.TurnEnding, Optional: false, Effects: [{ Op: SkillProgramEffectOp.ResolveBoundaryWangJiJinqu }] }]))
            throw new InvalidOperationException(path + ": mobile Jinqu needs both completed paired uses and the owner's ending peak.");
    }
}
