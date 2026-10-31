#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHeMouMobileRuntime
{
    internal MouQiaobianRuntime MouQiaobian => _mouQiaobianOperations.Get();

    int global::CardGame.Core.IMouZhangHeFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IMouZhangHeFacts.TurnOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    bool global::CardGame.Core.IMouZhangHeFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    IReadOnlyList<int> global::CardGame.Core.IMouZhangHeFacts.Seats => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(p => p.Seat).ToArray());

    MouZhangHePlayer global::CardGame.Core.IMouZhangHeFacts.Player(int s) => new(s, _coreMatchParticipantsPlayersAccess._players[s].IsAlive, _coreMatchParticipantsPlayersAccess._players[s].IsHuman, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count, _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[s]).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[s]).Count, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[s]).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[s]).Count);
    SkillProgram? global::CardGame.Core.IMouZhangHeFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouZhangHeFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<MouZhangHeMovement> global::CardGame.Core.IMouZhangHeFacts.Movements => Array.AsReadOnly(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Select(m => new MouZhangHeMovement(m.Sequence, m.CardId, m.From, m.To, m.Reason.Value)).ToArray());

    CardLocation global::CardGame.Core.IMouZhangHeFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    bool global::CardGame.Core.IMouZhangHeFacts.ExactStart(ProgramSkillFrame f, SkillProgramEffectOp op) => f.InstructionIndex == 1 && f.ActivationId == f.TriggerId && f.SkillId == MouQiaobianRuntime.Qiaobian && ((global::CardGame.Core.IMouZhangHeFacts)this).Program(f.SkillId)?.GameplayHash == f.GameplayHash && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == op && ((global::CardGame.Core.IMouZhangHeFacts)this).NativeParent(f);
    bool global::CardGame.Core.IMouZhangHeFacts.NativeParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } x || x.OwnerSeat != f.OwnerSeat || x.SourceSeat != f.OwnerSeat || x.TargetSeat != f.OwnerSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1].Id != x.ParentFrameId)
            return false;
        var parent = _resolutionStackOperations._resolutionStack[n - 1];
        if (parent is ProgramLifecycleTriggerWindowFrame w)
            return w.Window == x.Window && w.OwnerSeat == f.OwnerSeat && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && (x.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && w.Continuation == ProgramLifecycleContinuation.NormalTurnStart || x.Window == SkillProgramTriggerWindow.ActualPhaseSkipped && w.Continuation == ProgramLifecycleContinuation.CompleteActualSkippedPhase && w.MouZhangHeSkip is { } s && s.Entitlement.Source.OwnerSeat == f.OwnerSeat && s.Entitlement.Source.SkillInstanceId == f.SkillInstanceId && s.Entitlement.GameplayHash == f.GameplayHash && s.NativeReturn.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber);
        return parent is TurnEndingBoundaryFrame t && x.Window == SkillProgramTriggerWindow.TurnEnding && t.OwnerSeat == f.OwnerSeat && t.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && t.ItemIndex >= 0 && t.ItemIndex < t.Items.Count && t.Items[t.ItemIndex].Candidate is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }

    IReadOnlyList<MouZhangHeCard> global::CardGame.Core.IMouZhangHeFacts.Discardable(int owner) => Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, owner)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, new(z, owner), OwnedCardMoveIntent.Discard)).Select(c => new MouZhangHeCard(c.Id, new(z, owner)))).ToArray());
    IReadOnlyList<MouZhangHeCard> global::CardGame.Core.IMouZhangHeFacts.FieldCards => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => new[] { CardZoneKind.Equipment, CardZoneKind.Judgment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, p.Seat)).Select(c => new MouZhangHeCard(c.Id, new(z, p.Seat))))).ToArray());

    bool global::CardGame.Core.IMouZhangHeFacts.CanMoveField(int id, int target)
    {
        var from = ((global::CardGame.Core.IMouZhangHeFacts)this).Location(id);
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        return from.OwnerSeat is { } source && source != target && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && from.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && _coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(card, from.Zone, target) && (from.Zone != CardZoneKind.Equipment || _coreMatchParticipantsPlayersAccess._players[target].EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot) > 0) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, card, from, OwnedCardMoveIntent.Transfer);
    }

    IReadOnlyList<PromptChoice> global::CardGame.Core.IMouZhangHeFacts.HandChoices(ProgramSkillFrame f, int target) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand], OwnedCardMoveIntent.Obtain, destination: SkillProgramCardDestination.OwnerHand, destinationSeat: f.OwnerSeat);
    int global::CardGame.Core.IMouZhangHeFacts.ResolveHandChoice(PromptChoice c, int subject)
    {
        if (c.Cards is [var id])
            return id;
        if (c.Parameters.GetValueOrDefault("card-owner-seat") != subject.ToString(CultureInfo.InvariantCulture) || c.Parameters.GetValueOrDefault("source-zone") != "Hand" || !int.TryParse(c.Parameters.GetValueOrDefault("slot-index"), out var slot) || slot < 0 || slot >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[subject]).Count)
            throw new InvalidOperationException("Qiaobian's opaque Hand slot is stale.");
        return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[subject])[slot].Id;
    }

    int global::CardGame.Core.IMouZhangHeFacts.KeepValue(int s, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[s]);
    bool global::CardGame.Core.IMouZhangHeFacts.Friendly(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false)) > 0;
    long global::CardGame.Core.IMouZhangHeFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    ProgramPhaseSchedule? global::CardGame.Core.IMouZhangHeFacts.Schedule => _programPhaseScheduleOperations._programPhaseSchedule;

    bool global::CardGame.Core.IMouZhangHeFacts.FirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => MouQiaobianFirstNativeChild(f, child);
    void global::CardGame.Core.IMouZhangHeNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouZhangHeNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouZhangHeNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouZhangHeNative.Discard(ProgramSkillFrame f, MouZhangHeCard card) => _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId), card.From, new(MouQiaobianRuntime.CostReason), m => MouZhangHeNative_Seal(f, m), f.Id);
    void global::CardGame.Core.IMouZhangHeNative.Obtain(ProgramSkillFrame f, int target, int id) => _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(target), CardLocation.Hand(f.OwnerSeat), new(MouQiaobianRuntime.ObtainReason), m => MouZhangHeNative_Seal(f, m), disclosure: CardMovementDisclosure.FaceDown);
    void global::CardGame.Core.IMouZhangHeNative.MoveField(ProgramSkillFrame f, int id, int target)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        if (f.MouZhangHe?.From != from || !_characterZhangHeMouMobileOperations.CreateMouZhangHeFacts().CanMoveField(id, target))
            throw new InvalidOperationException("Qiaobian's corresponding-zone move is no longer native legal.");
        var destination = _coreProgramCardPaymentsOperations.ProgramCorrespondingZoneLocation(card, from.Zone, target);
        if (destination.Zone == CardZoneKind.Equipment)
        {
            var slot = _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;
            var occupants = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Where(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == slot).ToArray();
            if (occupants.Length >= _coreMatchParticipantsPlayersAccess._players[target].EquipmentSlotCapacity(slot))
                _corePhysicalMovementFacadesOperations.MoveCard(occupants[0], CardLocation.Equipment(target), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace, m => MouZhangHeNative_Seal(f, m), disclosure: CardMovementDisclosure.FaceUp);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, from, destination, new(MouQiaobianRuntime.FieldReason), m => MouZhangHeNative_Seal(f, m), disclosure: CardMovementDisclosure.FaceUp);
    }

    void global::CardGame.Core.IMouZhangHeNative.ApplySkips(ProgramSkillFrame f, IReadOnlyList<SkillProgramTurnPhase> phases)
    {
        if (!MouQiaobian.Valid(f) || f.MouZhangHe is not { Operation: SkillProgramEffectOp.PrepareMouZhangHeQiaobian, Accepted: true } r || r.PaidCount != phases.Count || !phases.SequenceEqual(r.Phases.Take(r.PaidCount)) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangHeCostPaidEvent>().Count(x => x.FrameId == f.Id) != r.PaidCount)
            throw new InvalidOperationException("Only Qiaobian's actually paid distinct-area costs may issue phase skips.");
        foreach (var phase in phases)
            _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= phase switch
            {
                SkillProgramTurnPhase.Judgment => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipJudgmentPhase,
                SkillProgramTurnPhase.Draw => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase,
                SkillProgramTurnPhase.Play => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase,
                SkillProgramTurnPhase.Discard => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDiscardPhase,
                _ => throw new InvalidOperationException("Qiaobian cannot skip Preparation or Ending.")};
    }

    void global::CardGame.Core.IMouZhangHeNative.OpenSkipWindow(MouZhangHeSkipEntitlement q, ProgramActualPhaseSkipContext c)
    {
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || c.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || c.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || q.ActualTurn != c.ActualTurn || q.Source.OwnerSeat != c.OwnerSeat || q.Phase != c.Phase || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangHeSkipIssuedEvent>().Any(x => x.Entitlement == q))
            throw new InvalidOperationException("Qiaobian can suspend only its real clean skipped-phase continuation.");
        var candidate = new ProgramTriggerCandidate(c.OwnerSeat, q.Source.SkillId, "actual-skipped-phase", q.Source.SkillInstanceId!, q.GameplayHash, 0);
        var window = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, c.OwnerSeat, SkillProgramTriggerWindow.ActualPhaseSkipped, Array.AsReadOnly(new[] { candidate }), ProgramLifecycleContinuation.CompleteActualSkippedPhase, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]))
        {
            MouZhangHeSkip = new(q, c)
        };
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangHeSkipObservedEvent(window.Id, q, c));
        _coreRuntimeOperations.AdvanceRuntimeFrame(window.Id);
    }

    void global::CardGame.Core.IMouZhangHeNative.ResumeSkip(ProgramLifecycleTriggerWindowFrame w)
    {
        var c = w.MouZhangHeSkip!.NativeReturn;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || w.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || c.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("Qiaobian's skipped-phase reward lost its native return cursor.");
        if (c.Phase == SkillProgramTurnPhase.Discard)
            _coreActualDiscardPhaseOperations.CompleteDiscardPhaseAfterProgramWindow(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]);
        else
            _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], (global::CardGame.Core.GameEngine.DelayedTurnEffects)c.DelayedEffects, c.AfterNormalDrawProgramsCompleted, c.DrawPhaseEndedProgramsCompleted, c.ActualDrawCompletionCompleted);
    }

    void global::CardGame.Core.IMouZhangHeNative.AdvanceLifecycleCursor(ProgramLifecycleTriggerWindowFrame w) => _coreProgramLifecycleOperations.AdvanceProgramLifecycleCursor(w);
    bool global::CardGame.Core.IMouZhangHeNative.BeginExtraPlay(ProgramSkillFrame f) => BeginMouQiaobianExtraPlay(f);
    void global::CardGame.Core.IMouZhangHeNative.ResumeExtraPlay(ProgramPhaseSchedule s, bool cancel) => ReturnMouQiaobianExtraPlay(s, cancel);
    bool global::CardGame.Core.IMouZhangHeNative.AdvanceObserverChildren() => _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal void MouZhangHeNative_Seal(ProgramSkillFrame f, CardMovementRecord m)
    {
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
        if (parent?.MouZhangHe is not { } r || !MouQiaobian.Valid(parent) || m.Sequence <= r.Before || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            throw new InvalidOperationException("Qiaobian's native movement lost its owning ancestor and exact payment interval.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent = parent with { MouZhangHe = r = r with { After = m.Sequence, PaidCount = r.PaidCount + (r.Stage == MouZhangHeStage.CostChildren && m.CardId == r.CardId && m.From == r.From && m.Reason.Value == MouQiaobianRuntime.CostReason ? 1 : 0) } });
        if (r.Stage == MouZhangHeStage.CostChildren && m.CardId == r.CardId && m.From == r.From && m.Reason.Value == MouQiaobianRuntime.CostReason)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangHeCostPaidEvent(f.Id, m.CardId, m.From, m.Sequence));
        if (r.Stage == MouZhangHeStage.ObtainChildren && m.CardId == r.CardId && m.From == r.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouQiaobianRuntime.ObtainReason)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangHeObtainedEvent(f.Id, f.OwnerSeat, m.Sequence));
        if (r.Stage == MouZhangHeStage.FieldChildren && m.CardId == r.CardId && m.From == r.From && m.To.OwnerSeat == r.TargetSeat && m.Reason.Value == MouQiaobianRuntime.FieldReason)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangHeFieldMovedEvent(f.Id, m.CardId, m.From.OwnerSeat!.Value, r.TargetSeat, m.Sequence));
    }

    internal bool BeginMouQiaobianExtraPlay(ProgramSkillFrame f)
    {
        if (!MouQiaobian.Valid(f) || f.MouZhangHe is not { Stage: MouZhangHeStage.ExtraPlay } r || f.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || _resolutionStackOperations._resolutionStack.Count != 2 || _resolutionStackOperations._resolutionStack[^1] != f || _resolutionStackOperations._resolutionStack[^2] is not TurnEndingBoundaryFrame parent || parent.Id != r.WindowId || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return false;
        r = r with
        {
            ExtraReturn = new(parent, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase, _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase(), _programPhaseScheduleOperations._programPhaseSchedule)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangHe = r });
        _coreRuntimeOperations.PopResolutionFrame(f.Id, ResolutionFrameKind.ProgramSkill);
        _coreRuntimeOperations.PopResolutionFrame(parent.Id, ResolutionFrameKind.TurnEndingBoundary);
        _programPhaseScheduleOperations._programPhaseSchedule = new(f, null, TurnPhase.Play, SkillProgramPhaseContinuation.BeforeNormalPreparation);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPhaseScheduledEvent(f.Id, f.SkillId, f.TriggerId!, f.OwnerSeat, TurnPhase.Play, true));
        _coreActualTurnsOperations.EnterPlayPhase(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
        return true;
    }

    internal void ReturnMouQiaobianExtraPlay(ProgramPhaseSchedule s, bool cancel)
    {
        if (s.Frame.MouZhangHe is not { Stage: MouZhangHeStage.ExtraPlay, ExtraReturn: { } r } q || s != _programPhaseScheduleOperations._programPhaseSchedule || s.Phase != TurnPhase.Play || s.ParentFrame is not null || q.WindowId != r.Parent.Id || q.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || s.Frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || r.Parent.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || r.Parent.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || !cancel && (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null) || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangHeEndingConsumedEvent>().Any(x => x.FrameId == s.Frame.Id && x.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber))
            throw new InvalidOperationException("Qiaobian's automatic extra Play lost its frozen actual Ending parent.");
        _programPhaseScheduleOperations._programPhaseSchedule = r.SuspendedSchedule;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPhaseScheduledEvent(s.Frame.Id, s.Frame.SkillId, s.Frame.TriggerId!, s.Frame.OwnerSeat, TurnPhase.Play, false));
        if (cancel)
        {
            _coreProgramLifecycleOperations.CompleteDetachedProgramBinding(s.Frame, false);
            return;
        }

        _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = r.CoarsePhase;
        if (r.ActualPhase is { } phase)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualDiscardRecoveryPhaseRestoredEvent(s.Frame.Id, phase));
        _coreRuntimeOperations.PushRuntimeFrame(r.Parent);
        _coreRuntimeOperations.PushRuntimeFrame(s.Frame with { MouZhangHe = q with { Stage = MouZhangHeStage.Complete, ExtraReturn = null } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(s.Frame.Id);
    }
}
