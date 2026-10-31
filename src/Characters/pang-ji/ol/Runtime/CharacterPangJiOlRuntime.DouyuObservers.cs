#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28132)]
    internal void AssertDouyuProgram(ProgramSkillFrame f)
    {
        if (f.Douyu is not { } r)
            return;
        if (f.SkillId != "ol:douyu" || f.InstructionIndex != 1 || r.Source != DouyuSource(f) || r.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveDouyuPreRevealSwapAndWinDraw || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !Enum.IsDefined(r.Stage) || f.WindowContext is not { Douyu: { } dc } context || context.ParentFrameId != r.WindowId || context.OwnerSeat != f.OwnerSeat || dc.ContestId != r.ContestId || dc.ComparisonIndex != r.ComparisonIndex || dc.SwapFrameId != r.SwapFrameId || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not DouyuWindowFrame window || !DouyuPindianChildRidesOn(f, window) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.ContestId)is not { } parent || !ValidDouyuContest(parent) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || r.ReplacementGainedIds is not System.Collections.IList { IsReadOnly: true } || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.OwnerSeat == f.OwnerSeat) != 1)
            throw new InvalidOperationException("Douyu lost its exact instruction, native window, original source or immutable draw invoice.");
        var contest = parent.DouyuContest!;
        if (r.Stage == DouyuStage.OpponentChoice)
        {
            if (window.Stage != DouyuWindowStage.BeforeReveal || dc.Stage != window.Stage || f.TriggerId != "pre-reveal" || r.IssuedSource != r.Source || r.SwapFrameId is not null || r.DrawStarted || r.Before != 0 || r.After != 0 || r.AttemptFrameId is not null || r.ActualDrawIds.Count != 0 || r.ReplacementGainedIds.Count != 0 || f.PendingMovementContinuation is not null || f.NativeDraw is not null || contest.Ready || contest.Paid)
                throw new InvalidOperationException("A private Douyu selection cannot reveal, pay, draw or replace an already assigned contest.");
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt && (prompt.PlayerSeat != f.OwnerSeat || !prompt.IsPrivate || prompt.ValidCardIds.Count != 0 || prompt.SkillPrompt?.SkillId != f.SkillId || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, DouyuChoices(f))))
                throw new InvalidOperationException("Douyu's blind prompt may expose only genuine opponent seats, never committed cards or ranks.");
            return;
        }

        if (window.Stage != DouyuWindowStage.WinDraw || dc.Stage != window.Stage || f.TriggerId != "swapped-win" || !contest.Paid || r.SwapFrameId is not { } swapId || contest.Swaps.SingleOrDefault(s => s.FrameId == swapId)is not { } swap || r.IssuedSource != swap.Source || r.Source != (swap.Source with
        {
            BindingId = "swapped-win"
        }

        ) || swap.GameplayHash != r.GameplayHash || !r.DrawStarted || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuResultRecordedEvent>().Count(e => e.ContestId == r.ContestId && e.ComparisonIndex == r.ComparisonIndex && DouyuSwappedPair(contest, swap, e.Result) && e.Result.WonBy(f.OwnerSeat)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuWinDrawStartedEvent>().Count(e => e.FrameId == f.Id && e.ContestId == r.ContestId && e.ComparisonIndex == r.ComparisonIndex && e.SwapFrameId == swapId && e.Source == swap.Source && e.RecipientSeat == f.OwnerSeat) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuRewardClosedEvent>().Any(e => e.ContestId == r.ContestId && e.SwapFrameId == swapId))
            throw new InvalidOperationException("Douyu requires the once-issued right of the actual swapped pair's native winner.");
        var returned = _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuWinDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == DouyuStage.DrawWaiting)
        {
            if (returned.Length != 0 || f.PendingMovementContinuation is not null || r.ActualDrawIds.Count != 0 || r.ReplacementGainedIds.Count != 0)
                throw new InvalidOperationException("Douyu cannot settle its native draw before the typed producer return.");
            return;
        }

        if (f.NativeDraw is not null || returned is not [var paid] || paid.ContestId != r.ContestId || paid.ComparisonIndex != r.ComparisonIndex || paid.SwapFrameId != swapId || paid.RecipientSeat != f.OwnerSeat || paid.RequestedCount != 2 || paid.Before != r.Before || paid.After != r.After || paid.AttemptFrameId != r.AttemptFrameId || paid.ActualCount != r.ActualDrawIds.Count || paid.ReplacementCount != r.ReplacementGainedIds.Count || r.ActualDrawIds.Count > 2 || r.ActualDrawIds.Distinct().Count() != r.ActualDrawIds.Count || r.ReplacementGainedIds.Distinct().Count() != r.ReplacementGainedIds.Count || r.ReplacementGainedIds.Count != 0 && r.ActualDrawIds.Count != 0 || !_coreProgramEventHistoryOperations.ProgramEventHistory<DouyuWinDrawEntityEvent>().Where(e => e.FrameId == f.Id && !e.Replacement).OrderBy(e => e.Index).Select(e => e.CardId).SequenceEqual(r.ActualDrawIds) || !_coreProgramEventHistoryOperations.ProgramEventHistory<DouyuWinDrawEntityEvent>().Where(e => e.FrameId == f.Id && e.Replacement).OrderBy(e => e.Index).Select(e => e.CardId).SequenceEqual(r.ReplacementGainedIds) || r.ActualDrawIds.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == DouyuDrawReason) != 1) || f.PendingMovementContinuation is not { } pending || !IsDouyuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Douyu lost the exact actual draw, replacement distinction or once-paid child continuation.");
    }

    internal bool CanContinueIssuedDouyu(ProgramSkillFrame f)
    {
        if (f.Douyu is { DrawStarted: true })
        {
            AssertDouyuProgram(f);
            return true;
        }

        return f.Douyu is null && f.TriggerId == "swapped-win" && f.WindowContext is { } context && IsIssuedDouyuCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    }

    internal bool IsDouyuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.Douyu is { DrawStarted: true, Stage: DouyuStage.DrawChildren } && effect?.Op == SkillProgramEffectOp.ResolveDouyuPreRevealSwapAndWinDraw && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    internal bool DouyuDrawMovement(ProgramSkillFrame f, CardMovementRecord movement)
    {
        var r = f.Douyu!;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement) && movement.Sequence > r.Before && movement.Sequence <= r.After && (movement.From == CardLocation.DrawPile && movement.To == CardLocation.Hand(f.OwnerSeat) && movement.Reason.Value == DouyuDrawReason && r.ActualDrawIds.Contains(movement.CardId) || movement.From == CardLocation.DiscardPile && movement.To == CardLocation.DrawPile && movement.Reason == CardMoveReasons.Reshuffle);
    }

    internal bool DouyuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Douyu is not { DrawStarted: true } r)
            return false;
        AssertDouyuProgram(f);
        if (r.Stage == DouyuStage.DrawWaiting)
            return f.NativeDraw is not null && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (r.Stage != DouyuStage.DrawChildren)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => DouyuDrawMovement(f, m));
    }

    internal bool DouyuObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        return _coreObserverSubtreesOperations.IsMovementObserverEdge(index) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child) || parent is ProgramLifecycleTriggerWindowFrame w && child is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == w.Id && context.Window == w.Window || parent is ProgramSkillFrame { Zenrun: not null } z && _characterSunHongOlOperations.ZenrunFirstChild(z, child) || parent is ProgramSkillFrame { Douyu: not null } own && DouyuFirstChild(own, child) || DouyuPindianChildRidesOn(child, parent) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(index);
    }

    internal ProgramSkillFrame? DouyuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterPangJiOlOperations.DouyuFirstChild(f, child), (index) => _coreObserverSubtreesOperations2.HasObserverSuffix(index, (n) => _characterPangJiOlOperations.DouyuObserverEdge(n), (n, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, dying)));
    internal bool HasDouyuDamageObserver(long id) => DouyuObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    internal bool HasDouyuBeforeDamageObserver(long id) => DouyuObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    internal bool IsDouyuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && DouyuObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    internal bool AllowsDouyuNestedDamage(ProgramSkillFrame f, int target, int amount, ProgramParticipantReference? source, DamageNature? nature, bool sourceLess)
    {
        if (f.AttackAttempt is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || DouyuObserverRoot()is not { } root || root.Id == f.Id)
            return false;
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f);
        return effect is { Op: SkillProgramEffectOp.Damage } && effect.Amount == amount && effect.ActorReference == source && effect.DamageNature == nature && !sourceLess && target == (effect.TargetReference is { } participant ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(f, participant) : global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(f, effect.Target));
    }

    internal bool IsDouyuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Douyu is { DrawStarted: true, Stage: DouyuStage.DrawChildren } && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28132)]
    internal bool AdvanceDouyuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DouyuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
