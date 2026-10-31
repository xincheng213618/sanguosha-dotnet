#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangJiMouRuntime
{
    internal void IssueMouJjRepeat(ProgramSkillFrame f)
    {
        if (f.MouJiangJi is not { } r || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame window || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { } use)
            throw new InvalidOperationException("Yingshi lost its issued original native window.");
        var state = MouJiangJi.IssueEffect(f, use, window);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouJiangJiRounds = state });
    }

    internal bool HasUnstartedMouJjEffect(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouJiangJiRounds: { } r, Action: { } a } u && a.ActionId == r.ActionId && r.Started < r.Grants.Count && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(u) && _coreMatchOutcomeWinnerAccess._winner == Winner.None;
    internal bool IsMouJjEffectRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouJiangJiRounds is { Started: > 0 };
    internal bool IsTargetScopedMouJjEffect(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouJiangJiRounds: { Active: true, Started: > 0 } r, Action: { } a } && r.ActionId == a.ActionId;
    internal bool IsMouJjEffectSubset(CardUseFrame use, IReadOnlyList<int> targets) => use is { MouJiangJiRounds: { Active: true, Started: > 0 } r, Action: { } a } && a.ActionId == r.ActionId && r.ActiveIndex >= 0 && r.ActiveIndex < use.TargetSeats.Count && use.TargetSeats[r.ActiveIndex] == r.ActiveTarget && targets.SequenceEqual(use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Skip(r.ActiveIndex).Take(2) : [r.ActiveTarget]) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangJiEffectStartedEvent>().Count(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.FrameId == r.Grants[r.Started - 1].FrameId && e.Round == r.Started && e.Target == r.ActiveTarget) == 1;
    internal void CompleteMouJjEffect(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouJiangJiRounds: { Active: true } } use)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || use.NativeDraw is not null || use.NativeCardEffectDraw is not null || use.WumiaoFlood is not null || use.DragonBoatCardEffect is not null || use.GuanduCardEffect is not null)
            throw new InvalidOperationException("Yingshi cannot retire its target-local native effect before actual children return.");
        var state = MouJiangJi.CompleteEffect(use);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { MouJiangJiRounds = state, TargetIndex = u.TargetSeats.Count, SequentialTrick = null });
    }

    internal bool TryBeginMouJjEffect(long id, bool afterAttack)
    {
        if (!HasUnstartedMouJjEffect(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouJiangJiRounds: { } r, Action: { } a } use || use.ExtraCardEffect is { WholeGroupRepeated: false } || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || _characterDaQiaoLeOperations.HasUnstartedZixiExtraEffect(id) || _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Yingshi cannot pass its original whole native effect.");
        var state = MouJiangJi.BeginEffect(use, _characterFeiYiOlOperations.NativeExtraEffectState(use));
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { MouJiangJiRounds = state, TargetIndex = state.ActiveIndex < 0 ? use.TargetSeats.Count : state.ActiveIndex, Step = ResolutionFrameStep.ResolvingEffect, SequentialTrick = null, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        if (state.ActiveIndex < 0 || !_coreMatchParticipantsPlayersAccess._players[state.ActiveTarget].IsAlive)
        {
            CompleteMouJjEffect(id);
            return false;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, a.ActionId, state.ActiveIndex, state.ActiveTarget, use.CardKind));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
        _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true);
        var trick = state.Native.Trick!;
        IReadOnlyList<int> targets = use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Skip(state.ActiveIndex).Take(2).ToArray() : [state.ActiveTarget];
        _coreNativeNullificationOperations.BeginNullificationWindow(id, card, use.SourceSeat, targets, trick.ActionKind, null, trick.RequiredCardKind, use.CardKind);
        return true;
    }

    internal void AssertMouJjEffects()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.MouJiangJiRounds is not null))
            if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || !MouJiangJi.ValidEffects(use))
                throw new InvalidOperationException("Yingshi lost its original Use, once-issued selected target or ordered native effect return.");
    }
}
