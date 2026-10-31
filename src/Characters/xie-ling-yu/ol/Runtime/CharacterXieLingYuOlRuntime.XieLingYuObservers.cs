#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXieLingYuOlRuntime
{
    internal bool CanContinueIssuedXieLingYu(ProgramSkillFrame f)
    {
        if (f.XieLingYu is { Paid: true })
        {
            AssertXieLingYu(f);
            return true;
        }

        if (f.InstructionIndex is not (0 or 1) || f.TriggerId is null || f.WindowContext is not { } c)
            return false;
        return IsIssuedYuandiCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27581)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27581)]
    internal bool PermitIssuedXieLingYu(ProgramSkillFrame f) => CanContinueIssuedXieLingYu(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27581)]
    internal void AssertXieLingYu(ProgramSkillFrame f)
    {
        if (f.XieLingYu is not { } r)
            return;
        if (f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != XieLingYuSource(f) || r.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !XieLingYuComposition.Handles(r.Op) || !Enum.IsDefined(r.Stage) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.Requests is < 0 or > 5 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Selected is not System.Collections.IList { IsReadOnly: true } || r.Materials is not System.Collections.IList { IsReadOnly: true } || r.Gains is not System.Collections.IList { IsReadOnly: true } || r.Selected.Distinct().Count() != r.Selected.Count || r.Materials.Select(c => c.CardId).Distinct().Count() != r.Materials.Count || r.Gains.Select(c => c.CardId).Distinct().Count() != r.Gains.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<XieLingYuStartedEvent>().Count(e => e == new XieLingYuStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.TurnOwner, r.PlayPhase?.Token ?? 0)) != 1)
            throw new InvalidOperationException("Xie Ling Yu lost her immutable owning source, actual turn, native cursor or started proof.");
        if (r.Op == SkillProgramEffectOp.ResolveYuandiDiscardNames)
        {
            if (!r.Paid || f.TriggerId != "discard-name-group" || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } c || r.WindowId != c.ParentFrameId || r.WindowId != batch.Id || r.PlayPhase is not null || r.Materials.Count != 0 || r.Selected.Count != 0 || r.Requests > 1 || r.Stage is XieLingYuStage.Cost or XieLingYuStage.CostChildren || _coreProgramEventHistoryOperations.ProgramEventHistory<YuandiPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid != new YuandiPaidEvent(f.Id, f.OwnerSeat, r.WindowId, r.ActualTurn, r.TurnOwner, r.OpportunitySequence, r.NamesMask, r.NonUse) || _coreProgramEventHistoryOperations.ProgramEventHistory<YuandiPaidEvent>().Count(e => e.OwnerSeat == f.OwnerSeat && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<YuandiOpportunityEvent>().Any(e => e.BatchId == batch.Id && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash && e.CompletionSequence == r.OpportunitySequence && e.NamesMask == r.NamesMask && e.NonUse == r.NonUse))
                throw new InvalidOperationException("Yuandi lost its one real first-group opportunity and frozen remaining printed names.");
        }
        else
        {
            if (f.TriggerId is not null || f.WindowContext is not null || f.ActivationId != "basic-cycle" || r.WindowId != 0 || r.PlayPhase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || phase.TurnNumber != r.ActualTurn || phase.ActualTurnOwnerSeat != r.TurnOwner || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || !r.Paid && (r.Stage != XieLingYuStage.Cost || r.Materials.Count != 0 || r.NamesMask != 0 || r.Requests != 0))
                throw new InvalidOperationException("Xinyou lost its actual Play draft or spent a canceled initial selection.");
            if (r.Paid)
            {
                var frozen = _coreProgramEventHistoryOperations.ProgramEventHistory<XinyouMaterialFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
                if (r.Materials.Count == 0 || r.NamesMask == 0 || r.Stage == XieLingYuStage.Cost || !frozen.Select(e => new XieLingYuMaterial(e.CardId, e.PrintedKind, e.BasicName, CardLocation.Hand(f.OwnerSeat))).SequenceEqual(r.Materials) || r.Materials.Aggregate(0, (mask, c) => mask | XieLingYuNameBit(c.BasicName)) != r.NamesMask || _coreProgramEventHistoryOperations.ProgramEventHistory<XinyouPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.NamesMask != r.NamesMask || paid.MaterialCount != r.Materials.Count || paid.PhaseToken != phase.Token || paid.OwnerSeat != f.OwnerSeat || paid.ActualTurn != r.ActualTurn || paid.TurnOwner != r.TurnOwner || _coreProgramEventHistoryOperations.ProgramEventHistory<XinyouPaidEvent>().Count(e => e.OwnerSeat == f.OwnerSeat && e.PhaseToken == phase.Token) != 1)
                    throw new InvalidOperationException("Xinyou lost its one initial whole Hand Basic cost and fixed discarded-name set.");
                foreach (var material in r.Materials)
                    if (_coreProgramEventHistoryOperations.ProgramEventHistory<XinyouMaterialDiscardedEvent>().Where(e => e.FrameId == f.Id && e.CardId == material.CardId).ToArray()is not [var moved] || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == moved.MovementSequence && m.CardId == material.CardId && m.CardKind == material.PrintedKind && m.From == material.From && (m.To == CardLocation.DiscardPile || _coreHellMengpoProgramsOperations.IsHellShiyouPaidDiscardMovement(m, material.From, new(XinyouDiscardReason))) && m.Reason.Value == XinyouDiscardReason))
                        throw new InvalidOperationException("Xinyou cannot replace or repay an original cost entity.");
            }
        }

        if (r.Stage == XieLingYuStage.DrawChildren)
        {
            if (r.Outcome is not { } outcome || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || outcome.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || r.Gains.Count != outcome.ActualDrawIds.Count + outcome.ReplacementGainedIds.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<XieLingYuDrawReturnedEvent>().Where(e => e.FrameId == f.Id && e.Request == r.Requests).ToArray()is not [var returned] || returned.ActualCount != outcome.ActualDrawIds.Count || returned.ReplacementCount != outcome.ReplacementGainedIds.Count || returned.Before != r.Before || returned.After != r.After || returned.Stop != r.Stop || returned.AttemptId != outcome.AttemptFrameId)
                throw new InvalidOperationException("Xie Ling Yu lost an exact native draw or replacement outcome.");
            if (r.Op == SkillProgramEffectOp.ResolveXinyouBasicCycle && !_coreProgramEventHistoryOperations.ProgramEventHistory<XinyouGainedEntityEvent>().Where(e => e.FrameId == f.Id && e.Request == r.Requests).OrderBy(e => e.Index).Select(e => new XieLingYuGain(e.CardId, e.PrintedKind, e.MovementSequence, e.Replacement)).SequenceEqual(r.Gains))
                throw new InvalidOperationException("Xinyou cannot exempt an entity absent from its actual gain invoice.");
        }

        if (f.PendingMovementContinuation is { } p && !IsXieLingYuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            throw new InvalidOperationException("Xie Ling Yu retained an unrelated movement continuation.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 530)]
    internal bool IsXieLingYuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.XieLingYu is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is XieLingYuStage.CostChildren or XieLingYuStage.DrawChildren;
    internal bool IsXieLingYuPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.XieLingYu!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == XieLingYuStage.CostChildren)
            return m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == XinyouDiscardReason && r.Materials.Any(c => c.CardId == m.CardId && c.PrintedKind == m.CardKind && c.From == m.From) && _coreProgramEventHistoryOperations.ProgramEventHistory<XinyouMaterialDiscardedEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.MovementSequence == m.Sequence);
        if (r.Stage != XieLingYuStage.DrawChildren || r.Outcome is not { } o)
            return false;
        return r.Gains.Any(c => c.CardId == m.CardId && c.PrintedKind == m.CardKind && c.MovementSequence == m.Sequence && m.To == CardLocation.Hand(f.OwnerSeat)) || m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool XieLingYuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.XieLingYu is not { Paid: true } r)
            return false;
        AssertXieLingYu(f);
        if (r.Stage == XieLingYuStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsXieLingYuPaidMovement(f, m));
    }

    internal ProgramSkillFrame? XieLingYuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterXieLingYuOlOperations.XieLingYuFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1050)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1160)]
    internal bool HasXieLingYuDamageObserver(long id) => XieLingYuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1190)]
    internal bool HasXieLingYuBeforeDamageObserver(long id) => XieLingYuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1020)]
    internal bool IsXieLingYuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && XieLingYuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 280)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 270)]
    internal bool IsXieLingYuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.XieLingYu is not { Paid: true })
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var recoveryIndex = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return rootIndex >= 0 && recoveryIndex > rootIndex + 1 && XieLingYuFirstChild(f, _resolutionStackOperations._resolutionStack[rootIndex + 1]) && _coreObserverSubtreesOperations.HasObserverSuffix(rootIndex, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[recoveryIndex - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27581)]
    internal bool AdvanceXieLingYuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XieLingYuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
