#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunJianMouOlRuntime
{
    internal bool CanMouSunJianOlCounter(ProgramSkillFrame f)
    {
        var e = f.MouSunJianOlHulie!.Entry;
        return _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[e.ActorSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[e.TargetSeat].IsAlive && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[e.TargetSeat], _coreMatchParticipantsPlayersAccess._players[e.ActorSeat], CardKind.Slash, Suit.None, effectiveColor: null, effectiveRank: 0, physicalCardIds: []) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(e.TargetSeat, e.ActorSeat, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[e.ActorSeat], CardKind.Slash, Suit.None, null);
    }

    internal void IssueMouSunJianOlCounter(ProgramSkillFrame f)
    {
        var r = f.MouSunJianOlHulie!;
        if (!r.Accepted || !CanMouSunJianOlCounter(f))
        {
            FinishMouSunJianOl(f);
            return;
        }

        var outer = _coreCardUseLifecycleOperations.LifecycleCardUse(r.Entry.UseId)!;
        var wasActive = outer.CardAttack?.Active == true;
        if (wasActive)
            _coreCardAttackStateOperations.UpdateCardAttackState(outer.Id, a => a!with { Active = false });
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlHulie = r with { Stage = MouSunJianOlHulieStage.Issuing, OuterWasActive = wasActive } });
        // Reuse the committed native foreign-actor empty-material Slash producer.
        _coreProgramVirtualSlashOffersOperations.BeginOfferedProgramVirtualSlash(f, r.Entry.TargetSeat, f.OwnerSeat);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 32501)]
    internal void BindMouSunJianOlCounter(IGameEvent fact)
    {
        if (fact is not CardUseDeclaredEvent d || _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is not { } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { MouSunJianOlHulie: { Stage: MouSunJianOlHulieStage.Issuing, Accepted: true } r } f)
            return;
        if (!ValidMouSunJianOlHulie(f) || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || a.ActorSeat != r.Entry.TargetSeat || a.ProviderSeat != r.Entry.TargetSeat || a.RequesterSeat is not null || !a.TargetSeats.SequenceEqual([f.OwnerSeat]) || !use.TargetSeats.SequenceEqual([f.OwnerSeat]))
            throw new InvalidOperationException("Hulie cannot bind another actor, provider, target or original zero-material use.");
        var origin = new MouSunJianOlCounterOrigin(f.Id, MouSunJianOlSource(f), f.GameplayHash, r.Entry.UseId, r.Entry.ActionId, use.Id, a.ActionId, r.Entry.TargetSeat, f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouSunJianOlHulie = r with { Stage = MouSunJianOlHulieStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouSunJianOlCounter = origin });
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && a.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && use.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], null))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(use.Id, a.ActorSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(a.ActorSeat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlCounterIssuedEvent(origin));
    }

    internal bool IsMouSunJianOlCounterUse(CardUseFrame use)
    {
        if (use.MouSunJianOlCounter is not { } o || use.CardId != 0 || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActionId != o.ActionId || o.UseId != use.Id || a.ProviderSeat != o.ActorSeat || a.RequesterSeat is not null || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == o.FrameId)is not ProgramSkillFrame { MouSunJianOlHulie: { Stage: MouSunJianOlHulieStage.NativeUse, Accepted: true, NativeReturned: false } r } parent || !ValidMouSunJianOlHulie(parent) || o.Source != MouSunJianOlSource(parent) || o.GameplayHash != parent.GameplayHash || r.UseId != use.Id || r.ActionId != a.ActionId || o.OriginalUseId != r.Entry.UseId || o.OriginalActionId != r.Entry.ActionId || o.ActorSeat != r.Entry.TargetSeat || o.TargetSeat != parent.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlCounterIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.ActorSeat, o.ActorSeat) : use.SourceSeat == o.ActorSeat && a.ActorSeat == o.ActorSeat;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 32501)]
    internal bool FinishMouSunJianOlCounterAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouSunJianOlCounter: not null } use || !IsMouSunJianOlCounterUse(use))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(32501)]
    internal void ValidateMouSunJianOlCounterUse(CardUseFrame use)
    {
        if (use.MouSunJianOlCounter is not null && (!IsMouSunJianOlCounterUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Hulie must complete its entire genuine counter Use before returning.");
    }

    [NativeUseReturn(32501)]
    internal void ReturnMouSunJianOlCounterUse(CardUseFrame use)
    {
        if (use.MouSunJianOlCounter is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouSunJianOlHulie!;
        if (r.UseId != use.Id || r.ActionId != o.ActionId || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Hulie lost its exact whole-Use typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouSunJianOlHulie = r with { NativeReturned = true } });
        if (use.CardAttack is not { Active: true })
            CompleteMouSunJianOlCounter(o.FrameId);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 32501)]
    internal bool ReturnMouSunJianOlCounterAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouSunJianOlHulie: { Stage: MouSunJianOlHulieStage.NativeUse, NativeReturned: true } r } f || r.UseId != completion.ResolutionId)
            return false;
        if (completion.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Hulie returned to a different owning program.");
        CompleteMouSunJianOlCounter(f.Id);
        return true;
    }

    internal void CompleteMouSunJianOlCounter(long frameId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var r = f.MouSunJianOlHulie!;
        if (!r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlCounterReturnedEvent>().Any(e => e.FrameId == frameId))
            throw new InvalidOperationException("Hulie cannot repeat a returned counter Use.");
        if (r.OuterWasActive && _coreCardUseLifecycleOperations.LifecycleCardUse(r.Entry.UseId)?.CardAttack is { Active: false })
            _coreCardAttackStateOperations.UpdateCardAttackState(r.Entry.UseId, a => a!with { Active = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlCounterReturnedEvent(frameId, r.UseId, r.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlHulie = r with { Stage = MouSunJianOlHulieStage.Children }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ResumeMouSunJianOl(f.Id);
    }
}
