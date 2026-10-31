#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMobileShiRuntime
{
    internal bool ValidShiXq(ProgramSkillFrame f) => f.ShiXiaoQiao is { } r && f.InstructionIndex == 1 && r.Source == ShiXqSource(f) && r.Hash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == r.Hash && r.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && r.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && r.ParentId == (f.WindowContext?.ParentFrameId ?? f.ParentFrameId ?? 0) && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == r.Operation && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXqStartedEvent>().Count(e => e == new ShiXqStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.Round, r.ParentId, r.Operation)) == 1;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 50400)]
    internal void RequireShiXq(ProgramSkillFrame f)
    {
        if (f.ShiXiaoQiao is not { } r)
            return;
        if (!ValidShiXq(f) || r.RevokeIds.Count != r.RevokeIds.Distinct(StringComparer.Ordinal).Count() || r.RevokeIds.Any(id => !ShiXqGrantLedger(f.OwnerSeat).Any(e => e.GrantId == id)) || r.Operation == SkillProgramEffectOp.ResolveMobileShiXiaoQiaoHeyun && (f.SkillId != ShiXqHeyun || f.TriggerId is not null || f.ActivationId != "lose-for-draw" || f.WindowContext is not null || r.Paid && (r.SelectedSkill is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXqHeyunPaidEvent>().Count(e => e == new ShiXqHeyunPaidEvent(f.Id, r.Source, r.Target, r.SelectedSkill)) != 1)) || r.Operation == SkillProgramEffectOp.ResolveMobileShiXiaoQiaoYinhui && (f.SkillId != ShiXqYinhui || f.TriggerId != "copy-current-skill" || f.WindowContext?.Window != SkillProgramTriggerWindow.RoundStarting || r.GrantId is not null && (r.GrantSource != ShiXqGrantSource(f.OwnerSeat, f.Id) || r.GrantId != r.GrantSource + ":" + r.SelectedSkill || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXqCopiedEvent>().Count(e => e.FrameId == f.Id && e.GrantId == r.GrantId && e.GrantSourceId == r.GrantSource && e.SkillId == r.SelectedSkill && e.SkillInstanceId == r.GrantedInstance) != 1)) || r.Draw is { } draw && (r.Operation != SkillProgramEffectOp.ResolveMobileShiXiaoQiaoHeyun || draw.RecipientSeat != r.Target || draw.RequestedCount != 2 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXqDrawPaidEvent>().Count(e => e.FrameId == f.Id && e.DrawId == draw.AttemptFrameId && e.Recipient == r.Target && e.ActualCount == draw.ActualDrawIds.Count && e.Replaced == (draw.Replacement is not null) && e.Before == draw.SequenceBefore && e.After == draw.SequenceAfter) != 1))
            throw new InvalidOperationException("Shi Xiao Qiao lost its original instruction/source, exact issued acquisition, skill payment or once-paid native draw.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && f.Step == ResolutionFrameStep.AwaitingResponse && r.Stage is ShiXqStage.Target or ShiXqStage.LoseSkill or ShiXqStage.CopyTarget or ShiXqStage.CopySkill && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != f.OwnerSeat || !p.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, ShiXqChoices(f))))
            throw new InvalidOperationException("Shi Xiao Qiao lost its exact current public-skill choices.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 50400)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 50400)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 50400)]
    internal bool ShiXqMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ShiXiaoQiao is { Paid: true } r && ValidShiXq(f) && e?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is ShiXqStage.LossChildren or ShiXqStage.RevokeChildren or ShiXqStage.GrantChildren or ShiXqStage.DrawChildren;
    internal bool ShiXqFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidShiXq(f) || f.ShiXiaoQiao is not { Paid: true } r)
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == ShiXqStage.Drawing && draw.Context.RecipientSeat == r.Target && draw.Context.RequestedCount == 2 && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && ShiXqMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ShiXqChildEdge(int i) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiXqFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiXqChildSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterXiaoQiaoMobileShiOperations.ShiXqChildEdge(n), (n, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, dying));
    internal ProgramSkillFrame? ShiXqObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterXiaoQiaoMobileShiOperations.ShiXqFirstChild(f, child), (i) => _characterXiaoQiaoMobileShiOperations.ShiXqChildSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 50400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 50400)]
    internal bool ShiXqDamageObserver(long id) => ShiXqObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 50400)]
    internal bool ShiXqBeforeDamageObserver(long id) => ShiXqObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 50400)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 50400)]
    internal bool ShiXqDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ShiXqObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 50400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 50400)]
    internal bool ShiXqRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiXqFirstChild(f, recovery))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return i >= 0 && child > i + 1 && ShiXqFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && ShiXqChildSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 50400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 50400)]
    internal bool KeepShiXqPaid(ProgramSkillFrame f) => f.ShiXiaoQiao is { Paid: true } && ValidShiXq(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 50400)]
    internal bool AdvanceShiXqChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiXqObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
