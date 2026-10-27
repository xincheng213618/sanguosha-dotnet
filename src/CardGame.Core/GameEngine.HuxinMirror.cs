namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static readonly CardMoveReason HuxinMirrorPaymentReason = new("equipment.huxin-mirror.prevent");

    private Card? ActiveHuxinMirror(int owner, string instance) =>
        KongwuEffectiveEquipment(_players[owner]).FirstOrDefault(card => card.Kind == CardKind.HuxinMirror &&
            IsActiveProgramSourceEquipmentCard(owner, HuxinMirrorContract.SkillId, instance, card));

    private bool HuxinMirrorDamageQualifies(int owner, int amount, IDamageAttempt attack) => amount > 1 ||
        amount > 0 && _players[owner].HasHpValue && PublicDamagePrediction.Calculate(_players[owner].Hp,
            _players[owner].Armor, amount, maxHp: _players[owner].MaxHp,
            bypassArmor: BypassesNativeDamageArmor(attack), armorDamageMultiplier: NativeDamageArmorMultiplier(attack)).IsLethal;

    private bool HuxinMirrorIsEligible(int owner, string instance, ProgramSkillWindowContext window) =>
        window is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0 } &&
        window.TargetSeat == owner && _players[owner].IsAlive && !IsArmorIneffectiveForTurn(_players[owner]) &&
        ActiveHuxinMirror(owner, instance) is not null && CurrentDamageAttempt is { } attack &&
        attack.TargetSeat == owner && (attack.IsChainPropagation || !attack.IgnoresArmor) && !HasDirectedCardArmorBypass(attack.ResolutionId, owner) &&
        HuxinMirrorDamageQualifies(owner, window.Amount, attack);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62058,
        SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage)]
    private bool CanOfferHuxinMirror(ProgramTriggerLegalityContext context) =>
        context.SkillId == HuxinMirrorContract.SkillId && context.WindowContext is { } window &&
        HuxinMirrorIsEligible(context.OwnerSeat, context.Candidate.SkillInstanceId, window);

    [ProgramCapability]
    private sealed class HuxinMirrorCapability(GameEngine engine) : IHuxinMirrorHost
    {
        public SkillProgramStepOutcome PayAndPrevent(ProgramSkillFrame frame) => engine.PayHuxinMirrorAndPrevent(frame);
    }

    private SkillProgramStepOutcome PayHuxinMirrorAndPrevent(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.HuxinMirrorPayment is not null || frame.WindowContext is not
                { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context ||
            _resolutionStack.Count < 2 || _resolutionStack[^2] is not BeforeDamageProgramWindowFrame window ||
            window.Id != context.ParentFrameId || window.Prevented || window.TargetSeat != frame.OwnerSeat ||
            window.Amount != context.Amount || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].Candidate) ||
            !HuxinMirrorIsEligible(frame.OwnerSeat, frame.SkillInstanceId, context) ||
            ActiveHuxinMirror(frame.OwnerSeat, frame.SkillInstanceId) is not { } card || CurrentDamageAttempt is not { } attack)
            throw new InvalidOperationException("Huxin Mirror requires its exact unpaid armor and original damage candidate.");
        var receipt = new HuxinMirrorPaymentReceipt(frame.InstructionIndex, window.Id, attack.ResolutionId,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            card.Id, window.Amount, 0);
        ReplaceRuntimeTop(frame with { HuxinMirrorPayment = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        // This is the actual equipped entity put into discard, not a logical-discard instruction.
        MoveCard(card, CardLocation.Equipment(frame.OwnerSeat), CardLocation.DiscardPile, HuxinMirrorPaymentReason, move =>
        {
            var active = GetActiveProgramFrame(frame.Id);
            var paid = receipt with { MovementSequence = move.Sequence };
            ReplaceRuntimeTop(active with { HuxinMirrorPayment = paid });
            AdvanceEventRulesAndQueueFact(new HuxinMirrorPaidEvent(frame.Id, paid));
        });
        PreventProgramCurrentDamage(GetActiveProgramFrame(frame.Id));
        AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private bool ValidPaidHuxinMirror(ProgramSkillFrame frame)
    {
        if (frame.HuxinMirrorPayment is not { } paid || paid.InstructionIndex != frame.InstructionIndex ||
            paid.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            paid.GameplayHash != frame.GameplayHash || frame.WindowContext?.ParentFrameId != paid.WindowFrameId || paid.MovementSequence <= 0 ||
            !ProgramEventHistory<HuxinMirrorPaidEvent>().Any(e => e.ProgramFrameId == frame.Id && e.Receipt == paid) ||
            !_cardMovements.Any(move => move.Sequence == paid.MovementSequence && move.CardId == paid.CardId &&
                move.CardKind == CardKind.HuxinMirror && move.From == CardLocation.Equipment(frame.OwnerSeat) &&
                move.To == CardLocation.DiscardPile && move.Reason == HuxinMirrorPaymentReason)) return false;
        var index = _resolutionStack.FindIndex(f => f.Id == frame.Id);
        return index > 0 && _resolutionStack[index - 1] is BeforeDamageProgramWindowFrame window &&
            window.Id == paid.WindowFrameId && window.Prevented && window.Amount == paid.Amount &&
            (window.ContinuationAttackResolutionId ?? window.ParentFrameId) == paid.AttackFrameId;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62058)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62058)]
    private bool KeepPaidHuxinMirror(ProgramSkillFrame frame) => frame.HuxinMirrorPayment is not null && ValidPaidHuxinMirror(frame);

    [ProgramContinuation(ProgramContinuationKind.Resume, 62058)]
    private bool ResumePaidHuxinMirror(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { HuxinMirrorPayment: not null } frame || frame.Id != id ||
            frame.PendingMovementContinuation is null) return false;
        if (!ValidPaidHuxinMirror(frame)) throw new InvalidOperationException("Huxin Mirror lost its actual paid damage receipt.");
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(id) ||
            TryBeginAdvancedSkillsChanged(id)) return true;
        ReplaceRuntimeTop(GetActiveProgramFrame(id) with { PendingMovementContinuation = null });
        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62058)]
    private bool ReturnPaidHuxinMirrorMovement(ProgramSkillFrame frame)
    {
        if (frame.HuxinMirrorPayment is null) return false;
        if (frame.PendingMovementContinuation is null || !ValidPaidHuxinMirror(frame))
            throw new InvalidOperationException("Huxin Mirror lost its original movement child return.");
        AdvanceRuntimeProgram(frame.Id); return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62058)]
    private bool IsPaidHuxinMirrorMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage && frame.HuxinMirrorPayment is not null &&
        pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidPaidHuxinMirror(frame);

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62058)]
    private void AssertPaidHuxinMirror(ProgramSkillFrame frame)
    {
        if (frame.HuxinMirrorPayment is not null && !ValidPaidHuxinMirror(frame))
            throw new InvalidOperationException("Huxin Mirror lost its original damage parent, source or unique physical payment.");
    }

    private IEnumerable<LegalAction> HuxinMirrorPlacementActions(CharacterState actor, IReadOnlyList<Card> cards) =>
        cards.Where(card => card.Kind == CardKind.HuxinMirror && _cardZones.GetLocation(card.Id) == CardLocation.Hand(actor.Seat))
            .SelectMany(card => _players.Where(target => target.Seat != actor.Seat &&
                CanPlaceOwnedEquipment(actor.Seat, target.Seat, card, CardLocation.Hand(actor.Seat)))
                .Select(target => new LegalAction(LegalActionKind.Equip, card.Id, target.Seat,
                    $"将【护心镜】置于{target.Name}的装备区")));

    private bool TryPlaceHandHuxinMirror(CharacterState actor, LegalAction action)
    {
        if (action.Kind != LegalActionKind.Equip || action.TargetSeat is not { } target ||
            action.CardId is not { } id || GetAdvancedCard(id).Kind != CardKind.HuxinMirror) return false;
        var card = GetAdvancedCard(id);
        if (_resolutionStack.Count != 0 || _phase != TurnPhase.Play || _currentSeat != actor.Seat || target == actor.Seat ||
            !CanPlaceOwnedEquipment(actor.Seat, target, card, CardLocation.Hand(actor.Seat)))
            throw new InvalidOperationException("Huxin Mirror placement requires its original hand entity and live other equipment slot.");
        var frame = new HuxinMirrorPlacementFrame(++_resolutionSequence, actor.Seat, target, id, CardLocation.Hand(actor.Seat), _movementSequence);
        PushRuntimeFrame(frame);
        var replaced = GetEquipment(_players[target]).FirstOrDefault(c => FieldEquipmentSlot(c) == EquipmentSlot.Armor);
        if (GetEquipment(_players[target]).Count(c => FieldEquipmentSlot(c) == EquipmentSlot.Armor) <
            _players[target].EquipmentSlotCapacity(EquipmentSlot.Armor)) replaced = null;
        if (replaced is not null) MoveCard(replaced, CardLocation.Equipment(target), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        MoveCard(card, frame.From, CardLocation.Equipment(target), CardMoveReasons.EquipmentEnter);
        ReplaceRuntimeTop(frame with { SequenceAfter = _movementSequence });
        AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, target, EquipmentSlot.Armor, id, card.Kind, replaced?.Id));
        ContinueHuxinMirrorPlacement(frame.Id); return true;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 20746,
        NativeFrameAutomaticPending.Ignore, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(HuxinMirrorPlacementFrame), 20746)]
    private void ContinueHuxinMirrorPlacement(long id)
    {
        if (_pendingDecision is not null) return;
        if (_resolutionStack.LastOrDefault() is not HuxinMirrorPlacementFrame frame || frame.Id != id ||
            frame.ActorSeat == frame.TargetSeat || frame.From != CardLocation.Hand(frame.ActorSeat) || frame.SequenceAfter <= frame.SequenceBefore ||
            !_cardMovements.Any(move => move.Sequence > frame.SequenceBefore && move.Sequence <= frame.SequenceAfter && move.CardId == frame.CardId &&
                move.CardKind == CardKind.HuxinMirror && move.From == frame.From && move.To == CardLocation.Equipment(frame.TargetSeat)))
            throw new InvalidOperationException("Huxin Mirror placement lost its frozen original hand payment.");
        if (TryBeginHpChangedProgramWindow(id, PostEventContinuation.HuxinMirrorPlacement) || TryBeginCardsMovedProgramWindow(id)) return;
        PopResolutionFrame(id, ResolutionFrameKind.HuxinMirrorPlacement);
    }

    private void ReturnHuxinMirrorPlacementMovement(CardsMovedTriggerWindowFrame window)
    {
        var id = window.ResumeHuxinMirrorPlacementFrameId!.Value;
        if (_resolutionStack.LastOrDefault() is not HuxinMirrorPlacementFrame parent || parent.Id != id ||
            window.Batch.ParentFrameId != id || window.Batch.AwaitingProgramFrameId is not null)
            throw new InvalidOperationException("Huxin Mirror placement lost its exact movement child parent.");
        ContinueHuxinMirrorPlacement(id);
    }
}
