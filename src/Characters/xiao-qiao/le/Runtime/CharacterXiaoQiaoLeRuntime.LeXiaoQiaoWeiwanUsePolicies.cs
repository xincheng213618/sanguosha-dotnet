#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    internal static bool WeiwanDeclarationIsTrueUse(CardDeclarationParentReturn parent) => global::CardGame.Core.RuntimeSkills.RuntimeRules_xiao_qiao_le_LeXiaoQiaoWeiwan_UsePolicies.WeiwanDeclarationIsTrueUse(parent);
    internal bool ValidWeiwanLease(WeiwanTurnLease lease)
    {
        if (lease.Source.SkillId != "le:weiwan" || lease.Source.BindingId != "discard-qin-obtain-suits" || lease.Source.OwnerSeat != lease.ActorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(lease.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(lease.TargetSeat) || lease.ActorSeat == lease.TargetSeat || lease.ActualCount is not (2 or 3) || lease.ActualTurn < 1 || _contentRegistryOperations._contentRegistry.GetSkill(lease.Source.SkillId).Program?.GameplayHash != lease.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanTurnLeaseIssuedEvent>().Count(e => e.Lease.FrameId == lease.FrameId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanTurnLeaseIssuedEvent>().Single(e => e.Lease.FrameId == lease.FrameId).Lease != lease || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanStartedEvent>().Where(e => e.FrameId == lease.FrameId).ToArray()is not [var start] || start.Source != lease.Source || start.GameplayHash != lease.GameplayHash || start.Source.OwnerSeat != lease.ActorSeat || start.TargetSeat != lease.TargetSeat || start.ActualTurn != lease.ActualTurn || start.TurnOwner != lease.TurnOwner || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanCostPaidEvent>().Where(e => e.FrameId == lease.FrameId).ToArray()is not [var cost] || cost.CardId != start.CardId || cost.From != start.CostFrom || cost.MovementSequence <= start.SequenceBefore || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == cost.MovementSequence && m.CardId == cost.CardId && m.CardKind == start.PrintedKind && m.From == cost.From && m.To == cost.To && m.Reason.Value == WeiwanCostReason) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanObtainPaidEvent>().Where(e => e.FrameId == lease.FrameId).ToArray()is not [var paid] || paid.ActualCount != lease.ActualCount || paid.After != lease.SequenceAfter || paid.Before < cost.MovementSequence)
            return false;
        var originals = _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanEntityObtainedEvent>().Where(e => e.FrameId == lease.FrameId).ToArray();
        return originals.Length == paid.PrimaryCount && originals.Count(e => e.To == CardLocation.Hand(lease.ActorSeat)) == lease.ActualCount && originals.Select(e => e.CardId).Distinct().Count() == originals.Length && originals.All(e => e.BatchId == paid.BatchId && e.From.OwnerSeat == lease.TargetSeat && e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && e.MovementSequence > paid.Before && e.MovementSequence <= paid.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == e.From && m.To == e.To && m.Reason.Value == WeiwanObtainReason) == 1);
    }

    internal bool CurrentWeiwanLease(WeiwanTurnLease lease)
    {
        if (lease.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || lease.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !ValidWeiwanLease(lease))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var issued = Array.FindIndex(history, e => e is WeiwanTurnLeaseIssuedEvent fact && fact.Lease == lease);
        // A real end-owned extra Play may issue after TurnEnded. Only a later
        // boundary expires its new ticket; an already expired old one stays dead.
        return issued >= 0 && !history.Skip(issued + 1).Any(e => e is TurnEndedEvent or TurnStartedEvent);
    }

    internal WeiwanUseSnapshot CaptureWeiwanUseSnapshot(int actor) => new(actor, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
    {
        Leases = _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanTurnLeaseIssuedEvent>().Select(e => e.Lease).Where(lease => lease.ActorSeat == actor && CurrentWeiwanLease(lease)).OrderBy(lease => lease.FrameId).ToArray()
    };
    internal IReadOnlyList<WeiwanTurnLease> WeiwanPreUseLeases(int actor)
    {
        if (!HasLeXiaoQiaoWeiwanCapability)
            return[];
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration && declaration.Return.ActorSeat == actor && WeiwanDeclarationIsTrueUse(declaration.Return))
            return declaration.Payment.Weiwan is { } q && ValidWeiwanDeclaration(declaration.Payment, q) ? q.Leases : [];
        var current = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (current is ResponseWindowFrame response)
            current = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == response.ParentFrameId);
        if (current?.AcceptedDeclarationPayment is { Claimed: false } paid && paid.ActorSeat == actor && _cardZonesOperations._cardZones.GetLocation(paid.Cost.CardId) == CardLocation.Processing)
            return paid.Weiwan is { } q && ValidWeiwanDeclaration(paid, q) ? q.Leases : [];
        return CaptureWeiwanUseSnapshot(actor).Leases;
    }

    internal IReadOnlyList<WeiwanTurnLease> WeiwanQueryLeases(int actor, long? useId) => useId is { } id ? ValidIssuedWeiwanUse(id, actor)is { } r ? r.Snapshot.Leases : [] : WeiwanPreUseLeases(actor);
    internal bool IsWeiwanTargetForbidden(int actor, int target, CardKind kind, long? useId = null) => HasLeXiaoQiaoWeiwanCapability && WeiwanQueryLeases(actor, useId).Any(lease => lease.TargetSeat == target && lease.ActualCount == 3);
    internal bool HasWeiwanUseDistance(CharacterState actor, CharacterState target, CardKind kind, long? useId = null)
    {
        if (!HasLeXiaoQiaoWeiwanCapability)
            return false;
        var leases = WeiwanQueryLeases(actor.Seat, useId);
        return !leases.Any(lease => lease.TargetSeat == target.Seat && lease.ActualCount == 3) && leases.Any(lease => lease.TargetSeat == target.Seat && lease.ActualCount == 2);
    }

    internal bool HasWeiwanQuota(CharacterState actor, CharacterState target) => HasWeiwanUseDistance(actor, target, CardKind.Slash);
    internal DeclaredCardPayment CaptureWeiwanDeclarationPayment(DeclaredCardPayment paid, CardDeclarationParentReturn parent)
    {
        if (!HasLeXiaoQiaoWeiwanCapability || !WeiwanDeclarationIsTrueUse(parent) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(parent.ActorSeat) || paid.ActorSeat != parent.ActorSeat || paid.Claimed || paid.Weiwan is not null || _cardZonesOperations._cardZones.GetLocation(paid.Cost.CardId) != paid.Cost.From)
            return paid;
        var q = CaptureWeiwanUseSnapshot(parent.ActorSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanDeclarationCapturedEvent(paid.DeclarationId, paid.OwnerFrameId, parent.ActorSeat, paid.ProviderSeat, paid.Cost.CardId, paid.Cost.From, paid.DeclaredKind, q.ActualTurn, q.TurnOwner, q.CaptureSequence, q.Leases.Count));
        for (var i = 0; i < q.Leases.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanDeclarationLeaseCapturedEvent(paid.DeclarationId, i, q.Leases[i].FrameId));
        return paid with
        {
            Weiwan = q
        };
    }

    internal bool ValidWeiwanSnapshot(WeiwanUseSnapshot q) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(q.ActorSeat) && q.CaptureSequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && q.Leases is System.Collections.IList { IsReadOnly: true } && q.Leases.Select(l => l.FrameId).Distinct().Count() == q.Leases.Count && q.Leases.All(l => l.ActorSeat == q.ActorSeat && l.ActualTurn == q.ActualTurn && l.TurnOwner == q.TurnOwner && l.SequenceAfter <= q.CaptureSequence && ValidWeiwanLease(l));
    internal bool ValidWeiwanDeclaration(DeclaredCardPayment paid, WeiwanUseSnapshot q) => paid.Weiwan == q && paid.ActorSeat == q.ActorSeat && ValidWeiwanSnapshot(q) && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.OwnerFrameId)is { } owner && (owner.AcceptedDeclarationPayment == paid || owner is CardDeclarationFrame declaration && declaration.Payment == paid) && _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanDeclarationCapturedEvent>().Where(e => e.DeclarationId == paid.DeclarationId).ToArray()is [var fact] && // The initial producer owns DeclarationId. Native return/claim moves
 // this same paid object to its current parent; the preceding owner test
    // proves that new owner without rewriting the historical producer fact.
    fact.OwnerFrameId == paid.DeclarationId && fact.ActorSeat == paid.ActorSeat && fact.ProviderSeat == paid.ProviderSeat && fact.CardId == paid.Cost.CardId && fact.From == paid.Cost.From && fact.DeclaredKind == paid.DeclaredKind && fact.ActualTurn == q.ActualTurn && fact.TurnOwner == q.TurnOwner && fact.CaptureSequence == q.CaptureSequence && fact.LeaseCount == q.Leases.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanDeclarationLeaseCapturedEvent>().Where(e => e.DeclarationId == paid.DeclarationId).SequenceEqual(q.Leases.Select((l, i) => new WeiwanDeclarationLeaseCapturedEvent(paid.DeclarationId, i, l.FrameId)));
    internal void IssueWeiwanUse(CardUseFrame use)
    {
        if (!HasLeXiaoQiaoWeiwanCapability || use.Action is not { Type: CardActionType.Use } action)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || use.WeiwanUse is not null)
            throw new InvalidOperationException("Weiwan must capture its original accepted native Use once before payment.");
        var paid = use.AcceptedDeclarationPayment;
        if (paid is null && action.PhysicalCards is [var cost])
            paid = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(action.ProviderSeat, cost.CardId);
        WeiwanUseSnapshot snapshot;
        if (paid is not null && paid.ActorSeat == action.ActorSeat)
        {
            snapshot = paid.Weiwan ?? new(action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            if (paid.Weiwan is not null && !ValidWeiwanDeclaration(paid, snapshot))
                throw new InvalidOperationException("Weiwan lost its exact pre-paid declaration rights.");
        }
        else
            snapshot = CaptureWeiwanUseSnapshot(action.ActorSeat);
        var r = new WeiwanUseReceipt(use.Id, action.ActionId, use.CardKind, snapshot, paid?.DeclarationId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { WeiwanUse = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanUseAcceptedEvent(use.Id, action.ActionId, snapshot.ActorSeat, use.CardKind, snapshot.ActualTurn, snapshot.TurnOwner, snapshot.CaptureSequence, snapshot.Leases.Count, r.DeclarationId));
        for (var i = 0; i < snapshot.Leases.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanUseLeaseCapturedEvent(use.Id, i, snapshot.Leases[i].FrameId));
    }

    internal WeiwanUseReceipt? ValidIssuedWeiwanUse(long id, int actor)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { WeiwanUse: { } r, Action: { Type: CardActionType.Use } action } use || r.UseId != use.Id || r.ActionId != action.ActionId || use.SourceSeat != actor || action.ActorSeat != actor || !ValidWeiwanSnapshot(r.Snapshot) || !(use.CardKind == r.OriginalKind || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.OriginalKind) && use.CardKind == CardKind.FireSlash && _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use)) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanUseAcceptedEvent>().Where(e => e.UseId == id).ToArray()is not [var accepted] || accepted != new WeiwanUseAcceptedEvent(id, action.ActionId, r.Snapshot.ActorSeat, r.OriginalKind, r.Snapshot.ActualTurn, r.Snapshot.TurnOwner, r.Snapshot.CaptureSequence, r.Snapshot.Leases.Count, r.DeclarationId) || !_coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanUseLeaseCapturedEvent>().Where(e => e.UseId == id).SequenceEqual(r.Snapshot.Leases.Select((l, i) => new WeiwanUseLeaseCapturedEvent(id, i, l.FrameId))))
            return null;
        var originalActor = r.Snapshot.ActorSeat;
        foreach (var change in _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == id))
        {
            if (change.PreviousActorSeat != originalActor || change.ProviderSeat != action.ProviderSeat)
                return null;
            originalActor = change.ActorSeat;
        }

        return originalActor == actor ? r : null;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28620)]
    internal void CaptureWeiwanZeroMaterialUse(IGameEvent payload)
    {
        if (HasLeXiaoQiaoWeiwanCapability && payload is CardUseDeclaredEvent declared && declared.CardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { WeiwanUse: null, CardId: 0, Action: { Type: CardActionType.Use, PhysicalCards.Count: 0 } } use && use.PhysicalCardIds is null or { Count: 0 } && use.SourceSeat == declared.SourceSeat && use.CardKind == declared.CardKind)
            IssueWeiwanUse(use);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 28620)]
    internal void AssertWeiwanUses()
    {
        if (!HasLeXiaoQiaoWeiwanCapability)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(f => f.WeiwanUse is not null))
            if (ValidIssuedWeiwanUse(use.Id, use.SourceSeat)is null)
                throw new InvalidOperationException("Weiwan lost its exact accepted use, including the original absence of any target lease.");
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 40)]
    internal bool DistanceCardUseExemption40HasWeiwanUseDistance(ProgramCardDistanceQuery q) => HasWeiwanUseDistance(q.Actor, q.Target, q.Kind, q.CardUseFrameId);
}
