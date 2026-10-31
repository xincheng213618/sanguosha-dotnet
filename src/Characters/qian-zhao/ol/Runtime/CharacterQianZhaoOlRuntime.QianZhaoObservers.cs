#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26620)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26620)]
    internal bool ContinuePaidQianZhao(ProgramSkillFrame f)
    {
        if (f.Weifu is null && f.WeifuTargets is null && f.Kuansai is null && f.InstructionIndex is 0 or 1 && f.TriggerId is { } binding && f.ActivationId == binding && f.WindowContext is { CardUse: { } card } context && _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)?.WeifuUse is { Offered: false } && HasIssuedWeifuCandidate(new(f.OwnerSeat, f.SkillId, binding, f.SkillInstanceId, f.GameplayHash, 0), context))
            return true;
        if (f.Weifu is null && f.WeifuTargets is null && f.Kuansai is not { Stage: KuansaiStage.Children })
            return false;
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 150)]
    internal bool IsQianZhaoMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        var matches = p is { BeforeCount: 0, CoverageResultBind: null } && p.SubjectSeat == f.OwnerSeat && (e?.Op == SkillProgramEffectOp.ResolveWeifuJudgment && f.Weifu is { Stage: not WeifuStage.Judging } || e?.Op == SkillProgramEffectOp.ResolveKuansaiTargetChoice && f.Kuansai is { Stage: KuansaiStage.Children });
        if (matches)
            ;
        return matches;
    }

    internal bool QianZhaoSilverLion(ProgramSkillFrame f, int target, CardMoveReason? reason) => f.Weifu is { Stage: WeifuStage.PaymentChildren, CostFrom.Zone: CardZoneKind.Equipment } w && target == f.OwnerSeat && reason?.Value == WeifuDiscardReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == w.CostCardId && m.CardKind == CardKind.SilverLion && m.From == w.CostFrom && m.Reason.Value == WeifuDiscardReason) || f.Kuansai is { Stage: KuansaiStage.Children, Gift: true, From: { Zone: CardZoneKind.Equipment } } k && target == k.ChooserSeat && reason?.Value == KuansaiGiftReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > k.Before && m.Sequence <= k.After && m.CardId == k.CardId && m.CardKind == CardKind.SilverLion && m.Reason.Value == KuansaiGiftReason);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 680)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 620)]
    internal bool IsQianZhaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => (f.Weifu is not null || f.Kuansai is not null) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && (f.Kuansai is { Stage: KuansaiStage.Children, Gift: false } && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == f.InstructionIndex || recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && QianZhaoSilverLion(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason));
    internal bool QianZhaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Weifu is null && f.Kuansai is null)
            return false;
        if (child is JudgmentFrame judgment)
            return f.Weifu is { Stage: WeifuStage.Judging } && IsWeifuJudgment(judgment, f);
        if (f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (!global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            var before = f.Weifu?.Before ?? f.Kuansai!.Before;
            var after = f.Weifu?.After ?? f.Kuansai!.After;
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && (f.Weifu is not null ? m.Reason.Value is WeifuDiscardReason or WeifuDrawReason or "skill-program.judgment.result" || m.Reason == CardMoveReasons.JudgmentFinish || m.Reason == CardMoveReasons.Reshuffle : m.Reason.Value == KuansaiGiftReason && m.CardId == f.Kuansai!.CardId && m.From == f.Kuansai.From && m.To == CardLocation.Hand(f.OwnerSeat)));
        }

        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is RecoveryReplacementFrame recovery)
            return IsQianZhaoRecoveryParent(f, recovery);
        if (child is not HpChangedTriggerWindowFrame hp || hp.Change.ParentFrameId != f.Id || hp.ResumeFrameId != f.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.Amount != 1)
            return false;
        return f.Kuansai is { Stage: KuansaiStage.Children, Gift: false } && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat || hp.Change.SourceSeat == hp.Change.TargetSeat && QianZhaoSilverLion(f, hp.Change.TargetSeat, new(f.Weifu is not null ? WeifuDiscardReason : KuansaiGiftReason));
    }

    internal ProgramSkillFrame? QianZhaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterQianZhaoOlOperations.QianZhaoFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 480)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 590)]
    internal bool HasQianZhaoDamageObserver(long id) => QianZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 620)]
    internal bool HasQianZhaoBeforeDamageObserver(long id) => QianZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26620)]
    internal bool AdvanceQianZhaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QianZhaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
