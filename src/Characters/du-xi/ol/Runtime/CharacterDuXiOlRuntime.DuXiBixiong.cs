#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuXiOlRuntime
{
    internal static int DuXiSuitBit(Suit suit) => global::CardGame.Core.RuntimeSkills.RuntimeRules_du_xi_ol_DuXiBixiong.DuXiSuitBit(suit);
    internal IEnumerable<CardMovementRecord> DuXiBixiongMovements(CardMovementBatchContext batch, int owner)
    {
        if (batch.DiscardRecoveryPhase is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase || phase.ActorSeat != owner || phase.ActualTurnOwnerSeat != owner || phase.TurnNumber != batch.TurnNumber || batch.OriginOwnerSeat is { } producer && producer != owner)
            return[];
        return batch.Movements.Where(m => m.From == CardLocation.Hand(owner) && m.To == CardLocation.DiscardPile && global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime.IsDiscardMovementReason(m.Reason));
    }

    internal void CaptureDuXiBixiongBatch(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordBixiongActualDiscard) || batch.DiscardRecoveryPhase is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase)
            return;
        var owner = phase.ActorSeat;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            return;
        var moves = DuXiBixiongMovements(batch, owner).ToArray();
        if (moves.Length == 0)
            return;
        foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[owner], SkillProgramTriggerWindow.CardsMoved).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.Any(e => e.Op == SkillProgramEffectOp.RecordBixiongActualDiscard)))
        {
            var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, owner, candidate.SkillInstanceId);
            foreach (var move in moves)
            {
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<DuXiBixiongSuitRecordedEvent>().Any(e => e.Source == source && e.MovementSequence == move.Sequence))
                    continue;
                var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], _coreAdvancedLifecycleOperations.GetAdvancedCard(move.CardId));
                if (DuXiSuitBit(suit) == 0)
                    continue;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuXiBixiongSuitRecordedEvent(batch.Id, source, owner, batch.TurnNumber, phase.Token, move.CardId, move.Sequence, suit));
            }
        }
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26300)]
    internal void ExpireDuXiBixiong(IGameEvent payload)
    {
        if (payload is TurnStartedEvent turn && _coreProgramEventHistoryOperations.ProgramEventHistory<DuXiBixiongSuitRecordedEvent>().Any(e => e.OwnerSeat == turn.ActorSeat))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuXiBixiongExpiredEvent(turn.ActorSeat, turn.TurnNumber));
    }

    internal IReadOnlyDictionary<int, int> DuXiSuitProjection()
    {
        var count = _coreCommandJournalStorageEventsPendingEventsAccess._events.Count + _coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents.Count;
        var last = _coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents.LastOrDefault() ?? _coreCommandJournalStorageEventsPendingEventsAccess._events.LastOrDefault()?.Payload;
        if (_characterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionEventCount == count && ReferenceEquals(_characterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionLastFact, last))
            return _characterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjection;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var expired = facts.OfType<DuXiBixiongExpiredEvent>().GroupBy(e => e.OwnerSeat).ToDictionary(g => g.Key, g => g.Max(e => e.ActualTurnNumber));
        var masks = new Dictionary<int, int>();
        foreach (var fact in facts.OfType<DuXiBixiongSuitRecordedEvent>())
            if (fact.ActualTurnNumber >= expired.GetValueOrDefault(fact.OwnerSeat))
                masks[fact.OwnerSeat] = masks.GetValueOrDefault(fact.OwnerSeat) | DuXiSuitBit(fact.Suit);
        _characterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjection = new System.Collections.ObjectModel.ReadOnlyDictionary<int, int>(masks);
        _characterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionEventCount = count;
        _characterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionLastFact = last;
        return _characterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjection;
    }

    internal bool HasAnyDuXiSuitProhibition() => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordBixiongActualDiscard) && DuXiSuitProjection().Count != 0;
    internal bool IsDuXiSuitTargetProhibited(int sourceSeat, int targetSeat, Suit effectiveSuit) => sourceSeat != targetSeat && DuXiSuitBit(effectiveSuit) != 0 && HasAnyDuXiSuitProhibition() && (DuXiSuitProjection().GetValueOrDefault(targetSeat) & DuXiSuitBit(effectiveSuit)) != 0;
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2500)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureDuXiBixiongBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureDuXiBixiongBatch(completed);
        return completed;
    }
}
