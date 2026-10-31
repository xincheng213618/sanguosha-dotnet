#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiMouOlRuntime
{
    internal bool ValidMouLz(ProgramSkillFrame f)
    {
        if (f.MouLuZhiOl is not { } r || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || r.Source != MouLzSource(f) || r.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Selected.Count != r.Selected.Distinct().Count() || f.WindowContext is not { } w || w.ParentFrameId != r.WindowId || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlStartedEvent>().Count(e => e == new MouLuZhiOlStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.OriginalId, r.OriginalActionId, r.ActorSeat, r.TargetSeat, r.Turn, r.BatchId)) != 1)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1].Id != r.WindowId)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouLuZhiLiance)
            return f.SkillId == MouLuZhiOlComposition.Liance && r.BatchId > 0 && w.MovementBatch?.Id == r.BatchId && _resolutionStackOperations._resolutionStack[n - 1] is CardsMovedTriggerWindowFrame moved && moved.CandidateIndex >= 0 && moved.CandidateIndex < moved.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, moved.Candidates[moved.CandidateIndex]) && r.Stage is MouLuZhiOlStage.Drawing or MouLuZhiOlStage.DrawChildren;
        if (f.SkillId != MouLuZhiOlComposition.Sibing)
            return false;
        if (_resolutionStackOperations._resolutionStack[n - 1] is MouLuZhiOlDirectWindowFrame dw)
        {
            if (dw.Context != w || !ExactMouLzDirectWindow(dw) || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, dw.Candidate) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == dw.ParentFrameId)is not ProgramSkillFrame { MouLuZhiOlDirectDuel: { } d } || d.FrameId != r.OriginalId || d.Instruction != r.OriginalInstruction || r.OriginalActionId != 0 || r.ActorSeat != d.Actor || r.TargetSeat != d.Target)
                return false;
        }
        else if (_resolutionStackOperations._resolutionStack[n - 1] is ProgramCardTriggerWindowFrame window && MouLzContextUse(w)is { Action: { } a } use)
        {
            if (window.ParentFrameId != use.Id || r.OriginalId != use.Id || r.OriginalActionId != a.ActionId || window.Action.ActionId != a.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])))
                return false;
        }
        else
            return false;
        return r.Op == SkillProgramEffectOp.ResolveMouLuZhiSibingPress ? f.TriggerId == "red-press" && r.Stage is MouLuZhiOlStage.Red or MouLuZhiOlStage.RedChildren or MouLuZhiOlStage.TargetRed or MouLuZhiOlStage.TargetRedChildren : r.Op == SkillProgramEffectOp.ResolveMouLuZhiSibingRetaliation && f.TriggerId == "undamaged-retaliation" && r.Stage is MouLuZhiOlStage.Black or MouLuZhiOlStage.BlackChildren or MouLuZhiOlStage.SlashTarget or MouLuZhiOlStage.Issuing or MouLuZhiOlStage.NativeUse or MouLuZhiOlStage.NativeReturned;
    }

    internal bool DrainMouLz(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 32900)]
    internal bool MouLzMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => ValidMouLz(f) && p == f.PendingMovementContinuation && e?.Op == f.MouLuZhiOl!.Op && p.BeforeCount == 0 && p.CoverageResultBind is null && p.SubjectSeat == (f.MouLuZhiOl.Stage == MouLuZhiOlStage.TargetRedChildren ? f.MouLuZhiOl.TargetSeat : f.OwnerSeat) && f.MouLuZhiOl.Stage is MouLuZhiOlStage.RedChildren or MouLuZhiOlStage.TargetRedChildren or MouLuZhiOlStage.BlackChildren or MouLuZhiOlStage.DrawChildren;
    internal bool MouLzLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouLz(f) || f.MouLuZhiOl is not { Stage: MouLuZhiOlStage.RedChildren or MouLuZhiOlStage.TargetRedChildren or MouLuZhiOlStage.BlackChildren } r)
            return false;
        var seat = r.Stage == MouLuZhiOlStage.TargetRedChildren ? r.TargetSeat : f.OwnerSeat;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => r.Selected.Contains(m.CardId) && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(seat) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == MouLzDiscard))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouLzDiscard && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat && recovery.Attempt.Amount == 1;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == seat && e.Reason.Value == MouLzDiscard && e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouLzFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouLz(f))
            return false;
        var r = f.MouLuZhiOl!;
        if (r.Stage == MouLuZhiOlStage.NativeUse && child is CardUseFrame u)
            return IsMouLzCounterUse(u);
        if (r.Stage == MouLuZhiOlStage.Drawing && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return f.NativeDraw is not null;
        if (MouLzLionChild(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame m)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, m) && m.Batch.Movements.All(x => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(x) && x.Sequence > r.Before && x.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouLzNativeEdge(int n)
    {
        if (_resolutionStackOperations._resolutionStack[n - 1] is CardUseFrame { MouLuZhiOlCounter: not null } u && IsMouLzCounterUse(u) && _resolutionStackOperations._resolutionStack[n] is ProgramCardTriggerWindowFrame w)
            return w.ParentFrameId == u.Id && w.Action.ActionId == u.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[n - 1] is ProgramCardTriggerWindowFrame window && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is { MouLuZhiOlCounter: not null } use && IsMouLzCounterUse(use) && _resolutionStackOperations._resolutionStack[n] is ProgramSkillFrame f && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext(window, window.Candidates[window.CandidateIndex]);
        if (_resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame { MouLuZhiOlDirectDuel: { } d } p && _resolutionStackOperations._resolutionStack[n] is MouLuZhiOlDirectWindowFrame direct)
            return direct.ParentFrameId == p.Id && ValidMouLzDirect(p, d) && ExactMouLzDirectWindow(direct);
        return _resolutionStackOperations._resolutionStack[n - 1] is MouLuZhiOlDirectWindowFrame dw && _resolutionStackOperations._resolutionStack[n] is ProgramSkillFrame skill && ExactMouLzDirectWindow(dw) && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(skill, dw.Candidate) && skill.WindowContext == dw.Context;
    }

    internal bool MouLzObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || MouLzNativeEdge(n) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouLzFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouLzSuffix(int n) => _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterLuZhiMouOlOperations.MouLzObserverEdge(i), (i, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouLzRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLuZhiMouOlOperations.MouLzFirstChild(f, c), (n) => _characterLuZhiMouOlOperations.MouLzSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 32900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 32900)]
    internal bool MouLzDamage(long id) => MouLzRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 32900)]
    internal bool MouLzBeforeDamage(long id) => MouLzRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 32900)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 32900)]
    internal bool MouLzDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouLzRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 32900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 32900)]
    internal bool MouLzRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        if (!ValidMouLz(f))
            return false;
        if (MouLzLionChild(f, child) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == child.Id);
        return a >= 0 && b > a + 1 && MouLzFirstChild(f, _resolutionStackOperations._resolutionStack[a + 1]) && MouLzSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 32900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 32900)]
    internal bool KeepMouLzPaid(ProgramSkillFrame f) => f.MouLuZhiOl is { Paid: true } && ValidMouLz(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 32900)]
    internal bool ResumeMouLz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuZhiOl: not null } f || f.Id != id)
            return false;
        ContinueMouLz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 32900)]
    internal bool ReturnMouLzMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouLz(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 32900)]
    internal bool AdvanceMouLzChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouLzRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 32900)]
    internal void AssertMouLz(ProgramSkillFrame f)
    {
        if (f.MouLuZhiOl is not null && !ValidMouLz(f))
            throw new InvalidOperationException("Lu Zhi lost its original source, current native parent or paid child receipt.");
    }
}
