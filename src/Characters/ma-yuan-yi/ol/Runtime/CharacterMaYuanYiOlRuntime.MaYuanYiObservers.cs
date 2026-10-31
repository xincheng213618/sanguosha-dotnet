#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaYuanYiOlRuntime
{
    internal bool MaYuanYiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MaYuanYi is not { } r)
            return false;
        if (child is JudgmentFrame judgment)
            return IsBinghuoJudgment(judgment, f);
        if (child is DeathFrame death)
            return r.Stage == MaYuanYiStage.MaximumChildren && r.MaximumBefore == 1 && death.ParentFrameId == f.Id && death.VictimSeat == f.OwnerSeat && death.ReturnKind == DeathReturnKind.ProgramSkill;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return (r.Stage == MaYuanYiStage.GrantChildren || HasPaidMoucuanSourceLoss(f)) && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == MaYuanYiStage.MaximumChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage is MaYuanYiStage.StoreChildren or MaYuanYiStage.OpponentDrawChildren or MaYuanYiStage.JudgmentChildren && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage == MaYuanYiStage.JudgmentChildren ? IsBinghuoHandoffMovement(r, m) || m.Sequence > r.Before && m.Sequence <= r.After && IsBinghuoCleanupMovement(r, m) : m.Sequence > r.Before && m.Sequence <= r.After && (m.Reason.Value == (r.Stage == MaYuanYiStage.StoreChildren ? JibingStoreReason : WangjingDrawReason) || m.Reason == CardMoveReasons.Reshuffle && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile)));
        return r.Stage == MaYuanYiStage.DamageChildren && f.AttackAttempt is { SourceSeat: var source, TargetSeat: var target, DamageAmount: 1, SourceLess: false, Nature: DamageNature.Thunder } && source == f.OwnerSeat && target == r.TargetSeat && f.AttackReturn is not null && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    }

    internal ProgramSkillFrame? MaYuanYiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterMaYuanYiOlOperations.MaYuanYiFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27370)]
    internal bool AdvanceMaYuanYiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaYuanYiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
