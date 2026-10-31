#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    internal bool ZhouQunMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ZhouQun!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == ZhouQunStage.JudgmentChildren && m.CardId == r.JudgmentCardId && m.From == CardLocation.Judgment(r.Peer) && m.To == CardLocation.Processing && m.Reason.Value == "skill-program.judgment.result")
            return m.Sequence <= r.Before && _coreProgramEventHistoryOperations.ProgramEventHistory<NingwuJudgmentReturnedEvent>().Count(e => e.FrameId == f.Id && e.CardId == m.CardId && e.JudgmentId == r.JudgmentId) == 1;
        if (m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (m.Reason == CardMoveReasons.Reshuffle && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile)
            return r.Stage is ZhouQunStage.RevealChildren or ZhouQunStage.ViewChildren;
        if (r.Stage == ZhouQunStage.RevealChildren)
            return r.Revealed.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == ChenshuoRevealReason;
        if (r.Stage == ZhouQunStage.GainChildren)
            return r.Revealed.Contains(m.CardId) && m.From == CardLocation.Processing && (m.To == CardLocation.Hand(f.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && m.To == CardLocation.DiscardPile) && m.Reason.Value == ChenshuoGainReason;
        if (r.Stage == ZhouQunStage.CleanupChildren)
            return r.Revealed.Contains(m.CardId) && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason.Value == ZhouQunCleanupReason;
        if (r.Stage == ZhouQunStage.ExchangeChildren)
            return m.Reason.Value == TianhouExchangeReason && (r.SelectedHand.Contains(m.CardId) && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DrawPile || r.SelectedTop.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat));
        return r.Stage == ZhouQunStage.JudgmentChildren && m.CardId == r.JudgmentCardId && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.JudgmentFinish;
    }

    internal bool ZhouQunFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhouQun is not { Paid: true } r)
            return false;
        if (child is JudgmentFrame judgment)
            return IsNingwuJudgment(judgment, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => ZhouQunMovementMatches(f, m));
        if (child is IdentityAppearanceFrame appearance)
            return r.Stage == ZhouQunStage.HpIssuing && r.Cursor < r.Targets.Count && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == f.Id && appearance.Receipt.OwnerSeat == r.Targets[r.Cursor] && appearance.Receipt.RequestedDeduction == 1 && appearance.Receipt.OriginalInstructionIndex == f.InstructionIndex;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == ZhouQunStage.HpChildren && r.Cursor < r.Targets.Count && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == r.Targets[r.Cursor] && hp.Change.SourceSeat is null && hp.Change.Amount == 1;
        if (child is DyingFrame dying)
            return r.Stage == ZhouQunStage.HpChildren && r.Cursor < r.Targets.Count && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Targets[r.Cursor] && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhouQunHpPaidEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Cursor && e.TargetSeat == dying.VictimSeat && e.RemainingHp == 0);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? ZhouQunObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhouQunOlOperations.ZhouQunFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 260)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 250)]
    internal bool IsZhouQunRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.ZhouQun is null)
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var recoveryIndex = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return rootIndex >= 0 && recoveryIndex > rootIndex + 1 && ZhouQunFirstChild(f, _resolutionStackOperations._resolutionStack[rootIndex + 1]) && Enumerable.Range(rootIndex + 2, recoveryIndex - rootIndex - 1).All(i => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[recoveryIndex - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27572)]
    internal bool AdvanceZhouQunChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhouQunObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
