#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeYanAnniversaryRuntime
{
    internal bool HasAnniversaryHeYanPrompt()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHeYan: { } r, Step: ResolutionFrameStep.AwaitingResponse } f || r.Stage is not (AnniversaryHeYanStage.SourceChoice or AnniversaryHeYanStage.DiscardChoice or AnniversaryHeYanStage.SealedChoice or AnniversaryHeYanStage.SuitChoice) || p.PlayerSeat != HeYanChooser(r, f.OwnerSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, HeYanChoices(f)))
            return false;
        RequireHeYan(f);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 44200)]
    internal void RequireHeYan(ProgramSkillFrame f)
    {
        if (f.AnniversaryHeYan is not { } r)
            return;
        if (f.InstructionIndex != 1 || r.Source != HeYanSource(f) || r.Hash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.Operation != _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op || r.ParentId != f.WindowContext?.ParentFrameId || r.DamageId != f.WindowContext?.DamageFrameId || !Enum.IsDefined(r.Stage) || !Enum.IsDefined(r.Following) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.SourceSeat) || r.ChooserIndex < 0 || r.ChooserIndex > r.Plans.Count || r.DrawIndex < 0 || r.DrawIndex > r.Recipients.Count || r.RequiredDiscard < 0 || r.Selected.Distinct().Count() != r.Selected.Count || r.Plans.Select(p => p.Seat).Distinct().Count() != r.Plans.Count || r.Plans.Any(p => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.Seat) || p.Cards.Select(c => c.Id).Distinct().Count() != p.Cards.Count || p.SelectedId is { } selected && !p.Cards.Any(c => c.Id == selected)) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHeYanStartedEvent>().Count(e => e == new AnniversaryHeYanStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.ActualTurn, r.TurnOwner, r.ParentId, r.DamageId, r.SourceSeat, r.Plans.Count)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHeYanCompletedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("He Yan lost his accepted source, actual-turn cursor or frozen participants.");
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanYachai && (f.SkillId != AnniversaryHeYanComposition.Yachai || f.WindowContext?.TargetSeat != f.OwnerSeat || f.WindowContext.SourceSeat != r.SourceSeat || r.Plans.Count != 1 || r.Plans[0].Seat != r.SourceSeat))
            throw new InvalidOperationException("Yachai cannot replace its actual damage source.");
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanQingtan)
        {
            if (f.SkillId != AnniversaryHeYanComposition.Qingtan || f.TriggerId is not null || r.Plans.Any(p => p.Cards.Count == 0) || r.Plans.SelectMany(p => p.Cards).Select(c => c.Id).Distinct().Count() != r.Plans.Sum(p => p.Cards.Count) || r.Plans.Any(p => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHeYanChoiceCommittedEvent>().Count(e => e.FrameId == f.Id && e.Seat == p.Seat) != (p.SelectedId is null ? 0 : 1)))
                throw new InvalidOperationException("Qingtan lost a sealed participant's original Hand or commitment.");
            if (r.Stage == AnniversaryHeYanStage.SealedChoice && (r.ChooserIndex >= r.Plans.Count || r.Plans.Where((_, i) => i < r.ChooserIndex).Any(p => p.SelectedId is null) || r.Plans.Where((_, i) => i >= r.ChooserIndex).Any(p => p.SelectedId is not null) || r.Plans.Any(p => !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[p.Seat]).Select(c => c.Id).Order().SequenceEqual(p.Cards.Select(c => c.Id).Order())) || f.CardSetBindings.Any(b => b.Name == "he-yan-shown")))
                throw new InvalidOperationException("A private Qingtan commitment cannot reveal or move a Hand before all commit.");
            if (r.Stage != AnniversaryHeYanStage.SealedChoice && r.Plans.Any(p => p.SelectedId is null))
                throw new InvalidOperationException("Qingtan cannot reveal a partially committed roster.");
        }

        if (r.Stage == AnniversaryHeYanStage.MovementChildren)
        {
            if (r.Payment is not { } paid || paid.Before < 0 || paid.After < paid.Before || paid.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat || paid.CardIds.Distinct().Count() != paid.CardIds.Count || paid.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= paid.Before || m.Sequence > paid.After || m.Reason != HeYanMoveReason(paid.Kind)) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHeYanPaidEvent>().Count(e => e == new AnniversaryHeYanPaidEvent(f.Id, paid.Kind, paid.Executor, paid.Before, paid.After, paid.CardIds.Count)) != 1 || paid.CardIds.Any(id => !paid.Movements.Any(m => m.CardId == id && m.From.Zone == CardZoneKind.Hand)))
                throw new InvalidOperationException("He Yan lost his original one-time physical payment and movement return.");
        }

        if (r.Stage == AnniversaryHeYanStage.DrawChildren && (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } m || m.SubjectSeat != f.OwnerSeat))
            throw new InvalidOperationException("He Yan lost his completed native draw observer return.");
        foreach (var d in r.Draws)
            if (d.SequenceBefore > d.SequenceAfter || d.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHeYanDrawReturnedEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == d.RecipientSeat && e.Requested == d.RequestedCount && e.Actual == d.ActualDrawIds.Count && e.Before == d.SequenceBefore && e.After == d.SequenceAfter && e.AttemptId == d.AttemptFrameId) != 1)
                throw new InvalidOperationException("He Yan lost a native draw's frozen return invoice.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 44200)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 44200)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 44200)]
    internal bool HeYanMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.AnniversaryHeYan is not { Stage: AnniversaryHeYanStage.MovementChildren or AnniversaryHeYanStage.DrawChildren } r || effect?.Op != r.Operation || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null)
            return false;
        RequireHeYan(f);
        return true;
    }

    internal bool HeYanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryHeYan is not { } r)
            return false;
        RequireHeYan(f);
        if (child is PublicHandRevealWindowFrame reveal)
            return r.Stage == AnniversaryHeYanStage.Revealing && reveal.ParentFrameId == f.Id && reveal.ReturnKind == PublicHandRevealReturnKind.Registered && reveal.ReturnKey == HeYanRevealReturn && _characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(f.Id, HeYanRevealReturn);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryHeYanStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == r.DrawTarget && draw.Context.RequestedCount == r.DrawAmount && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (r.Stage is not (AnniversaryHeYanStage.MovementChildren or AnniversaryHeYanStage.DrawChildren))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            var before = r.Stage == AnniversaryHeYanStage.MovementChildren ? r.Payment!.Before : r.Draws[^1].SequenceBefore;
            var after = r.Stage == AnniversaryHeYanStage.MovementChildren ? r.Payment!.After : r.Draws[^1].SequenceAfter;
            return f.PendingMovementContinuation is { } p && HeYanMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > before && m.Sequence <= after && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        }

        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal ProgramSkillFrame? HeYanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterHeYanAnniversaryOperations.HeYanFirstChild(f, child), (i) => _coreSameNameHandDiscardOrDamageObserversOperations2.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 44200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 44200)]
    internal bool HeYanDamageObserver(long id) => HeYanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 44200)]
    internal bool HeYanBeforeDamageObserver(long id) => HeYanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 44200)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 44200)]
    internal bool HeYanDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && HeYanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 44200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 44200)]
    internal bool HeYanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryHeYan is null)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n && HeYanFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && (c == n + 1 || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(n)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44200)]
    internal bool AdvanceHeYanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HeYanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
