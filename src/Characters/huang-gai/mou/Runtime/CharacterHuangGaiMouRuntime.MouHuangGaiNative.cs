#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangGaiMouRuntime
{
    internal IReadOnlyList<int> MouHgTargets(ProgramSkillFrame f)
    {
        if (f.MouHuangGai is not { Reveals.Count: > 0 } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        var shown = r.Reveals[^1];
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(shown.Kind) || _cardZonesOperations._cardZones.GetLocation(shown.CardId) != CardLocation.Processing)
            return[];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(shown.CardId);
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, t, card, effectiveKind: shown.Kind)).Select(t => t.Seat).ToArray());
    }

    internal void IssueMouHgSlash(ProgramSkillFrame f, int targetSeat)
    {
        if (!ValidMouHg(f) || f.MouHuangGai is not { Stage: MouHgStage.NativeUse, Native: null } r || !MouHgTargets(f).Contains(targetSeat))
            throw new InvalidOperationException("Quzhou requires its actual revealed Slash and current legal target.");
        var shown = r.Reveals[^1];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(shown.CardId);
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [targetSeat], playedCardKind: shown.Kind, physicalCardIds: [card.Id], ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, shown.Kind));
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.Action is not { Type: CardActionType.Use } a || a.PhysicalCards is not [{ From: var from, CardId: var material }] || from != CardLocation.Processing || material != card.Id)
            throw new InvalidOperationException("Quzhou must use its real Processing entity.");
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame { MouHuangGai: { Stage: MouHgStage.NativeUse, Native: null } paid } parent || parent.Id != f.Id || paid.Source != r.Source || paid.GameplayHash != r.GameplayHash || !ValidMouHg(parent) || use.SourceSeat != actor.Seat || a.ActorSeat != actor.Seat || a.ProviderSeat != actor.Seat)
            throw new InvalidOperationException("Quzhou lost its exact original parent after issuing the native child.");
        var origin = new MouHgNativeUse(f.Id, id, a.ActionId, card.Id, shown.Kind, actor.Seat, targetSeat);
        f = parent;
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouHuangGai = f.MouHuangGai!with { Native = origin } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { MouHgQuzhou = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHgNativeIssuedEvent(origin));
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targetSeat, card: card, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: shown.Kind, physicalCards: [card], ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, shown.Kind), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.UnlimitedUse != true)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, shown.Kind);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, shown.Kind, actor.Seat, targetSeat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [targetSeat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool MouHgUseRidesOn(ProgramSkillFrame f, CardUseFrame u) => ValidMouHg(f) && f.MouHuangGai is { Stage: MouHgStage.NativeUse, Native: { } r, NativeReturned: false } && u.MouHgQuzhou == r && r.FrameId == f.Id && r.UseId == u.Id && u.CardId == r.CardId && u.Action is { Type: CardActionType.Use } a && a.ActionId == r.ActionId && a.ProviderSeat == r.Actor && a.PhysicalCards is [{ CardId: var id, From: var from }] && id == r.CardId && from == CardLocation.Processing && u.PhysicalCardIds is [var physical] && physical == id && f.MouHuangGai.Reveals.Any(e => e.CardId == id && e.Kind == r.Kind) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(u, r.Actor, r.Actor) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHgNativeIssuedEvent>().Count(e => e.Origin == r) == 1;
    [NativeUseValidation(35400)]
    internal void ValidateMouHgUse(CardUseFrame use)
    {
        if (use.MouHgQuzhou is { } o && (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.FrameId)is not { } f || !MouHgUseRidesOn(f, use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Quzhou cannot retire another or incomplete native Slash.");
    }

    [NativeUseReturn(35400)]
    internal void ReturnMouHgUse(CardUseFrame use)
    {
        if (use.MouHgQuzhou is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouHuangGai!;
        if (!MouHgUseRidesOn(f, use) || r.NativeReturned || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Quzhou requires its once-completed original native Slash return.");
        MouHuangGaiRules.UseReturned(o, use.CardAttack is { Active: true });
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 35400)]
    internal bool ReturnMouHgAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouHuangGai: { Stage: MouHgStage.NativeUse, NativeReturned: true, Native: { } o } } f || completion.ResolutionId != o.UseId)
            return false;
        MouHuangGaiRules.SettleUse(o);
        return true;
    }

    internal bool MouHgFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouHg(f) || f.MouHuangGai is not { } r)
            return false;
        if (child is CardUseFrame use)
            return MouHgUseRidesOn(f, use);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal ProgramSkillFrame? MouHgObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuangGaiMouOperations.MouHgFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 35400)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 35400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 35400)]
    internal bool MouHgDamageObserver(long id) => MouHgObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 35400)]
    internal bool MouHgDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouHgObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 35400)]
    internal bool AdvanceMouHgChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouHgObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
