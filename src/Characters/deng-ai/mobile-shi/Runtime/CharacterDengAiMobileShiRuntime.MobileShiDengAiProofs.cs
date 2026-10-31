#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiMobileShiRuntime
{
    internal bool ShiDaWindowMatches(ProgramSkillFrame f, SkillProgramTriggerWindow expected)
    {
        if (f.WindowContext is not { } context || context.Window != expected || context.OwnerSeat != f.OwnerSeat || f.TriggerId is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId);
        if (parent is DeferredTurnEndFrame ended)
            return expected == SkillProgramTriggerWindow.AfterTurnEnded && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(ended, f, context);
        if (parent is ProgramLifecycleTriggerWindowFrame lifecycle)
            return lifecycle.Window == expected && lifecycle.CandidateIndex >= 0 && lifecycle.CandidateIndex < lifecycle.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, lifecycle.Candidates[lifecycle.CandidateIndex]);
        if (parent is CardsMovedTriggerWindowFrame moved)
            return expected == SkillProgramTriggerWindow.CardsMoved && context.MovementBatch is { } batch && batch.Id == moved.Batch.Id && batch.Movements.SequenceEqual(moved.Batch.Movements) && moved.CandidateIndex >= 0 && moved.CandidateIndex < moved.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, moved.Candidates[moved.CandidateIndex]);
        return parent is ProgramCardTriggerWindowFrame card && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(card) == expected && card.CandidateIndex >= 0 && card.CandidateIndex < card.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(card.Candidates[card.CandidateIndex])) && context.CardUse?.CardActionId == card.Action.ActionId;
    }

    internal bool ValidShiDa(ProgramSkillFrame f)
    {
        if (f.ShiDengAi is not { } r || r.Source != ShiDaSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || f.InstructionIndex != 1 || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.WindowId != (f.WindowContext?.ParentFrameId ?? 0) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaStartedEvent>().Count(e => e == new ShiDaStartedEvent(f.Id, r.Source, r.Hash, r.Op, r.Turn, r.TurnOwner, r.WindowId)) != 1)
            return false;
        return r.Op switch
        {
            SkillProgramEffectOp.ResolveMobileShiDengAiFull => f.SkillId == ShiDaTuntian && f.TriggerId == "full-charge" && ShiDaWindowMatches(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),
            SkillProgramEffectOp.ResolveMobileShiDengAiTuntian => f.SkillId == ShiDaTuntian && f.TriggerId is null && f.ActivationId == "spend-charge" && f.WindowContext is null && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == f.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play,
            SkillProgramEffectOp.ResolveMobileShiDengAiJixi => f.SkillId == ShiDaJixi && f.TriggerId == "ending-jixi" && ShiDaWindowMatches(f, SkillProgramTriggerWindow.AfterTurnEnded),
            _ => false
        };
    }
    internal bool ShiDaMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.ShiDengAi is { Paid: true } r && ValidShiDa(f) && effect?.Op == r.Op && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && r.Stage is ShiDaStage.DrawChildren or ShiDaStage.RewardChildren or ShiDaStage.HeartChildren or ShiDaStage.CostChildren or ShiDaStage.NativeChildren;
    internal bool ShiDaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ShiDengAi is not { Paid: true } r || !ValidShiDa(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == ShiDaStage.Drawing && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == 1 && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardUseFrame use)
            return r.Stage == ShiDaStage.NativeUse && use.Id == r.NativeUseId && ValidShiDaVirtualSnatchUse(use);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (f.PendingMovementContinuation is not { } pending || !ShiDaMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) || !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
                return false;
            if (r.Stage == ShiDaStage.NativeChildren)
                return r.NativeBatches.Contains(moved.Batch.Id) && moved.Batch.ParentFrameId == r.NativeUseId && moved.Batch.AwaitingProgramFrameId is null && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaNativeBatchEvent>().Count(e => e == new ShiDaNativeBatchEvent(f.Id, r.NativeUseId, moved.Batch.Id, moved.Batch.ParentBatchId, moved.Batch.Movements.Count)) == 1;
            return moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        }

        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ShiDaNativeEdge(int i)
    {
        if (_resolutionStackOperations._resolutionStack[i - 1] is CardUseFrame use && ValidShiDaVirtualSnatchUse(use) && _resolutionStackOperations._resolutionStack[i] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[i - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { } native && ValidShiDaVirtualSnatchUse(native) && _resolutionStackOperations._resolutionStack[i] is ProgramSkillFrame f && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, parent.Candidates[parent.CandidateIndex]);
        return false;
    }

    internal bool ShiDaChildEdge(int i) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || ShiDaNativeEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiDaFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiDaChildSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterDengAiMobileShiOperations.ShiDaChildEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? ShiDaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterDengAiMobileShiOperations.ShiDaFirstChild(f, child), (i) => _characterDengAiMobileShiOperations.ShiDaChildSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 50300)]
    internal bool ShiDaDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ShiDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 50300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 50300)]
    internal bool ShiDaRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiDaFirstChild(f, recovery))
            return true;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && ShiDaFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && ShiDaChildSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 50300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 50300)]
    internal bool KeepPaidShiDa(ProgramSkillFrame f) => f.ShiDengAi is { Paid: true } && ValidShiDa(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 50300)]
    internal bool AdvanceShiDaChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiDaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
