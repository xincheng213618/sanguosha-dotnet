#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuYongOlRuntime
{
    internal bool MatchesOrdinaryBingyiRoot(ProgramSkillFrame f)
    {
        if (f.OrdinaryBingyi is not { Paid: true } r || f.SkillId != "ol:bingyi" || f.TriggerId != "ordinary-discard-reveal-share" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c || r.Source != OrdinaryGuYongSource(f) || r.GameplayHash != f.GameplayHash || r.WindowId != c.ParentFrameId || r.BatchId != c.MovementBatch?.Id || r.Phase != c.MovementBatch?.DiscardRecoveryPhase || r.OccurrenceIndex != c.OccurrenceIndex || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveOrdinaryGuYongBingyi || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || !OrdinaryBingyiMovementRoot(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiShownEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var shown] && shown == new OrdinaryBingyiShownEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.WindowId, r.BatchId, r.OccurrenceIndex, r.ShownCards.Count, r.UniformColor, r.Color) && _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiShownEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.Phase == r.Phase) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27598)]
    internal void AssertOrdinaryBingyi(ProgramSkillFrame f)
    {
        if (f.OrdinaryBingyi is not { } r)
            return;
        if (!MatchesOrdinaryBingyiRoot(f) || !Enum.IsDefined(r.Stage) || r.ShownCards is not System.Collections.IList { IsReadOnly: true } || r.OtherRecipients is not System.Collections.IList { IsReadOnly: true } || r.Recipients is not System.Collections.IList { IsReadOnly: true } || r.ShownCards.Count < 1 || r.ShownCards.Select(c => c.CardId).Distinct().Count() != r.ShownCards.Count || r.OtherRecipients.Count > r.ShownCards.Count || r.OtherRecipients.Distinct().Count() != r.OtherRecipients.Count || r.OtherRecipients.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == f.OwnerSeat) || r.Recipients.Distinct().Count() != r.Recipients.Count || r.Cursor < 0 || r.Cursor > r.Recipients.Count)
            throw new InvalidOperationException("Ordinary Bingyi lost its immutable phase payment, original movement window or frozen hand.");
        var colors = r.ShownCards.Select(c => c.EffectiveSuit is Suit.Heart or Suit.Diamond ? (bool? )true : c.EffectiveSuit is Suit.Club or Suit.Spade ? false : null).Distinct().ToArray();
        var entities = _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiShownEntityEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.UniformColor != (colors.Length == 1) || r.Color != (colors.Length == 1 ? colors[0] : null) || entities.Length != r.ShownCards.Count || !entities.Select((e, i) => e == new OrdinaryBingyiShownEntityEvent(f.Id, i, r.ShownCards[i].CardId, r.ShownCards[i].PrintedKind, r.ShownCards[i].EffectiveSuit)).All(v => v) || f.CardSetBindings.SingleOrDefault(b => b.Name == OrdinaryBingyiShownBind)is not { Visibility: SkillProgramCardSetVisibility.Public } bind || !bind.CardIds.SequenceEqual(r.ShownCards.Select(c => c.CardId)) || !bind.SourceLocations.All(l => l == CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("Ordinary Bingyi must retain exactly the hand and effective colors actually shown at payment.");
        var committed = _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiRecipientsCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var recipients = _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiRecipientEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (!r.RecipientsCommitted)
        {
            if (r.Stage != OrdinaryBingyiStage.SelectingRecipients || r.Recipients.Count != 0 || r.Cursor != 0 || r.DrawOutcome is not null || f.NativeDraw is not null || f.PendingMovementContinuation is not null || committed.Length != 0 || recipients.Length != 0)
                throw new InvalidOperationException("Ordinary Bingyi cannot draw before committing its one frozen recipient queue.");
            return;
        }

        if (!r.UniformColor || r.Stage == OrdinaryBingyiStage.SelectingRecipients || !r.Recipients.SequenceEqual(new[] { f.OwnerSeat }.Concat(r.OtherRecipients.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(r.Phase.ActualTurnOwnerSeat, s)))) || committed is not [var issued] || issued.Count != r.Recipients.Count || recipients.Length != r.Recipients.Count || !recipients.Select((e, i) => e == new OrdinaryBingyiRecipientEvent(f.Id, i, r.Recipients[i])).All(v => v))
            throw new InvalidOperationException("Ordinary Bingyi changed its paid recipient count, order or identities.");
        var returned = _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var skipped = _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiRecipientSkippedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (returned.Any(e => e.DrawIndex < 0 || e.DrawIndex > r.Cursor || e.DrawIndex >= r.Recipients.Count || e.RecipientSeat != r.Recipients[e.DrawIndex]) || skipped.Any(e => e.DrawIndex < 0 || e.DrawIndex >= r.Cursor || e.RecipientSeat != r.Recipients[e.DrawIndex]) || Enumerable.Range(0, r.Cursor).Any(i => returned.Count(e => e.DrawIndex == i) + skipped.Count(e => e.DrawIndex == i) != 1))
            throw new InvalidOperationException("Ordinary Bingyi repeated or skipped a recipient's already-paid native draw.");
        if (r.Stage == OrdinaryBingyiStage.DrawChildren)
        {
            if (r.Cursor >= r.Recipients.Count || r.DrawOutcome is not { } o || o.RecipientSeat != r.Recipients[r.Cursor] || o.RequestedCount != 1 || o.SequenceBefore < 0 || o.SequenceAfter < o.SequenceBefore || o.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || returned.Where(e => e.DrawIndex == r.Cursor).ToArray()is not [var actual] || actual != new OrdinaryBingyiDrawReturnedEvent(f.Id, r.Cursor, o.RecipientSeat, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId) || f.NativeDraw is not null || f.PendingMovementContinuation is not { } pending || !IsOrdinaryGuYongMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
                throw new InvalidOperationException("Ordinary Bingyi lost its native outcome or exact movement-child return.");
        }
        else if (r.DrawOutcome is not null || f.PendingMovementContinuation is not null || returned.Any(e => e.DrawIndex == r.Cursor))
            throw new InvalidOperationException("Ordinary Bingyi retained a finished draw as a new request.");
    }

    internal bool CanContinueIssuedOrdinaryGuYong(ProgramSkillFrame f)
    {
        if (f.OrdinaryBingyi is not { Paid: true })
            return false;
        AssertOrdinaryBingyi(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27598)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27598)]
    internal bool PermitPaidOrdinaryBingyi(ProgramSkillFrame f) => CanContinueIssuedOrdinaryGuYong(f);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 150)]
    internal bool IsOrdinaryGuYongMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.OrdinaryBingyi is { Paid: true, Stage: OrdinaryBingyiStage.DrawChildren, DrawOutcome: { } o } && effect?.Op == SkillProgramEffectOp.ResolveOrdinaryGuYongBingyi && p == f.PendingMovementContinuation && p.SubjectSeat == o.RecipientSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool IsOrdinaryBingyiDrawMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.OrdinaryBingyi is not { Paid: true, Stage: OrdinaryBingyiStage.DrawChildren, DrawOutcome: { } o } || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= o.SequenceBefore || m.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(o.RecipientSeat) && m.Reason.Value == OrdinaryBingyiDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(o.RecipientSeat);
    }

    internal bool OrdinaryBingyiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.OrdinaryBingyi is not { Paid: true } r)
            return false;
        AssertOrdinaryBingyi(f);
        if (r.Stage == OrdinaryBingyiStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsOrdinaryBingyiDrawMovement(f, m));
    }

    internal bool OrdinaryBingyiObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? OrdinaryBingyiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterGuYongOlOperations.OrdinaryBingyiFirstChild(f, child), (i) => _characterGuYongOlOperations.OrdinaryBingyiObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1080)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1190)]
    internal bool HasOrdinaryGuYongDamageObserver(long id) => OrdinaryBingyiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1220)]
    internal bool HasOrdinaryGuYongBeforeDamageObserver(long id) => OrdinaryBingyiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1050)]
    internal bool IsOrdinaryGuYongDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && OrdinaryBingyiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 310)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 300)]
    internal bool IsOrdinaryGuYongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.OrdinaryBingyi is not { Paid: true } || recovery.ParentFrameId == f.Id)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var j = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return i >= 0 && j > i + 1 && OrdinaryBingyiFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && OrdinaryBingyiObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[j - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27598)]
    internal bool AdvanceOrdinaryBingyiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && OrdinaryBingyiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
