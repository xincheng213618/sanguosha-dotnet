#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuJinDingOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26820)]
    internal void AssertHuJinDing(ProgramSkillFrame f)
    {
        if (f.Qingyuan is not { } r)
            return;
        if (r.Source != HuJinDingSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.InstructionIndex != 1 || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || !HuJinDingComposition.Handles(r.Op) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !Enum.IsDefined(r.Stage) || f.WindowContext is not { } context || context.ParentFrameId != r.ParentId || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start.Source != r.Source || start.GameplayHash != r.GameplayHash || start.Op != r.Op || start.ParentId != r.ParentId || start.ActualTurn != r.ActualTurn || start.BatchId != r.BatchId || start.MovementIndex != r.MovementIndex)
            throw new InvalidOperationException("Qingyuan lost its exact original source, instruction or single native candidate invoice.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (index < 1)
            throw new InvalidOperationException("Qingyuan requires its retained exact native parent.");
        if (r.Op == SkillProgramEffectOp.SelectQingyuanOpeningPartner)
        {
            if (r.Stage != QingyuanStage.ChoosingPartner || r.Damage is not null || context.Window != SkillProgramTriggerWindow.GameStarting || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != r.ParentId || parent.Window != context.Window || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
                throw new InvalidOperationException("Qingyuan lost its actual mandatory opening cursor.");
        }
        else if (r.Op == SkillProgramEffectOp.SelectQingyuanFirstDamagePartner)
        {
            if (r.Stage != QingyuanStage.ChoosingPartner || r.Damage is not { } d || d.OwnerSeat != f.OwnerSeat || d.Amount < 1 || context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.DamageFrameId != d.DamageFrameId || context.Amount != d.Amount || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanFirstDamageEvent>().Count(e => e == d) != 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame parent || parent.Id != r.ParentId || parent.ParentFrameId != d.DamageFrameId || parent.TargetSeat != f.OwnerSeat || parent.TriggerWindow != context.Window || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].ToProgramCandidate()) || !_resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(p => p.Id == d.DamageFrameId && p.ParentFrameId == d.DamageParentId && p.TargetSeat == f.OwnerSeat && p.Amount == d.Amount))
                throw new InvalidOperationException("Qingyuan lost its first real damage instance or would select a second partner once per damage point.");
        }
        else
        {
            if (r.Damage is not null || r.Stage != QingyuanStage.TakeChildren || r.BatchId != r.ParentId || r.MovementIndex != context.MovementIndex || QingyuanGainQualification(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, r.MovementIndex), context)is null || r.HolderCandidates is not { Count: > 0 } holders || holders.Distinct().Count() != holders.Count || holders.Count > 2 || r.HolderRandomIndex < 0 || r.HolderRandomIndex >= holders.Count || r.HolderSeat != holders[r.HolderRandomIndex] || holders.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == f.OwnerSeat || !QingyuanPartners(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Any(p => p.PartnerSeat == s)) || r.HandCandidates is not { Count: > 0 } hand || hand.Distinct().Count() != hand.Count || r.CardRandomIndex < 0 || r.CardRandomIndex >= hand.Count || r.CardId != hand[r.CardRandomIndex] || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat || r.Before < 0 || r.After <= r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanTakePaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.HolderSeat != r.HolderSeat || paid.CardId != r.CardId || paid.Before != r.Before || paid.After != r.After || paid.ActualTurn != r.ActualTurn || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).ToArray()is not [var move] || move.CardId != r.CardId || move.From != CardLocation.Hand(r.HolderSeat!.Value) || move.To != CardLocation.Hand(f.OwnerSeat) || move.Reason.Value != QingyuanTakeReason)
                throw new InvalidOperationException("Qingyuan lost its frozen genuine gain qualification, private random indexes, exact hand material or once-paid native movement.");
            return;
        }

        if (r.BatchId is not null || r.MovementIndex != -1 || r.HolderCandidates is not null || r.HandCandidates is not null || r.HolderSeat is not null || r.CardId is not null || r.Before != 0 || r.After != 0 || f.PendingMovementContinuation is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanTakePaidEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Qingyuan partner selection cannot contain a paid take or private hand material.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26820)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26820)]
    internal bool ContinuePaidHuJinDing(ProgramSkillFrame f)
    {
        if (f.Qingyuan is not { Stage: QingyuanStage.TakeChildren })
            return false;
        AssertHuJinDing(f);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 190)]
    internal bool IsHuJinDingMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.Qingyuan is not { Stage: QingyuanStage.TakeChildren } r || effect?.Op != r.Op || pending != f.PendingMovementContinuation)
            return false;
        AssertHuJinDing(f);
        return true;
    }

    internal bool HuJinDingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Qingyuan is not { Stage: QingyuanStage.TakeChildren } r)
            return false;
        AssertHuJinDing(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.CardId && m.From == CardLocation.Hand(r.HolderSeat!.Value) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == QingyuanTakeReason;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 840)]
    internal bool IsHuJinDingRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => false;
    internal ProgramSkillFrame? HuJinDingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuJinDingOlOperations.HuJinDingFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1490)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1600)]
    internal bool HasHuJinDingDamageObserver(long id) => HuJinDingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1620)]
    internal bool HasHuJinDingBeforeDamageObserver(long id) => HuJinDingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1460)]
    internal bool IsHuJinDingDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && HuJinDingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26820)]
    internal bool AdvanceHuJinDingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuJinDingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
