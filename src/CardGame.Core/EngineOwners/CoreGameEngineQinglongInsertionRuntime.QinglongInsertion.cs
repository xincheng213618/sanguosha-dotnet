#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineQinglongInsertionRuntime
{
    internal void SuspendFactionRequestForQinglongChoice(NativeOwners.CardAttackHandle attack)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not { AwaitingProviders: false, ActiveAttack: { } supplied } request || !CoreNativeDamageHandlesRuntime.SameAttackOwner(supplied, attack))
            return;
        _coreCardAttackStateOperations.UpdateCardContinuations(attack.ResolutionId, state => state with { QinglongCrescentBlade = state.QinglongCrescentBlade!with { SuspendedFactionRequestOwnerId = request.OwnerFrameId } });
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
    }

    internal void RestoreFactionRequestAfterQinglongChoice(long ownerId)
    {
        if (_coreCardAttackStateOperations.GetCardContinuations(ownerId).QinglongCrescentBlade is not { SuspendedFactionRequestOwnerId: { } request } saved)
            return;
        _coreCardAttackStateOperations.UpdateCardContinuations(ownerId, state => state with { QinglongCrescentBlade = saved with { SuspendedFactionRequestOwnerId = null } });
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(request);
    }

    internal void SuspendContinuationForQinglongFollowup(long outerResolutionId, NativeOwners.FactionCardRequestHandle? followupRequest = null)
    {
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId != outerResolutionId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame original || original.Id != outerResolutionId || original.Continuations.QinglongFollowup is { Active: true })
            throw new InvalidOperationException("A Qinglong insertion requires its exact unfinished dodged Slash.");
        var request = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest;
        var savedRequest = original.Continuations.QinglongCrescentBlade?.SuspendedFactionRequestOwnerId ?? (request is not null && !CoreNativeDamageHandlesRuntime.SameContinuationOwner(request, followupRequest) ? request.OwnerFrameId : (long? )null);
        var suspension = new QinglongFollowupState(outerResolutionId, null, _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd?.OwnerFrameId, EngineStatus.Running, BorrowedSwordOwnerId: _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword?.OwnerFrameId, FactionRequestOwnerId: savedRequest);
        _coreCardAttackStateOperations.UpdateCardContinuations(outerResolutionId, state => state with { QinglongFollowup = suspension, QinglongCrescentBlade = state.QinglongCrescentBlade is { } choice ? choice with { SuspendedFactionRequestOwnerId = null } : null });
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = null;
        _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
        if (request is not null && !CoreNativeDamageHandlesRuntime.SameContinuationOwner(request, followupRequest))
            _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
    }

    internal void BindQinglongInsertedUse(CardUseFrame child)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == child.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not CardUseFrame original || original.Continuations.QinglongFollowup is not { Active: true, FollowupUseId: null } suspension)
            return;
        if (original.Action is not { } action || child.Action?.ParentActionId != action.ActionId || child.SourceSeat != original.SourceSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(child.CardKind) || original.CardAttack is not { } attack || !child.Action.TargetSeats.SequenceEqual([attack.TargetSeat]))
            throw new InvalidOperationException("A Qinglong inserted use changed its parent, actor, or exact dodging target.");
        _coreCardAttackStateOperations.UpdateCardContinuations(original.Id, state => state with { QinglongFollowup = suspension with { FollowupUseId = child.Id } });
    }

    internal bool RestoreContinuationAfterQinglongFollowup(long completedOwnerId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame original || original.Continuations.QinglongFollowup is not { Active: true } suspension || suspension.FollowupUseId != completedOwnerId)
            return false;
        // Schedule the parent, rather than recursively finishing a whole chain
        // inside this child's native return. One engine step retires one parent.
        _coreCardAttackStateOperations.UpdateCardContinuations(original.Id, state => state with { QinglongFollowup = suspension with { ChildCompleted = true } });
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 20)]
    internal bool TryResumeQinglongInsertion()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame original || original.Continuations.QinglongFollowup is not { Active: true, ChildCompleted: true } suspension)
            return false;
        _coreCardAttackStateOperations.UpdateCardContinuations(original.Id, state => state with { QinglongFollowup = suspension with { Active = false } });
        if (suspension.FangtianOwnerId is { } fangtian)
            _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = _kernelCoreMainOperations.CreateFangtianHalberdHandle(fangtian);
        if (suspension.BorrowedSwordOwnerId is { } borrowed)
            _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = _kernelCoreMainOperations.CreateBorrowedSwordHandle(borrowed);
        if (suspension.FactionRequestOwnerId is { } request)
            _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(request);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(original.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }
}
