using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record LingkongGainEntity(int CardId, CardKind PrintedKind, int GainSequence);

public sealed record LingkongGainCapture(int RecipientSeat, int ActualTurn, int TurnOwner,
    TurnPhase Phase, int PhaseActorSeat, bool First, CardConversionSource? Source, string? GameplayHash)
{
    private readonly IReadOnlyList<LingkongGainEntity> _entities = Array.AsReadOnly(Array.Empty<LingkongGainEntity>());
    public IReadOnlyList<LingkongGainEntity> Entities
    { get => _entities; init => _entities = Array.AsReadOnly(value.ToArray()); }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? NativeDrawInvocationId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? NativeDrawAttemptId { get; init; }
}

public sealed partial record CardMovementBatchContext
{
    private readonly IReadOnlyList<LingkongGainCapture>? _lingkongGains;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<LingkongGainCapture>? LingkongGains
    {
        get => _lingkongGains;
        init => _lingkongGains = value is null ? null : Array.AsReadOnly(value.Select(r => r with { Entities = r.Entities }).ToArray());
    }
}

public sealed record LingkongOutsideGainObservedEvent(long BatchId, int RecipientSeat, int ActualTurn,
    int TurnOwner, TurnPhase Phase, int PhaseActorSeat, int EntityCount,
    int SequenceBefore, int SequenceAfter) : IGameEvent;
public sealed record LingkongGainQualifiedEvent(long BatchId, CardConversionSource Source,
    string GameplayHash, int ActualTurn, int TurnOwner, int EntityCount) : IGameEvent;
public sealed record LingkongGainEntityIssuedEvent(long BatchId, int RecipientSeat, int CardId,
    CardKind PrintedKind, int GainSequence) : IGameEvent;
public sealed record LingkongGainMarkedEvent(long FrameId, long BatchId, CardConversionSource Source,
    string GameplayHash, int ActualTurn, int TurnOwner, int EntityCount) : IGameEvent;
public sealed record LingkongGainInvocationBoundEvent(long BatchId, int RecipientSeat,
    long? NativeDrawInvocationId, long? NativeDrawAttemptId, long? ParentFrameId,
    int SequenceBefore, int SequenceAfter, int EntityCount) : IGameEvent;

internal interface ILeZhouFeiLingkongHost
{
    SkillProgramStepOutcome MarkLingkongGain(ProgramSkillFrame frame);
}

internal abstract class LingkongDescriptor(SkillProgramEffectOp op) : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => op;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy { get; } =
        new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(1));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsGained)];
    public override ISkillProgramEffectHandler Handler { get; } = new LingkongHandler(op);
}

internal sealed class MarkLingkongFirstOutsideDrawGainDescriptor()
    : LingkongDescriptor(SkillProgramEffectOp.MarkLingkongFirstOutsideDrawGain){
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.State.CanContinueAfterOwnerDeath(context.Frame);
}

internal sealed class LingkongHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int target, ISkillProgramEffectHost host) => host.GetCapability<ILeZhouFeiLingkongHost>().MarkLingkongGain(frame);
}

internal static class LingkongComposition
{
    [ProgramCatalogValidation(850)]
    internal static void ValidateProgram(string path, SkillProgram p)
    {
        var effects = p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects));
        if (!effects.Any(e => e.Op is SkillProgramEffectOp.MarkLingkongFirstOutsideDrawGain)) return;
        if (p.Id != "le:lingkong" || p.Activations.Count != 0 || p.ViewAs.Count != 0 ||
            p.Modifiers.Count != 0 || p.CardPolicies.Count != 0 || p.CardIdentities.Count != 0 ||
            p.SkillGrants.Count != 0 || p.DamageModifiers.Count != 0 || p.BooleanStates.Count != 0 ||
            p.Contributions.Count != 0 || p.Triggers.Count != 2 ||
            p.Triggers.SingleOrDefault(t => t.Id == "mark-first-outside-draw-gain") is not { } gained ||
            gained.Window != SkillProgramTriggerWindow.CardsGained ||
            gained.DestinationZones is not [CardZoneKind.Hand] ||
            gained.MovementOccurrence != SkillProgramMovementOccurrence.PerBatch ||
            gained.Effects is not [{ Op: SkillProgramEffectOp.MarkLingkongFirstOutsideDrawGain,
                Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] ||
            !Plain(gained, true))
            throw new InvalidOperationException(path + ": Lingkong requires its original opening entities and one exact first outside-own-Draw gain batch.");
    }

    private static bool Plain(SkillProgramTrigger t, bool gain) =>
        t.Subject == SkillProgramTriggerSubject.Owner && !t.Optional &&
        t.Condition.Kind == SkillProgramTriggerConditionKind.Always && t.UsageLimit is null &&
        t.UsageScope is null && t.DynamicUsageLimit is null && t.NamedUsageGroup is null &&
        t.MarkerCost is null && t.ChoiceGroup is null && t.OrderGroup is null && !t.EvaluateConditionAtResolution &&
        t.OwnerRelation is null && t.SourceSkillId is null && t.SourceViewAsId is null &&
        t.CardKinds.Count == 0 && t.CardCategories.Count == 0 && t.DamageCardKinds.Count == 0 &&
        t.Suits.Count == 0 && t.SourceZones.Count == 0 && (gain || t.DestinationZones.Count == 0) &&
        t.MovementReasons.Count == 0 && t.ExcludedMovementReasons.Count == 0 && t.ExcludedReasons.Count == 0 &&
        (gain || t.MovementOccurrence is null) && !t.MovementDiscardOnly && !t.IncludeResponseUses && !t.SingleActionInstance &&
        !t.DeferredTurnEndOnly && !t.IgnoreOwnSkillMovements && t.GainPhaseQualification is null &&
        t.DamageOccurrence is null && !t.NoDyingAtActivation && t.RequireDamageSource is null &&
        t.RequireNoCardConversion is null && !t.AllowNoEventTarget && !t.OnlyDesignatedCardTargets &&
        !t.AllowOwnDiscardPhaseEnded;
}
