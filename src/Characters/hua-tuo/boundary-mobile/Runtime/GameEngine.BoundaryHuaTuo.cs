using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record BoundaryHuaTuoQingnangReceipt(CardConversionSource Source, string Hash, SkillGrantPhaseExpiry Phase,
    int Target, int CardId, CardKind Kind, Suit Suit, bool Red, long Before, long MovementSequence = 0);
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryHuaTuoQingnangReceipt? BoundaryHuaTuoQingnang { get; init; } }
public sealed record BoundaryHuaTuoQingnangPaidEvent(long FrameId, BoundaryHuaTuoQingnangReceipt Payment) : IGameEvent;

internal interface IBoundaryHuaTuoSelection : IProgramSelectionQuery
{
    int PhaseLimit(int owner);
    bool MaySelect(int owner, int target);
}
[ProgramSelectionModule(ProgramSelectionStage.PhaseLimit, 60910, SkillProgramEffectOp.Recover)]
internal sealed class BoundaryHuaTuoQingnangPhaseLimit : IProgramSelectionPhaseLimitProvider
{
    public int? GetLimit(ProgramSelectionContext context, IProgramSelectionQueries queries) => context.SkillId == "boundary:qingnang-mobile"
        ? queries.Get<IBoundaryHuaTuoSelection>().PhaseLimit(context.OwnerSeat) : context.Activation.UsesPerPhase;
}
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 60910, SkillProgramEffectOp.Recover)]
internal sealed class BoundaryHuaTuoQingnangTargetFilter : IProgramSelectionTargetFilter
{
    public bool Allows(ProgramSelectionContext context, int targetSeat, IProgramSelectionQueries queries) => context.SkillId != "boundary:qingnang-mobile" ||
        queries.Get<IBoundaryHuaTuoSelection>().MaySelect(context.OwnerSeat, targetSeat);
}
[ProgramSelectionValidator(ProgramSelectionValidationStage.AfterCardFilters, 60910, SkillProgramEffectOp.Recover)]
internal sealed class BoundaryHuaTuoQingnangTargetValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries) => request.Context.SkillId != "boundary:qingnang-mobile" ||
        request.Targets is [var target] && queries.Get<IBoundaryHuaTuoSelection>().MaySelect(request.Context.OwnerSeat, target)
        ? null : new(CommandErrorCode.IllegalAction, "青囊不能再次选择本回合已选择的角色。");
}
internal static class BoundaryHuaTuoComposition
{
    [ProgramCatalogValidation(60910)]
    private static void Validate(string path, SkillProgram p)
    {
        if (p.Id != "boundary:qingnang-mobile") return;
        if (p.Triggers.Count != 0 || p.Activations is not [{ Id: "discard-and-heal", MinCards: 1, MaxCards: 1, SourceZones: [CardZoneKind.Hand],
            MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.AnyWounded, UsesPerTurn: null, UsesPerPhase: 1,
            Effects: [{ Op: SkillProgramEffectOp.DiscardSelected, Target: SkillProgramEffectTarget.Owner, Amount: 1 }, { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] }])
            throw new InvalidOperationException(path + ": mobile Qingnang requires one native hand discard and one original-target recovery with its red-payment phase allowance.");
    }
}

public sealed partial class GameEngine
{
    private const string BoundaryHuaTuoQingnang = "boundary:qingnang-mobile", BoundaryHuaTuoDiscardReason = "skill-program.boundary:qingnang-mobile.DiscardSelected";
    private SkillGrantPhaseExpiry? BoundaryHuaTuoCurrentPlay(int owner) => _phase == TurnPhase.Play && _currentSeat == owner && _turnNumber > 0 && _cardUseDebitPhaseInstanceId > 0
        ? new(_turnNumber, owner, _cardUseDebitPhaseInstanceId) : null;
    private bool ValidBoundaryHuaTuoPayment(BoundaryHuaTuoQingnangPaidEvent e)
    {
        var r = e.Payment;
        return r.Source.SkillId == BoundaryHuaTuoQingnang && r.Source.BindingId == "discard-and-heal" && IsValidPlayerSeat(r.Source.OwnerSeat) && IsValidPlayerSeat(r.Target) &&
            r.Phase.TurnNumber > 0 && r.Phase.PhaseActorSeat == r.Source.OwnerSeat && r.Phase.PhaseInstanceId > 0 && r.MovementSequence > r.Before &&
            r.Hash == _contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash && r.Red == (r.Suit is Suit.Heart or Suit.Diamond) &&
            ProgramEventHistory<ProgramSkillStartedEvent>().Count(start => start.FrameId == e.FrameId && start.OwnerSeat == r.Source.OwnerSeat && start.SkillId == r.Source.SkillId && start.ActivationId == r.Source.BindingId) == 1 &&
            _cardMovements.Count(m => m.Sequence == r.MovementSequence && m.CardId == r.CardId && m.CardKind == r.Kind && m.From == CardLocation.Hand(r.Source.OwnerSeat) &&
                m.To == NormalizeEphemeralCardDestination(GetAdvancedCard(r.CardId), m.From, CardLocation.DiscardPile, new(BoundaryHuaTuoDiscardReason)) && m.Reason.Value == BoundaryHuaTuoDiscardReason) == 1;
    }
    private IEnumerable<BoundaryHuaTuoQingnangPaidEvent> BoundaryHuaTuoPayments(int owner) => ProgramEventHistory<BoundaryHuaTuoQingnangPaidEvent>()
        .Where(e => e.Payment.Source.OwnerSeat == owner && ValidBoundaryHuaTuoPayment(e));
    [ProgramCapability] private sealed class BoundaryHuaTuoSelection(GameEngine engine) : IBoundaryHuaTuoSelection
    {
        public int PhaseLimit(int owner) => engine.BoundaryHuaTuoCurrentPlay(owner) is { } phase
            ? 1 + engine.BoundaryHuaTuoPayments(owner).Count(e => e.Payment.Phase == phase && e.Payment.Red) : 0;
        public bool MaySelect(int owner, int target) => !engine.BoundaryHuaTuoPayments(owner).Any(e => e.Payment.Phase.TurnNumber == engine._turnNumber && e.Payment.Target == target);
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 60910)]
    private void CaptureBoundaryHuaTuoStart(ProgramSkillStartedEvent started)
    {
        if (started.SkillId != BoundaryHuaTuoQingnang) return;
        var f = GetActiveProgramFrame(started.FrameId);
        if (f.TriggerId is not null || f.ActivationId != "discard-and-heal" || f.InstructionIndex != 0 || f.BoundaryHuaTuoQingnang is not null ||
            f.SelectedCardIds is not [var cardId] || f.SelectedTargetSeats is not [var target] || BoundaryHuaTuoCurrentPlay(f.OwnerSeat) is not { } phase ||
            _cardZones.GetLocation(cardId) != CardLocation.Hand(f.OwnerSeat)) throw new InvalidOperationException("Qingnang must freeze its real owner hand and actual Play before payment.");
        var card = GetAdvancedCard(cardId); var suit = EffectiveSuit(_players[f.OwnerSeat], card);
        ReplaceRuntimeFrame(f.Id, f with { BoundaryHuaTuoQingnang = new(new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, phase, target, cardId, card.Kind, suit, suit is Suit.Heart or Suit.Diamond, _movementSequence) });
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 60911)]
    private void CaptureBoundaryHuaTuoPayment(CardMovedEvent moved)
    {
        if (moved.Reason.Value != BoundaryHuaTuoDiscardReason || _resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryHuaTuoQingnang: { MovementSequence: 0 } r } f ||
            moved.CardId != r.CardId || moved.From != CardLocation.Hand(f.OwnerSeat) || f.InstructionIndex != 1 ||
            f.SelectedCardPayment is not { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: false } payment || !payment.CardIds.SequenceEqual([r.CardId])) return;
        var movement = _cardMovements.Last(m => m.CardId == moved.CardId && m.From == moved.From && m.To == moved.To && m.Reason == moved.Reason);
        var paid = r with { MovementSequence = movement.Sequence };
        ReplaceRuntimeFrame(f.Id, f with { BoundaryHuaTuoQingnang = paid });
        AdvanceEventRulesAndQueueFact(new BoundaryHuaTuoQingnangPaidEvent(f.Id, paid));
    }
    private bool ValidBoundaryHuaTuoFrame(ProgramSkillFrame f) => f.BoundaryHuaTuoQingnang is { } r && f.SkillId == BoundaryHuaTuoQingnang && f.ActivationId == "discard-and-heal" && f.TriggerId is null &&
        r.Source == new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId) && r.Hash == f.GameplayHash && r.Hash == _contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash &&
        f.SelectedCardIds.SequenceEqual([r.CardId]) && f.SelectedTargetSeats.SequenceEqual([r.Target]) && r.Phase.PhaseActorSeat == f.OwnerSeat &&
        (r.MovementSequence == 0 ? _cardZones.GetLocation(r.CardId) == CardLocation.Hand(f.OwnerSeat) :
            ProgramEventHistory<BoundaryHuaTuoQingnangPaidEvent>().Count(e => e.FrameId == f.Id && e.Payment == r && ValidBoundaryHuaTuoPayment(e)) == 1);
    [ProgramContinuation(ProgramContinuationKind.Movement, 60910)]
    private bool ReturnPaidBoundaryHuaTuoDiscard(ProgramSkillFrame f)
    {
        if (f.BoundaryHuaTuoQingnang is null || f.SelectedCardPaymentResult is not null) return false;
        if (f.BoundaryHuaTuoQingnang is not { MovementSequence: > 0 } r || !ValidBoundaryHuaTuoFrame(f) || f.InstructionIndex != 1 ||
            _resolutionStack.LastOrDefault()?.Id != f.Id || f.PendingMovementContinuation is not null ||
            f.SelectedCardPayment is not { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: true, ActiveChildFrameId: null } payment ||
            !payment.CardIds.SequenceEqual([r.CardId]) || !payment.CardIds.SequenceEqual(f.SelectedCardIds))
            throw new InvalidOperationException("Qingnang cannot return before its exact original paid hand discard and movement children complete.");
        ReplaceRuntimeTop(f with { SelectedCardPaymentResult = new(payment.InstructionIndex, payment.Operation, payment.LastCompletedChildFrameId, true) });
        AdvanceRuntimeProgram(f.Id); return true;
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 60910)]
    private void AssertBoundaryHuaTuoFrame(ProgramSkillFrame f)
    { if (f.BoundaryHuaTuoQingnang is not null && !ValidBoundaryHuaTuoFrame(f)) throw new InvalidOperationException("Qingnang lost its frozen original hand payment, target or phase."); }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 60910)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 60910)]
    private bool KeepPaidBoundaryHuaTuo(ProgramSkillFrame f) => f.BoundaryHuaTuoQingnang is { MovementSequence: > 0 } && ValidBoundaryHuaTuoFrame(f);
}
