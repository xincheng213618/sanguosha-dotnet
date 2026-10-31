#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoOlRuntime
{
    internal void ObserveZhugeGuoMovement(CardMovementRecord movement)
    {
        if (!_characterZhugeGuoOlHasIssuedZhugeGuoStateAccess._hasIssuedZhugeGuoState || movement.From is not { Zone: CardZoneKind.Hand, OwnerSeat: { } owner } || movement.From == movement.To || _coreMatchParticipantsPlayersAccess._players[owner].QirangEntityLeases is not { Count: > 0 } leases)
            return;
        foreach (var l in leases.Where(l => l.CardId == movement.CardId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QirangEntityLeaseEndedEvent(owner, l.CardId, l.GainSequence, "left-owner-hand"));
        var remaining = leases.Where(l => l.CardId != movement.CardId).ToArray();
        _coreMatchParticipantsPlayersAccess._players[owner].QirangEntityLeases = remaining.Length == 0 ? null : remaining;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 100)]
    internal void ObserveZhugeGuoTurnAndDeath(IGameEvent payload)
    {
        if (!_characterZhugeGuoOlHasIssuedZhugeGuoStateAccess._hasIssuedZhugeGuoState)
            return;
        if (payload is TurnStartedEvent turn)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[turn.ActorSeat];
            var eligible = (owner.YuhuaDelayDebts ?? []).Where(d => d.CreatedActualTurn < turn.TurnNumber).ToArray();
            if (eligible.Length == 0 && owner.YuhuaActiveQuota is null)
                return;
            var count = Math.Min(5, 1 + eligible.Length);
            var remaining = (owner.YuhuaDelayDebts ?? []).Except(eligible).ToArray();
            owner.YuhuaDelayDebts = remaining.Length == 0 ? null : remaining;
            owner.YuhuaActiveQuota = new(owner.Seat, turn.TurnNumber, count);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuhuaActualTurnQuotaStartedEvent(owner.Seat, turn.TurnNumber, count));
        }
        else if (payload is PlayerDiedEvent died)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[died.VictimSeat];
            foreach (var l in owner.QirangEntityLeases ?? [])
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QirangEntityLeaseEndedEvent(owner.Seat, l.CardId, l.GainSequence, "owner-died"));
            owner.QirangEntityLeases = null;
            owner.YuhuaDelayDebts = null;
            owner.YuhuaActiveQuota = null;
        }
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 40)]
    internal CardSnapshot[] GetZhugeGuoPrivatelyViewedCards(int viewer) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ZhugeGuo.Stage: ZhugeGuoStage.YuhuaInspect, TopReorder: { } top } f && f.OwnerSeat == viewer && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: var seat } && seat == viewer ? top.ViewedCardIds.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(c => c.Id == id))).ToArray() : [];
    internal bool ZhugeGuoFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueZhugeGuo(root) || root.ZhugeGuo is not { Stage: ZhugeGuoStage.GainChildren } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.GainedCardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == QirangGainReason);
    }

    internal ProgramSkillFrame? ZhugeGuoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterZhugeGuoOlOperations.ZhugeGuoFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 110)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 220)]
    internal bool HasZhugeGuoDamageObserver(long id) => ZhugeGuoObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 310)]
    internal bool HasZhugeGuoBeforeDamageObserver(long id) => ZhugeGuoObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 200)]
    internal bool TryAdvanceZhugeGuoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhugeGuoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    [ProgramPipelineHook(ProgramPipelinePhase.ObserveCommittedMovement, 200)]
    internal void ObserveMovementBatchZhugeGuo(CardMovementBatchBuilder batch, CardMovementRecord movement) => ObserveZhugeGuoMovement(movement);
}
