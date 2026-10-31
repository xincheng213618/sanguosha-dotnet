#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBaOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27390)]
    internal void AssertLiuBa(ProgramSkillFrame f)
    {
        if (f.LiuBa is not { } r)
            return;
        if (r.Source != LiuBaSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !Enum.IsDefined(r.Stage) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Recipient) || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Entities.Select(e => e.CardId).Distinct().Count() != r.Entities.Count || r.Coins.Distinct().Count() != r.Coins.Count || r.SelectedBottom.Distinct().Count() != r.SelectedBottom.Count || r.Coins.Count > r.ViewedBottom.Count || r.SelectedBottom.Count > r.Coins.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<LiuBaStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new LiuBaStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ParentId, r.ActualTurn, r.TurnOwner, r.PhaseInstance))
            throw new InvalidOperationException("Liu Ba lost his original native binding, participant or frozen physical selections.");
        if (!r.Paid)
        {
            if (f.PendingMovementContinuation is not null || r.Step != 0)
                throw new InvalidOperationException("Liu Ba cannot claim a movement before physical payment.");
            return;
        }

        if (r.Step <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<LiuBaMovementIssuedEvent>().Where(e => e.FrameId == f.Id && e.Step == r.Step).ToArray()is not [var movement] || movement != new LiuBaMovementIssuedEvent(f.Id, r.Step, r.Stage, r.Before, r.After, r.ActualCount))
            throw new InvalidOperationException("Liu Ba lost his once-paid movement boundary.");
        var records = _coreProgramEventHistoryOperations.ProgramEventHistory<LiuBaEntityMovedEvent>().Where(e => e.FrameId == f.Id && e.Step == r.Step).ToArray();
        if (records.Length != (r.Stage == LiuBaStage.DrawChildren ? 0 : r.ActualCount) || records.Any(e => e.Sequence <= r.Before || e.Sequence > r.After || e.BatchId != r.BatchId || !r.Entities.Any(c => c.CardId == e.CardId && c.Kind == e.Kind && c.From == e.From) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To) != 1))
            throw new InvalidOperationException("Liu Ba lost an exact original physical payment invoice.");
        if (r.Op == SkillProgramEffectOp.IssueTongduHandGift && _coreProgramEventHistoryOperations.ProgramEventHistory<TongduGiftIssuedEvent>().Count(e => e.FrameId == f.Id && ValidTongduGift(e)) != 1)
            throw new InvalidOperationException("Tongdu lost the exact gifted Hand entity.");
        if (r.Op == SkillProgramEffectOp.ReturnTongduOriginalCardAtPlayEnd && (r.DebtId is not { } debt || _coreProgramEventHistoryOperations.ProgramEventHistory<TongduDebtConsumedEvent>().Count(e => e.DebtId == debt && e.FrameId == f.Id && e.ParentId == r.ParentId && e.ActualTurn == r.ActualTurn && e.PhaseInstance == r.PhaseInstance) != 1))
            throw new InvalidOperationException("Tongdu cannot repeat or transfer its issued original-card debt.");
        if (f.PendingMovementContinuation is { } p && !IsLiuBaMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            throw new InvalidOperationException("Liu Ba lost the typed waiting participant.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 340)]
    internal bool IsLiuBaMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.LiuBa is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == r.Recipient && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool LiuBaMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.LiuBa!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == LiuBaStage.DrawChildren)
            return m.Reason == CardMoveReasons.RecastDraw && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Recipient) || m.Reason == CardMoveReasons.Reshuffle && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<LiuBaEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.Step && e.Sequence == m.Sequence && e.CardId == m.CardId && e.From == m.From && e.To == m.To))
            return true;
        return r.Grain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(r.Recipient) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }

    internal bool LiuBaLion(ProgramSkillFrame f, int seat) => f.LiuBa is { Paid: true } r && r.Stage != LiuBaStage.DrawChildren && r.Entities.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(seat)) && _coreProgramEventHistoryOperations.ProgramEventHistory<LiuBaEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.Step && e.Kind == CardKind.SilverLion && e.From == CardLocation.Equipment(seat));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 140)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 130)]
    internal bool IsLiuBaRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.LiuBa is { Paid: true } && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && LiuBaLion(f, recovery.Attempt.TargetSeat) && recovery.Attempt.Completion.MoveReason?.Value == (f.LiuBa.Stage == LiuBaStage.RecastChildren ? CardMoveReasons.RecastDiscard.Value : f.LiuBa.Stage == LiuBaStage.ReturnChildren ? TongduReturnReason : LiuBaExchangeReason);
    internal bool LiuBaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LiuBa is not { Paid: true } r)
            return false;
        AssertLiuBa(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => LiuBaMovementMatches(f, m));
        if (child is RecoveryReplacementFrame recovery)
            return IsLiuBaRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && hp.Change.Amount > 0 && LiuBaLion(f, hp.Change.TargetSeat);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return false;
    }

    internal ProgramSkillFrame? LiuBaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLiuBaOlOperations.LiuBaFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 850)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 960)]
    internal bool HasLiuBaDamageObserver(long id) => LiuBaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 990)]
    internal bool HasLiuBaBeforeDamageObserver(long id) => HasLiuBaDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 820)]
    internal bool IsLiuBaDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && LiuBaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27390)]
    internal bool AdvanceLiuBaChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuBaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
