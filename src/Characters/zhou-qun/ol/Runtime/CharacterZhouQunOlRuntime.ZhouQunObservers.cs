#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27572)]
    internal void AssertZhouQun(ProgramSkillFrame f)
    {
        if (f.ZhouQun is not { } r)
            return;
        if (!r.Paid || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || r.Source != ZhouQunSource(f) || r.GameplayHash != f.GameplayHash || r.WindowId != context.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || !ZhouQunComposition.Handles(r.Op) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !ZhouQunParent(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context) || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhouQunStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new ZhouQunStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.TurnOwner))
            throw new InvalidOperationException("Zhou Qun lost his exact source, native candidate, original instruction or once-started receipt.");
        foreach (var set in new[]
        {
            r.Revealed,
            r.Viewed,
            r.SelectedHand,
            r.SelectedTop,
            r.Targets
        }

        )
            if (set is not System.Collections.IList { IsReadOnly: true } || set.Distinct().Count() != set.Count)
                throw new InvalidOperationException("Weather collections must remain deeply frozen exact sets.");
        if (r.Revealed.Count > 3 || r.Viewed.Count > 3 || r.SelectedHand.Count > r.Viewed.Count || r.SelectedTop.Count > r.SelectedHand.Count || r.Cursor < 0 || r.Cursor > r.Targets.Count || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Weather exceeded its physical collection or native movement boundary.");
        if (r.ShownCards is not System.Collections.IList { IsReadOnly: true } || r.ShownCards.Count > 3 || r.ShownCards.Count < r.Revealed.Count || r.ShownCards.Count > r.Revealed.Count + 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<ChenshuoHandShownEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Card).SequenceEqual(r.ShownCards) || r.ShownCards.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s.OwnerSeat) || s.NameLength != ZhouQunNameLength(s.Kind)) || r.ShownCards.Count > 0 && r.Shown != r.ShownCards[^1])
            throw new InvalidOperationException("Chenshuo must repeat the whole real Hand-show and top-show pair, preserving each round's frozen identity.");
        var top = _coreProgramEventHistoryOperations.ProgramEventHistory<ChenshuoTopRevealedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (!top.Select(e => e.CardId).SequenceEqual(r.Revealed) || top.Where((e, i) => e.Index != i).Any() || top.Any(e => e.Index >= r.ShownCards.Count || e.NameLength != ZhouQunNameLength(e.Kind) || e.Matched != (global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(e.Kind) == global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(r.ShownCards[e.Index].Kind) || e.Suit == r.ShownCards[e.Index].Suit || e.Rank == r.ShownCards[e.Index].Rank || e.NameLength == r.ShownCards[e.Index].NameLength)))
            throw new InvalidOperationException("Chenshuo cannot substitute its revealed cards or matching features.");
        if (r.Targets.Count > 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhouQunHpTargetEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.TargetSeat).SequenceEqual(r.Targets))
            throw new InvalidOperationException("Weather participants must be frozen before their first native child.");
        if (r.Op == SkillProgramEffectOp.ResolveNingwuSingletonSlashJudgment && (r.Targets.Count != 1 || r.UseId <= 0 || r.ActionId <= 0 || r.JudgmentId <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<NingwuJudgmentIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued != new NingwuJudgmentIssuedEvent(f.Id, r.JudgmentId, r.UseId, r.ActionId, r.Peer, r.Targets.Single(), r.SlashRank)))
            throw new InvalidOperationException("Ningwu lost its exact original Slash and native judgment.");
        if (f.PendingMovementContinuation is { } pending && !IsZhouQunMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Weather has an unrelated native movement continuation.");
        if (r.Stage == ZhouQunStage.HpChildren && (r.Cursor >= r.Targets.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhouQunHpPaidEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Cursor && e.TargetSeat == r.Targets[r.Cursor]) != 1))
            throw new InvalidOperationException("Weather HP children require the exact actual payment once.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 510)]
    internal bool IsZhouQunMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.ZhouQun is { Paid: true } r && effect?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is ZhouQunStage.RevealChildren or ZhouQunStage.GainChildren or ZhouQunStage.ViewChildren or ZhouQunStage.ExchangeChildren or ZhouQunStage.CleanupChildren or ZhouQunStage.JudgmentChildren;
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
        AssertZhouQun(f);
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
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1030)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1140)]
    internal bool HasZhouQunDamageObserver(long id) => ZhouQunObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1170)]
    internal bool HasZhouQunBeforeDamageObserver(long id) => ZhouQunObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1000)]
    internal bool IsZhouQunDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ZhouQunObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
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
