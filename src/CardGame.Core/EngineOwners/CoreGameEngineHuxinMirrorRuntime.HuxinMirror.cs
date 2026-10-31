#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHuxinMirrorRuntime
{
    internal Card? ActiveHuxinMirror(int owner, string instance) => _characterHuCheErMouOperations.KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[owner]).FirstOrDefault(card => card.Kind == CardKind.HuxinMirror && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, HuxinMirrorContract.SkillId, instance, card));
    internal bool HuxinMirrorDamageQualifies(int owner, int amount, global::CardGame.Core.GameEngine.IDamageAttempt attack) => amount > 1 || amount > 0 && _coreMatchParticipantsPlayersAccess._players[owner].HasHpValue && PublicDamagePrediction.Calculate(_coreMatchParticipantsPlayersAccess._players[owner].Hp, _coreMatchParticipantsPlayersAccess._players[owner].Armor, amount, maxHp: _coreMatchParticipantsPlayersAccess._players[owner].MaxHp, bypassArmor: _coreNativeDamageArmorPoliciesOperations.BypassesNativeDamageArmor(attack), armorDamageMultiplier: _coreNativeDamageArmorPoliciesOperations.NativeDamageArmorMultiplier(attack)).IsLethal;
    internal bool HuxinMirrorIsEligible(int owner, string instance, ProgramSkillWindowContext window) => window is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0 } && window.TargetSeat == owner && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(_coreMatchParticipantsPlayersAccess._players[owner]) && ActiveHuxinMirror(owner, instance)is not null && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && attack.TargetSeat == owner && (attack.IsChainPropagation || !attack.IgnoresArmor) && !_coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(attack.ResolutionId, owner) && HuxinMirrorDamageQualifies(owner, window.Amount, attack);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62058, SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage)]
    internal bool CanOfferHuxinMirror(ProgramTriggerLegalityContext context) => context.SkillId == HuxinMirrorContract.SkillId && context.WindowContext is { } window && HuxinMirrorIsEligible(context.OwnerSeat, context.Candidate.SkillInstanceId, window);
    SkillProgramStepOutcome global::CardGame.Core.IHuxinMirrorHost.PayAndPrevent(ProgramSkillFrame frame) => PayHuxinMirrorAndPrevent(frame);
    internal SkillProgramStepOutcome PayHuxinMirrorAndPrevent(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.HuxinMirrorPayment is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || window.Prevented || window.TargetSeat != frame.OwnerSeat || window.Amount != context.Amount || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].Candidate) || !HuxinMirrorIsEligible(frame.OwnerSeat, frame.SkillInstanceId, context) || ActiveHuxinMirror(frame.OwnerSeat, frame.SkillInstanceId)is not { } card || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack)
            throw new InvalidOperationException("Huxin Mirror requires its exact unpaid armor and original damage candidate.");
        var receipt = new HuxinMirrorPaymentReceipt(frame.InstructionIndex, window.Id, attack.ResolutionId, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, card.Id, window.Amount, 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HuxinMirrorPayment = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        // This is the actual equipped entity put into discard, not a logical-discard instruction.
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Equipment(frame.OwnerSeat), CardLocation.DiscardPile, global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorDefinitions.HuxinMirrorPaymentReason, move =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var paid = receipt with
            {
                MovementSequence = move.Sequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { HuxinMirrorPayment = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuxinMirrorPaidEvent(frame.Id, paid));
        });
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidPaidHuxinMirror(ProgramSkillFrame frame)
    {
        if (frame.HuxinMirrorPayment is not { } paid || paid.InstructionIndex != frame.InstructionIndex || paid.Source != new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || paid.GameplayHash != frame.GameplayHash || frame.WindowContext?.ParentFrameId != paid.WindowFrameId || paid.MovementSequence <= 0 || !_coreProgramEventHistoryOperations.ProgramEventHistory<HuxinMirrorPaidEvent>().Any(e => e.ProgramFrameId == frame.Id && e.Receipt == paid) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == paid.MovementSequence && move.CardId == paid.CardId && move.CardKind == CardKind.HuxinMirror && move.From == CardLocation.Equipment(frame.OwnerSeat) && move.To == CardLocation.DiscardPile && move.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorDefinitions.HuxinMirrorPaymentReason))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is BeforeDamageProgramWindowFrame window && window.Id == paid.WindowFrameId && window.Prevented && window.Amount == paid.Amount && (window.ContinuationAttackResolutionId ?? window.ParentFrameId) == paid.AttackFrameId;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62058)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62058)]
    internal bool KeepPaidHuxinMirror(ProgramSkillFrame frame) => frame.HuxinMirrorPayment is not null && ValidPaidHuxinMirror(frame);
    [ProgramContinuation(ProgramContinuationKind.Resume, 62058)]
    internal bool ResumePaidHuxinMirror(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuxinMirrorPayment: not null } frame || frame.Id != id || frame.PendingMovementContinuation is null)
            return false;
        if (!ValidPaidHuxinMirror(frame))
            throw new InvalidOperationException("Huxin Mirror lost its actual paid damage receipt.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62058)]
    internal bool ReturnPaidHuxinMirrorMovement(ProgramSkillFrame frame)
    {
        if (frame.HuxinMirrorPayment is null)
            return false;
        if (frame.PendingMovementContinuation is null || !ValidPaidHuxinMirror(frame))
            throw new InvalidOperationException("Huxin Mirror lost its original movement child return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62058)]
    internal bool IsPaidHuxinMirrorMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage && frame.HuxinMirrorPayment is not null && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidPaidHuxinMirror(frame);

    internal IEnumerable<LegalAction> HuxinMirrorPlacementActions(CharacterState actor, IReadOnlyList<Card> cards) => cards.Where(card => card.Kind == CardKind.HuxinMirror && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(actor.Seat)).SelectMany(card => _coreMatchParticipantsPlayersAccess._players.Where(target => target.Seat != actor.Seat && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(actor.Seat, target.Seat, card, CardLocation.Hand(actor.Seat))).Select(target => new LegalAction(LegalActionKind.Equip, card.Id, target.Seat, $"将【护心镜】置于{(target.Name)}的装备区")));
    internal bool TryPlaceHandHuxinMirror(CharacterState actor, LegalAction action)
    {
        if (action.Kind != LegalActionKind.Equip || action.TargetSeat is not { } target || action.CardId is not { } id || _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind != CardKind.HuxinMirror)
            return false;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != actor.Seat || target == actor.Seat || !_coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(actor.Seat, target, card, CardLocation.Hand(actor.Seat)))
            throw new InvalidOperationException("Huxin Mirror placement requires its original hand entity and live other equipment slot.");
        var frame = new HuxinMirrorPlacementFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, actor.Seat, target, id, CardLocation.Hand(actor.Seat), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        var replaced = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).FirstOrDefault(c => _coreFieldEquipmentOperations.FieldEquipmentSlot(c) == EquipmentSlot.Armor);
        if (_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Count(c => _coreFieldEquipmentOperations.FieldEquipmentSlot(c) == EquipmentSlot.Armor) < _coreMatchParticipantsPlayersAccess._players[target].EquipmentSlotCapacity(EquipmentSlot.Armor))
            replaced = null;
        if (replaced is not null)
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(target), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        _corePhysicalMovementFacadesOperations.MoveCard(card, frame.From, CardLocation.Equipment(target), CardMoveReasons.EquipmentEnter);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, target, EquipmentSlot.Armor, id, card.Kind, replaced?.Id));
        ContinueHuxinMirrorPlacement(frame.Id);
        return true;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 20746, NativeFrameAutomaticPending.Ignore, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(HuxinMirrorPlacementFrame), 20746)]
    internal void ContinueHuxinMirrorPlacement(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not HuxinMirrorPlacementFrame frame || frame.Id != id || frame.ActorSeat == frame.TargetSeat || frame.From != CardLocation.Hand(frame.ActorSeat) || frame.SequenceAfter <= frame.SequenceBefore || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence > frame.SequenceBefore && move.Sequence <= frame.SequenceAfter && move.CardId == frame.CardId && move.CardKind == CardKind.HuxinMirror && move.From == frame.From && move.To == CardLocation.Equipment(frame.TargetSeat)))
            throw new InvalidOperationException("Huxin Mirror placement lost its frozen original hand payment.");
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.HuxinMirrorPlacement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        _coreRuntimeOperations.PopResolutionFrame(id, ResolutionFrameKind.HuxinMirrorPlacement);
    }

    internal void ReturnHuxinMirrorPlacementMovement(CardsMovedTriggerWindowFrame window)
    {
        var id = window.ResumeHuxinMirrorPlacementFrameId!.Value;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not HuxinMirrorPlacementFrame parent || parent.Id != id || window.Batch.ParentFrameId != id || window.Batch.AwaitingProgramFrameId is not null)
            throw new InvalidOperationException("Huxin Mirror placement lost its exact movement child parent.");
        ContinueHuxinMirrorPlacement(id);
    }
}
