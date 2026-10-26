namespace CardGame.Core;

// All payloads are scalar. No hidden skill identity or hand content is published.
public sealed record ActualTurnFirstOwnedDiscardBatchEvent(int ActualTurnNumber, int TurnOwnerSeat,
    int DiscardOwnerSeat, long BatchId) : IGameEvent;
public sealed record DiscardNeighborhoodFrozenEvent(long BatchId, int ActualTurnNumber, int TurnOwnerSeat,
    int OwnerSeat, int PreviousLivingSeat, bool IncludesOwnDiscard, bool IncludesFirstPreviousDiscard) : IGameEvent;
public sealed record RevealedCardHpComparedEvent(long ProgramFrameId, int InstructionIndex,
    CardConversionSource Source, string GameplayHash, int CardId, CardKind RevealedKind,
    int TargetSeat, int OwnerHp, int TargetHp, bool LosesHp) : IGameEvent;

internal sealed partial class ProgramAiEstimateContext
{
    // Bounded prior for the optional zero-cost public ordering opportunity.
    // This does not inspect a hand, deck top, skill grant or random state.
    internal void PriceOptionalDiscardTop() =>
        _otherAdjustment += _publicContext.EventMovedCardCount is > 0 ? 0.5d : 0d;
}

internal sealed class PutOwnOrPreviousFirstDiscardOnTopDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PutOwnOrPreviousFirstDiscardOnTop;
    public override ISkillProgramEffectHandler Handler { get; } = new PutOwnOrPreviousFirstDiscardOnTopHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Move,
        static (_, context) => context.PriceOptionalDiscardTop());
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsMoved)];
}

public sealed class PutOwnOrPreviousFirstDiscardOnTopHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PutOwnOrPreviousFirstDiscardOnTop;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IProgramDiscardTopPlacementHost>().PutDiscardedCardsOnDrawPileTop(f);
}

internal interface IRevealedCardHpComparisonHost
{
    SkillProgramStepOutcome LoseHpIfRevealedNonEquipmentDiffers(ProgramSkillFrame frame, string sourceBind);
}

internal sealed class LoseHpIfRevealedNonEquipmentDiffersDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.LoseHpIfRevealedNonEquipmentDiffers;
    public override ISkillProgramEffectHandler Handler { get; } = new LoseHpIfRevealedNonEquipmentDiffersHandler();
    // The draw is unknown when choosing a target. The existing draw/recovery
    // estimate is retained; this branch never inspects the private deck top.
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.LoseHp, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.SelectedTarget)
            throw new InvalidOperationException("A revealed-card HP comparison requires the exact selected draw recipient.");
        var e = new SkillProgramEffect(Op, SkillProgramEffectTarget.SelectedTarget, 1, r.Condition(), sourceBind: r.RequiredIdentifier("sourceBind"));
        RequireAlways(e, r.Path); return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        // This reads metadata from its prior exact public Reveal event even if
        // the equipment child has consumed the original binding's entity.
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding), new ReadSelectedTarget(), new ReadCardSet(e.SourceBind!)];

    internal static void Validate(string path, IReadOnlyList<SkillProgramEffect> effects,
        SkillProgramTriggerWindow window, SkillProgramTriggerSubject? subject,
        SkillProgramMovementOccurrence? occurrence, bool discardOnly, IReadOnlyList<CardZoneKind> sourceZones,
        SkillProgramTurnOwnerScope scope, IReadOnlyList<string> movementReasons,
        IReadOnlyList<string> excludedMovementReasons, bool ignoreOwnSkillMovements)
    {
        if (effects.Any(e => e.Op == SkillProgramEffectOp.PutOwnOrPreviousFirstDiscardOnTop) &&
            (window != SkillProgramTriggerWindow.CardsMoved || subject != SkillProgramTriggerSubject.Owner ||
             occurrence != SkillProgramMovementOccurrence.PerOwnerBatch || !discardOnly || effects.Count != 1 ||
             movementReasons.Count != 0 || excludedMovementReasons.Count != 0 || ignoreOwnSkillMovements ||
             !sourceZones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment])))
            throw new InvalidOperationException($"Invalid skill program at {path}: neighbor discard-top requires one owner-batch actual HEJ-discard instruction.");
        var comparison = effects.LastOrDefault();
        if (scope == SkillProgramTurnOwnerScope.OwnOrPreviousLiving || effects.Any(e => e.Op == SkillProgramEffectOp.LoseHpIfRevealedNonEquipmentDiffers))
        {
            bool IsEquipmentCondition(SkillProgramEffect e) => e.Condition.Kind == SkillProgramConditionKind.BoundCardsMatchCategories &&
                e.Condition.SourceBind == "equipment" && e.Condition.CardCategories.SequenceEqual([SkillProgramCardCategory.Equipment]);
            var available = effects.Count == 8;
            var offset = available ? 1 : 0;
            var revealedBind = available ? "available-drawn" : "drawn";
            if (window != SkillProgramTriggerWindow.TurnEnding || subject != SkillProgramTriggerSubject.Owner ||
                scope != SkillProgramTurnOwnerScope.OwnOrPreviousLiving || effects.Count is not (7 or 8) ||
                effects[0] is not { Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.AnyLiving, Condition.Kind: SkillProgramConditionKind.Always } ||
                effects[1] is not { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1,
                    ResultBind: "drawn", NumberExpression: null, Condition.Kind: SkillProgramConditionKind.Always } ||
                available && (effects[2] is not { Op: SkillProgramEffectOp.FilterBoundCards, SourceBind: "drawn", ResultBind: "available-drawn", AvailableAtSourceOnly: true,
                    Condition.Kind: SkillProgramConditionKind.Always, TargetReference: null, MatchSuitOfBind: null } ||
                    !effects[2].CardCategories.SequenceEqual([SkillProgramCardCategory.Basic, SkillProgramCardCategory.Trick, SkillProgramCardCategory.Equipment]) ||
                    effects[2].Suits.Count + effects[2].EquipmentSlots.Count + effects[2].CardKinds.Count != 0) ||
                effects[2 + offset] is not { Op: SkillProgramEffectOp.RevealBoundCards, Condition.Kind: SkillProgramConditionKind.Always } ||
                effects[2 + offset].SourceBind != revealedBind ||
                effects[3 + offset] is not { Op: SkillProgramEffectOp.FilterBoundCards, ResultBind: "equipment", Condition.Kind: SkillProgramConditionKind.Always } ||
                effects[3 + offset].SourceBind != revealedBind || !effects[3 + offset].CardCategories.SequenceEqual([SkillProgramCardCategory.Equipment]) ||
                effects[4 + offset] is not { Op: SkillProgramEffectOp.UseBoundCardByTarget, Target: SkillProgramEffectTarget.SelectedTarget, SourceBind: "equipment" } ||
                !IsEquipmentCondition(effects[4 + offset]) ||
                effects[5 + offset] is not { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1, NumberExpression: null } ||
                !IsEquipmentCondition(effects[5 + offset]) ||
                comparison is not { Op: SkillProgramEffectOp.LoseHpIfRevealedNonEquipmentDiffers, Condition.Kind: SkillProgramConditionKind.Always } || comparison.SourceBind != revealedBind)
                throw new InvalidOperationException($"Invalid skill program at {path}: own/previous Ending requires the exact draw/reveal/equipment/use/recovery/comparison composition.");
        }
    }
}

public sealed class LoseHpIfRevealedNonEquipmentDiffersHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.LoseHpIfRevealedNonEquipmentDiffers;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IRevealedCardHpComparisonHost>().LoseHpIfRevealedNonEquipmentDiffers(f, e.SourceBind!);
}
