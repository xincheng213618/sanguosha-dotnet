#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{

    internal bool CanContinueIssuedDouyu(ProgramSkillFrame f)
    {
        if (f.Douyu is { DrawStarted: true })
        {
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

    internal ProgramSkillFrame? DouyuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterPangJiOlOperations.DouyuFirstChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _characterPangJiOlOperations.DouyuObserverEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying)));
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
