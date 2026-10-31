#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoSiOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 90)]
    internal bool GuoSiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasGuoSiCapability;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 100)]
    internal bool GuoSiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasGuoSiCapability;
    internal bool HasGuoSiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferSidaoCompletedPairSnatch);

    internal bool GuoSiQualified(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCompleted).Any(b => ProgramInstructionResolver.Default.Features(b.Trigger).HasOperation(SkillProgramEffectOp.OfferSidaoCompletedPairSnatch));
    internal bool GuoSiResponseUse(CardActionContext a)
    {
        if (a.Type != CardActionType.Response || a.ActorSeat != a.ProviderSeat || a.RequesterSeat is not null || a.ResponderSeat != a.ActorSeat)
            return false;
        if (a.EffectiveKind == CardKind.Nullification)
            return _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } n && a.OpponentSeat == n.SourceSeat && a.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(n);
        return a.EffectiveKind == CardKind.Dodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.EffectiveCardKind is { } incoming && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) && attack.TargetSeat == a.ActorSeat && a.OpponentSeat == attack.SourceSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId == a.ParentActionId;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 500)]
    internal void ObserveGuoSiAcceptedUse(IGameEvent payload)
    {
        if (payload is not CardActionAcceptedEvent e || !HasGuoSiCapability)
            return;
        var a = e.Action;
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || a.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || a.Type != CardActionType.Use && !GuoSiResponseUse(a) || !GuoSiQualified(a.ActorSeat) && _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].GuoSiUses is null)
            return;
        var p = _coreMatchParticipantsPlayersAccess._players[a.ActorSeat];
        var old = p.GuoSiUses ?? [];
        if (old.Any(s => s.ActionId == a.ActionId))
            return;
        p.GuoSiUses = Array.AsReadOnly(old.Where(s => s.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && s.Phase == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId).Append(new GuoSiUseStamp(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, a.ActionId, 0, a.ActorSeat, false, false, [])).OrderBy(s => s.ActionId).TakeLast(2).ToArray());
    }

    internal void BindGuoSiAcceptedUse(CardUseFrame use)
    {
        if (!HasGuoSiCapability || use.Action is not { } a || _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].GuoSiUses is not { } list)
            return;
        _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].GuoSiUses = Array.AsReadOnly(list.Select(s => s.ActionId == a.ActionId ? s with { NativeUseId = use.Id } : s).ToArray());
    }

    internal void CaptureGuoSiFinalizedUse(long id, CardActionContext a)
    {
        if (!HasGuoSiCapability || _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].GuoSiUses is not { } list || !list.Any(s => s.ActionId == a.ActionId))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { } actual } use || actual.ActionId != a.ActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(actual, a) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, a))
            throw new InvalidOperationException("Sidao finalized targets require their actual accepted native action.");
        _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].GuoSiUses = Array.AsReadOnly(list.Select(s => s.ActionId == a.ActionId ? s with { NativeUseId = id, Finalized = true, Targets = _characterSuFeiOlOperations.SuFeiPrimaryTargets(a) } : s).ToArray());
    }

    internal void CaptureGuoSiCompletedUse(long id, CardActionContext a)
    {
        if (!HasGuoSiCapability || _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].GuoSiUses is not { } list || !list.Any(s => s.ActionId == a.ActionId))
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        var response = GuoSiResponseUse(a);
        if (response && (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(f => f.Id == id)is not { CompletedResponseReturn: { } ret } window || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(window.Action, a) || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(window) || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(a, ret.Kind)))
            throw new InvalidOperationException("Sidao response completion requires its exact native use-return parent.");
        if (!response && (use is not { Step: ResolutionFrameStep.Completed, Action: { } actual } || actual.ActionId != a.ActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(actual, a) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, a) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.CardKind == use.CardKind) != 1))
            throw new InvalidOperationException("Sidao completion lost its native action.");
        var next = Array.AsReadOnly(list.Select(s => s.ActionId == a.ActionId ? s with { Completed = true, Finalized = response || s.Finalized, Targets = response ? [] : _characterSuFeiOlOperations.SuFeiPrimaryTargets(a) } : s).ToArray());
        _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].GuoSiUses = next;
        if (response || use is null || next.Count != 2 || !GuoSiQualified(a.ActorSeat) || next.Any(s => !s.Finalized || !s.Completed || s.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || s.Phase != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) || use.GuoSiPair is not null)
            return;
        var first = next[0];
        var second = next[1];
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuoSiPairQualifiedEvent>().Any(e => e.OwnerSeat == a.ActorSeat && e.FirstActionId == first.ActionId && e.SecondActionId == second.ActionId))
            return;
        var common = first.Targets.Intersect(second.Targets).Where(s => s != a.ActorSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).ToArray();
        if (common.Length == 0)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { GuoSiPair = new(first, second, common) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuoSiPairQualifiedEvent(id, a.ActorSeat, first.ActionId, second.ActionId));
    }

    internal bool HasGuoSiTargetEffect(int actor, int target, DirectedTurnCardPolicyEffect effect)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || _coreMatchParticipantsPlayersAccess._players[actor].GuoSiPacts is not { } pacts)
            return false;
        return pacts.Any(p => p.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && p.Actor == actor && p.Target == target && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuoSiPactPaidEvent>().Count(e => e.Pact == p) == 1 && (p.Branch == 0 ? effect == DirectedTurnCardPolicyEffect.ForbidTarget : effect is DirectedTurnCardPolicyEffect.IgnoreDistance or DirectedTurnCardPolicyEffect.BypassSlashLimit));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 510)]
    internal void ObserveGuoSiTurnExpiry(IGameEvent payload)
    {
        if (payload is not TurnEndedEvent || !HasGuoSiCapability)
            return;
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
        {
            if (p.GuoSiPacts is not null)
                p.GuoSiPacts = null;
            if (p.GuoSiUses is not null)
                p.GuoSiUses = null;
        }
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 470)]
    internal bool CanOfferGuoSi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (!ProgramInstructionResolver.Default.Features(t).HasOperation(SkillProgramEffectOp.OfferSidaoCompletedPairSnatch))
            return true;
        return _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && w.CardUse is { } i && _coreCardUseLifecycleOperations.LifecycleCardUse(i.ParentCardUseFrameId)is { GuoSiPair: { } q, Action: { } a } && a.ActionId == i.CardActionId && GuoSiQualified(c.OwnerSeat) && SidaoActions(c.OwnerSeat, q).Count > 0;
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 100)]
    internal bool DistanceCardUseExemption100HasGuoSiTargetEffect(ProgramCardDistanceQuery q) => HasGuoSiTargetEffect(q.Actor.Seat, q.Target.Seat, DirectedTurnCardPolicyEffect.IgnoreDistance);
}
