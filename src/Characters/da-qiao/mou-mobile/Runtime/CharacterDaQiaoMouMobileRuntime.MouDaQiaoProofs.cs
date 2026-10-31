#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoMouMobileRuntime
{
    internal bool ValidMouDaQiaoGuose(ProgramSkillFrame f) => f.MouDaQiaoGuose is { } r && f.SkillId == MouDaQiaoComposition.Guose && f.InstructionIndex == 1 && r.Source == MouDaQiaoSource(f) && r.GameplayHash == f.GameplayHash && Enum.IsDefined(r.Stage) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDaQiaoGuoseReservedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Phase == r.Phase && e.Branch == r.Branch && e.TargetSeat == r.TargetSeat) == 1 && (!r.Paid || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDaQiaoGuosePaidEvent>().Count(e => e.FrameId == f.Id && e.Branch == r.Branch && e.CardId == r.CardId && e.From == r.From) == 1);
    internal bool ValidMouDaQiaoPhase(ProgramSkillFrame f) => f.MouDaQiaoLiuliPhase is { } r && f.SkillId == MouDaQiaoComposition.Liuli && f.TriggerId == "marked-turn-start" && f.InstructionIndex == 1 && r.RecipientSeat == f.OwnerSeat && f.GameplayHash == r.GameplayHash && f.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } c && c.ParentFrameId == r.WindowId && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDaQiaoLiuliMarkGrantedEvent>().Count(e => e.GrantId == r.GrantId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.RecipientSeat == r.RecipientSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDaQiaoLiuliPhaseIssuedEvent>().Count(e => e.FrameId == f.Id && e.GrantId == r.GrantId && e.ActualTurn == r.ActualTurn && e.WindowId == r.WindowId) == 1;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31300)]
    internal bool ContinuePaidMouDaQiao(ProgramSkillFrame f) => f.MouDaQiaoGuose is { Paid: true } && ValidMouDaQiaoGuose(f) || PaidMouDaQiaoLiuli(f) || ValidMouDaQiaoPhase(f) || f.WindowContext is { } c && IsIssuedMouDaQiaoPhaseCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId ?? "", f.SkillInstanceId, f.GameplayHash, 0), c);
    [ProgramContinuation(ProgramContinuationKind.Resume, 31300)]
    internal bool ResumeMouDaQiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.MouDaQiaoLiuliPhase is not null && IsReturningMouDaQiaoLiuliPhase(f))
        {
            FinishMouDaQiaoLiuliPhase(f);
            return true;
        }

        if (f.MouDaQiaoGuose is not null)
        {
            ContinueMouDaQiaoGuose(id);
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31300)]
    internal bool ReturnMouDaQiaoMovement(ProgramSkillFrame f) => f.MouDaQiaoGuose is not null && ResumeMouDaQiao(f.Id);
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 31300)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 31300)]
    internal bool MouDaQiaoMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation m) => f.MouDaQiaoGuose is not null && ValidMouDaQiaoGuose(f) && e?.Op is (SkillProgramEffectOp.ResolveMouDaQiaoGuose or SkillProgramEffectOp.ResolveMouDaQiaoGuoseRemove) && m.SubjectSeat == f.OwnerSeat && m.BeforeCount == 0 && m.CoverageResultBind is null;
    internal bool MouDaQiaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouDaQiaoGuose(f))
            return false;
        var r = f.MouDaQiaoGuose!;
        if (child is CardUseFrame use)
            return r.Stage == MouDaQiaoGuoseStage.Using && use.Id == r.UseId && use.SourceSeat == f.OwnerSeat && use.CardKind == CardKind.Indulgence && use.CardId == r.CardId && use.Action is { PhysicalCards: [var physical] } a && physical.CardId == r.CardId && physical.From == r.From && a.ConversionChain.SequenceEqual([r.Source]);
        if (f.NativeDraw is not null && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return true;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal bool MouDaQiaoObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame f && _characterDaQiaoMouMobileOperations.MouDaQiaoFirstChild(f, _resolutionStackOperations2._resolutionStack()[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouDaQiaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterDaQiaoMouMobileOperations.MouDaQiaoFirstChild(f, c), (i) => _characterDaQiaoMouMobileOperations.MouDaQiaoObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 31300)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 31300)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 31300)]
    internal bool MouDaQiaoDamageObserver(long id) => MouDaQiaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31300)]
    internal bool MouDaQiaoDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouDaQiaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31300)]
    internal bool MouDaQiaoRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMouDaQiaoGuose(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f))
            return true;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouDaQiaoFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouDaQiaoObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31300)]
    internal bool AdvanceMouDaQiaoChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouDaQiaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
