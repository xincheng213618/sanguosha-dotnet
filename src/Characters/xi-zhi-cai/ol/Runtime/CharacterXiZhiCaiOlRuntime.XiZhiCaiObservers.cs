#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiZhiCaiOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 140)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 140)]
    internal bool CanContinueIssuedXiZhiCai(ProgramSkillFrame f) => ValidXianfuEcho(f) || ValidChouce(f);
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 150)]
    internal bool IsXiZhiCaiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => p.BeforeCount == 0 && p.CoverageResultBind is null && (e?.Op == SkillProgramEffectOp.SettleXianfuNativeEcho && f.XianfuPending is { Receipt.Kind: XianfuEchoKind.Recovery } && p.SubjectSeat == f.OwnerSeat && ValidXianfuEcho(f) || e?.Op == SkillProgramEffectOp.ResolveChouceFrozenJudgment && f.Chouce is { Stage: ChouceStage.NativeChildren, TargetSeat: { } target } && p.SubjectSeat == target && ValidChouce(f));
    internal bool XiZhiCaiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (ValidXianfuEcho(root) && root.XianfuPending is { } echo)
        {
            if (echo.Receipt.Kind == XianfuEchoKind.Damage && root.AttackAttempt is { SourceLess: true, SourceSeat: var source, TargetSeat: var target } attack && source == root.OwnerSeat && target == root.OwnerSeat)
            {
                if (child is DamageFrame d)
                    return d.ParentFrameId == root.Id && d.SourceSeat == source && d.TargetSeat == target && d.Amount == attack.DamageAmount;
                if (child is BeforeDamageProgramWindowFrame before)
                    return (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == root.Id && before.TargetSeat == target && before.SourceSeat == source && before.Amount == attack.DamageAmount;
            }

            if (echo.Receipt.Kind == XianfuEchoKind.Recovery)
            {
                var amount = echo.NativeAmount;
                if (child is RecoveryReplacementFrame recovery)
                    return recovery.ParentFrameId == root.Id && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Amount == amount && recovery.Attempt.HpBefore == echo.HpBefore && recovery.Attempt.Completion is { Producer: RecoveryAttemptProducer.Program, InstructionIndex: var instruction } && instruction == echo.InstructionIndex;
                if (child is HpChangedTriggerWindowFrame hp)
                    return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.HpBefore == echo.HpBefore && hp.Change.Amount == amount;
            }
        }

        if (!ValidChouce(root) || root.Chouce is not { } p)
            return false;
        if (child is JudgmentFrame j)
            return j.Id == p.Receipt.JudgmentFrameId && j.ParentFrameId == root.Id && j.TargetSeat == root.OwnerSeat && j.SourceSeat == root.OwnerSeat && j.Reason == p.Receipt.Reason && j.Continuation == JudgmentContinuationKind.ProgramSkill && j.ProgramResultBind == p.Receipt.ResultBind && j.ProgramResultVisibility == SkillProgramCardSetVisibility.Public;
        if (p.Stage != ChouceStage.NativeChildren)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == (ChouceIsRed(root) ? "program.chouce.red" : "program.chouce.black"));
        if (p.CardId is { } lion && _coreAdvancedLifecycleOperations.GetAdvancedCard(lion).Kind == CardKind.SilverLion && p.CardLocation is { Zone: CardZoneKind.Equipment, OwnerSeat: { } seat })
        {
            if (child is RecoveryReplacementFrame recovery)
                return recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion is { Producer: RecoveryAttemptProducer.SilverLion, MoveReason: { } reason } && reason.Value == "program.chouce.black";
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.TargetSeat == seat && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1;
        }

        return false;
    }

    internal bool XiZhiCaiObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        if (parent is JudgmentFrame j && child is ProgramJudgmentTriggerWindowFrame w)
            return w.ParentFrameId == j.Id && w.Judgment.JudgmentFrameId == j.Id;
        if (parent is ProgramJudgmentTriggerWindowFrame { ZhenyiBeforeResult: true } before && child is ProgramSkillFrame zhenyi)
            return _coreZhenyiJudgmentWindowsOperations.IsExactZhenyiBeforeResultProgramChild(before, zhenyi);
        if (parent is ProgramJudgmentTriggerWindowFrame window && child is ProgramSkillFrame skill)
            return window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && skill.WindowContext is { Window: SkillProgramTriggerWindow.JudgmentFinalized, Judgment: { } final } context && context.ParentFrameId == window.Id && final == window.Judgment && window.Candidates[window.CandidateIndex] is { } c && c.OwnerSeat == skill.OwnerSeat && c.SkillId == skill.SkillId && c.TriggerId == skill.TriggerId && c.SkillInstanceId == skill.SkillInstanceId && c.GameplayHash == skill.GameplayHash;
        return _coreObserverSubtreesOperations.IsMovementObserverEdge(index);
    }

    internal ProgramSkillFrame? XiZhiCaiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterXiZhiCaiOlOperations.XiZhiCaiFirstChild(r, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterXiZhiCaiOlOperations.XiZhiCaiObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 120)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 230)]
    internal bool HasXiZhiCaiDamageObserver(long id) => XiZhiCaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 320)]
    internal bool HasXiZhiCaiBeforeDamageObserver(long id) => XiZhiCaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 210)]
    internal bool TryAdvanceXiZhiCaiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XiZhiCaiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
