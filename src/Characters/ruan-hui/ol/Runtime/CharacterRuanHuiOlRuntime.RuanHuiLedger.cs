#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanHuiOlRuntime
{
    internal bool HasRuanHuiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMingchaRevealDrawChoice) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmJingzhongNextActualPlay);

    internal static CardConversionSource RuanHuiSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_ruan_hui_ol_RuanHuiLedger.RuanHuiSource(f);
    internal ActualDiscardRecoveryPhaseKey? RuanHuiPlay => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p ? p : null;

    internal ActualDiscardRecoveryPhaseKey? JingzhongDuePhase(JingzhongLeaseEvent lease) => _coreProgramEventHistoryOperations.ProgramEventHistory<ActualDiscardRecoveryPhaseStartedEvent>().Select(e => e.Phase).FirstOrDefault(p => p.Token > lease.AfterPhaseToken && p.ActorSeat == lease.RecipientSeat && p.Kind == ActualDiscardRecoveryPhaseKind.Play);
    internal bool SameJingzhongLeaseOwner(JingzhongLeaseEvent a, JingzhongLeaseEvent b) => a.Source.OwnerSeat == b.Source.OwnerSeat && a.Source.SkillId == b.Source.SkillId && a.Source.SkillInstanceId == b.Source.SkillInstanceId && a.GameplayHash == b.GameplayHash;
    internal int JingzhongUsedCount(long lease)
    {
        var grant = _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Single(e => e.FrameId == lease);
        var phase = JingzhongDuePhase(grant)?.Token;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongClaimIssuedEvent>().Count(e => e.PhaseToken == phase && _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Any(l => l.FrameId == e.LeaseId && SameJingzhongLeaseOwner(l, grant)));
    }

    internal bool JingzhongClaimAlreadyIssued(JingzhongLeaseEvent lease, long action) => _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongClaimIssuedEvent>().Any(e => e.PhaseToken == JingzhongDuePhase(lease)?.Token && e.ActionId == action && _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Any(l => l.FrameId == e.LeaseId && SameJingzhongLeaseOwner(l, lease)));
    internal JingzhongLeaseEvent[] JingzhongPhaseLeases(long phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Where(l => JingzhongDuePhase(l)?.Token == phase && JingzhongUsedCount(l.FrameId) < 3 && _coreMatchParticipantsPlayersAccess._players[l.Source.OwnerSeat].IsAlive).ToArray();
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27080)]
    internal void ObserveRuanHuiUse(IGameEvent fact)
    {
        if (!HasRuanHuiCapability || RuanHuiPlay is not { } phase)
            return;
        if (fact is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use)
        {
            var action = use.Action;
            var costs = action?.PhysicalCards ?? (use.PhysicalCardIds ?? (use.CardId > 0 ? [use.CardId] : [])).Select(id => new CardActionCost(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
            RecordJingzhongUse(phase, action?.ActionId ?? -use.Id, use.Id, use.SourceSeat, false, costs);
            return;
        }

        if (fact is not CardActionAcceptedEvent { Action.Type: CardActionType.Response } accepted)
            return;
        var a = accepted.Action;
        if (a.ActorSeat != a.ProviderSeat || a.ResponderSeat != a.ActorSeat || a.RequesterSeat is not null || !(a.EffectiveKind == CardKind.Nullification || a.EffectiveKind == CardKind.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], a.EffectiveKind)))
            return;
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == a.ParentActionId) ?? (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { DelayedReturn: not null } n ? _coreCardUseLifecycleOperations.LifecycleCardUse(n.ParentFrameId) : null);
        RecordJingzhongUse(phase, a.ActionId, parent?.Id ?? 0, a.ActorSeat, true, a.PhysicalCards);
        // Physical Nullification commits its movement before accepting its response action.
        foreach (var batch in _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.Movements.Any(m => a.PhysicalCards.Any(c => c.CardId == m.CardId))).ToArray())
            CaptureRuanHuiMovementBatch(batch);
    }

    internal void RecordJingzhongUse(ActualDiscardRecoveryPhaseKey phase, long action, long use, int actor, bool response, IReadOnlyList<CardActionCost> costs)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongUseEntityEvent>().Any(e => e.ActionId == action))
            return;
        for (var i = 0; i < costs.Count; i++)
        {
            var c = costs[i];
            var paid = response ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == c.CardId && m.From == c.From && (m.Reason == CardMoveReasons.Respond || m.Reason == CardMoveReasons.Nullification) && m.TurnNumber == phase.TurnNumber) : null;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JingzhongUseEntityEvent(phase.Token, action, use, actor, response, i, costs.Count, c.CardId, c.CardKind, c.From, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid?.Sequence));
        }
    }

    internal int JingzhongFinalActor(JingzhongUseEntityEvent e)
    {
        var actor = e.OriginalActor;
        if (e.Response)
            return actor;
        foreach (var change in _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(c => c.CardUseFrameId == e.UseId))
        {
            if (change.PreviousActorSeat != actor)
                throw new InvalidOperationException("Jingzhong lost its original Use actor replacement chain.");
            actor = change.ActorSeat;
        }

        return actor;
    }

    internal bool JingzhongMaterialStillOwnsDiscard(JingzhongUseEntityEvent fact, CardMovementRecord last)
    {
        if (last.To != CardLocation.DiscardPile || _cardZonesOperations._cardZones.GetLocation(fact.CardId) != CardLocation.DiscardPile || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == fact.CardId).Sequence != last.Sequence || last.CardKind != fact.PhysicalKind || _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongUseEntityEvent>().Any(e => e.CardId == fact.CardId && e.ActionId != fact.ActionId && (e.CapturedAfterSequence > fact.CapturedAfterSequence || e.CapturedAfterSequence == fact.CapturedAfterSequence && e.ActionId > fact.ActionId)))
            return false;
        var paid = fact.PaidEntrySequence is { } sequence ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == sequence && m.CardId == fact.CardId) : _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == fact.CardId && m.Sequence > fact.CapturedAfterSequence && m.From == fact.OriginalFrom && (fact.Response ? m.Reason == CardMoveReasons.Respond || m.Reason == CardMoveReasons.Nullification : m.Reason == CardMoveReasons.Use || m.Reason == CardMoveReasons.EquipmentUse || m.Reason == CardMoveReasons.IronChainUse));
        if (paid is null || paid.Sequence > last.Sequence || paid.From != fact.OriginalFrom || paid.To.Zone is not (CardZoneKind.Processing or CardZoneKind.Equipment or CardZoneKind.Judgment))
            return false;
        var location = paid.To;
        foreach (var move in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.CardId == fact.CardId && m.Sequence > paid.Sequence && m.Sequence <= last.Sequence))
        {
            if (move.From != location)
                return false;
            if (move.Sequence == last.Sequence)
                return location.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment || location == CardLocation.Processing && (fact.Response ? move.Reason == CardMoveReasons.ResponseFinished || move.Reason == CardMoveReasons.NullificationFinished : move.Reason == CardMoveReasons.UseFinished || move.Reason == CardMoveReasons.IronChainFinished || JingzhongAbolishedEquipmentUse(fact, move));
            if (location == CardLocation.Processing && move.To.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && (move.Reason == CardMoveReasons.Use || move.Reason == CardMoveReasons.EquipmentEnter || move.Reason == CardMoveReasons.DelayedCardPlace) || move.From == move.To && location.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment)
            {
                location = move.To;
                continue;
            }

            return false;
        }

        return false;
    }

    internal bool JingzhongAbolishedEquipmentUse(JingzhongUseEntityEvent fact, CardMovementRecord move) => !fact.Response && move.Reason.Value == "area.abolished.equipment" && _coreCardUseLifecycleOperations.LifecycleCardUse(fact.UseId)is { Action: { Type: CardActionType.Use } action } use && action.ActionId == fact.ActionId && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind) && action.PhysicalCards.Any(c => c.CardId == fact.CardId && c.CardKind == fact.PhysicalKind && c.From == fact.OriginalFrom) && use.PhysicalCardIds?.Contains(fact.CardId) == true;
    internal void CaptureRuanHuiMovementBatch(CardMovementBatchContext batch)
    {
        if (!HasRuanHuiCapability)
            return;
        if (batch.DiscardRecoveryPhase is { Kind: ActualDiscardRecoveryPhaseKind.Discard } discard)
            foreach (var m in batch.Movements.Where(m => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)?.OwnerSeat == discard.ActorSeat && global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[discard.ActorSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId))) == false))
                if (!_coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongDiscardEvent>().Any(e => e.MovementSequence == m.Sequence))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JingzhongDiscardEvent(discard.Token, discard.ActorSeat, m.Sequence, m.CardId));
        if (batch.DiscardRecoveryPhase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase)
            return;
        foreach (var lease in JingzhongPhaseLeases(phase.Token))
            foreach (var group in _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongUseEntityEvent>().Where(e => e.PhaseToken == phase.Token && JingzhongFinalActor(e) == lease.RecipientSeat).GroupBy(e => e.ActionId).ToArray())
            {
                if (JingzhongClaimAlreadyIssued(lease, group.Key) || _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongSettledEntityEvent>().Any(e => e.LeaseId == lease.FrameId && e.ActionId == group.Key))
                    continue;
                var facts = group.ToArray();
                // A multi-material cleanup may span several atomic batches. Wait until its processing materials have settled.
                if (facts.Any(e => _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.Processing))
                    continue;
                var eligible = facts.Select(e => (Fact: e, Move: _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == e.CardId))).Where(x => x.Move is not null && JingzhongMaterialStillOwnsDiscard(x.Fact, x.Move)).ToArray();
                if (!eligible.Any(x => batch.Movements.Contains(x.Move!)))
                    continue;
                foreach (var x in eligible)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JingzhongSettledEntityEvent(lease.FrameId, phase.Token, group.Key, batch.Id, x.Fact.CardId, x.Move!.Sequence));
            }
    }

    internal JingzhongSettledEntityEvent[] RuanHuiClaimFacts(CardMovementBatchContext batch, ProgramTriggerCandidate c)
    {
        if (c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count)
            return[];
        var move = batch.Movements[c.OccurrenceIndex];
        var matching = _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongSettledEntityEvent>().Where(e => e.BatchId == batch.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Any(l => l.FrameId == e.LeaseId && l.Source.OwnerSeat == c.OwnerSeat && l.Source.SkillInstanceId == c.SkillInstanceId && l.GameplayHash == c.GameplayHash) && !JingzhongClaimAlreadyIssued(_coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Single(l => l.FrameId == e.LeaseId), e.ActionId)).OrderBy(e => e.LeaseId).ThenBy(e => e.ActionId).ThenBy(e => e.MovementSequence).ToArray();
        var anchor = matching.FirstOrDefault(e => e.CardId == move.CardId && e.MovementSequence == move.Sequence);
        return anchor is null ? [] : matching.Where(e => e.LeaseId == anchor.LeaseId && e.ActionId == anchor.ActionId).ToArray();
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectRuanHuiClaimCandidates(CardMovementBatchContext batch)
    {
        if (!HasRuanHuiCapability)
            return[];
        return _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongSettledEntityEvent>().Where(e => e.BatchId == batch.Id).GroupBy(e => (e.LeaseId, e.ActionId)).Select(g => g.First(e => batch.Movements.Any(m => m.CardId == e.CardId && m.Sequence == e.MovementSequence))).Select(e =>
        {
            var lease = _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Single(l => l.FrameId == e.LeaseId);
            var t = _contentRegistryOperations._contentRegistry.GetSkill(lease.Source.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ObtainJingzhongUsedEntities }]);
            var occurrence = batch.Movements.ToList().FindIndex(m => m.CardId == e.CardId);
            return new ProgramTriggerCandidate(lease.Source.OwnerSeat, lease.Source.SkillId, t.Id, lease.Source.SkillInstanceId, lease.GameplayHash, t.Priority, Math.Max(0, occurrence));
        }).Distinct().ToArray();
    }

    internal bool IsIssuedRuanHuiCandidate(ProgramTriggerCandidate c) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(c.SkillId)?.Program?.Triggers.Any(t => t.Id == c.BindingId && t.Effects is [{ Op: SkillProgramEffectOp.ObtainJingzhongUsedEntities }]) == true;
    internal bool CanRunIssuedRuanHuiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsIssuedRuanHuiCandidate(c) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || context.OwnerSeat != c.OwnerSeat || context.Window != SkillProgramTriggerWindow.DiscardPileReceived || context.MovementBatch is not { } b || context.ParentFrameId != b.Id || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == b.Id)is not { } w || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != c || !w.Batch.Movements.SequenceEqual(b.Movements))
            return false;
        return RuanHuiClaimFacts(b, c).Any(e => JingzhongUsedCount(e.LeaseId) < 3 && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == e.CardId).Sequence == e.MovementSequence);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3300)]
    internal void CollectWindow_CollectRuanHuiClaimCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectRuanHuiClaimCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2700)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureRuanHuiMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureRuanHuiMovementBatch(completed);
        return completed;
    }

    [ProgramIssuedTrigger(560, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger560(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedRuanHuiCandidate(candidate) ? CanRunIssuedRuanHuiCandidate(candidate, context) : null;
}
