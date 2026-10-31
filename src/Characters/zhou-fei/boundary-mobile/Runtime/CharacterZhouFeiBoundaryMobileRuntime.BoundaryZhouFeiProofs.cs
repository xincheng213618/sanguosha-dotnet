#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiBoundaryMobileRuntime
{
    internal bool ValidBoundaryZf(ProgramSkillFrame frame)
    {
        if (frame.BoundaryZhouFei is not { } receipt || receipt.Source != BoundaryZfSource(frame) || receipt.Hash != frame.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || frame.InstructionIndex != 1 || frame.WindowContext is not { } context || receipt.WindowId != context.ParentFrameId || frame.ActivationId != frame.TriggerId || !Enum.IsDefined(receipt.Stage) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.Holder) || receipt.Before < 0 || receipt.After < receipt.Before || receipt.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || receipt.Selected.Distinct().Count() != receipt.Selected.Count || receipt.Deposits.Distinct().Count() != receipt.Deposits.Count || receipt.Cursor < 0 || receipt.Cursor > receipt.Deposits.Count || receipt.Payment.Select(material => material.CardId).Distinct().Count() != receipt.Payment.Count || !_resolutionStackOperations._resolutionStack.Any(parent => parent.Id == receipt.WindowId) || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiStartedEvent>().Count(entry => entry == new BoundaryZhouFeiStartedEvent(frame.Id, receipt.Source, receipt.Hash, receipt.Op, receipt.WindowId, receipt.Round, receipt.Holder, receipt.Op == SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin && context.Window == SkillProgramTriggerWindow.CardsMoved ? receipt.Direction : -1)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(entry => entry.FrameId == frame.Id && entry.SkillId == frame.SkillId && entry.BindingId == frame.TriggerId && entry.SkillInstanceId == frame.SkillInstanceId && entry.OwnerSeat == frame.OwnerSeat && entry.Window == context.Window) != 1)
            return false;
        if (receipt.Op == SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng)
        {
            if (frame.SkillId != BoundaryZfKongsheng || frame.TriggerId is not ("prepare-store" or "health-store") || receipt.Location != new CardLocation(CardZoneKind.PrivatePersistentPile, receipt.Holder, $"boundary-zhou-fei:{(frame.Id)}") || context.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterHealthChanged) || context.Window == SkillProgramTriggerWindow.AfterHealthChanged && (context.HpChange is not { Kind: HpChangeKind.Loss or HpChangeKind.Recovery, Amount: > 0 } change || change.TargetSeat != frame.OwnerSeat) || receipt.Stage is not (BoundaryZhouFeiStage.Store or BoundaryZhouFeiStage.StoreChildren))
                return false;
            if (receipt.Stage == BoundaryZhouFeiStage.Store)
                return receipt.Payment.Count == 0 && receipt.Selected.All(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat == receipt.Holder && _cardZonesOperations._cardZones.GetLocation(id).Zone is CardZoneKind.Hand or CardZoneKind.Equipment);
            return receipt.Payment.Count == receipt.Selected.Count && receipt.Payment.All(material => material.From.OwnerSeat == receipt.Holder && material.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After && move.CardId == material.CardId && move.From == material.From && move.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId), material.From, receipt.Location!.Value) && move.Reason.Value == BoundaryZfStoreReason) == 1);
        }

        if (receipt.Op == SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng)
        {
            if (frame.SkillId != BoundaryZfKongsheng || frame.TriggerId != "return-held" || context.Window != SkillProgramTriggerWindow.TurnEnding || context.TargetSeat != receipt.Holder || receipt.Deposits.Any(id =>
            {
                var deposit = BoundaryZfDeposit(id);
                return deposit.Holder != receipt.Holder || deposit.Source.OwnerSeat != frame.OwnerSeat || deposit.Source.SkillInstanceId != frame.SkillInstanceId || deposit.Hash != frame.GameplayHash;
            }) || receipt.Stage is not (BoundaryZhouFeiStage.UseChoice or BoundaryZhouFeiStage.IssuingUse or BoundaryZhouFeiStage.NativeUse or BoundaryZhouFeiStage.Used or BoundaryZhouFeiStage.ReturnChildren))
                return false;
            if (receipt.Stage == BoundaryZhouFeiStage.ReturnChildren)
                return receipt.Cursor < receipt.Deposits.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiDepositReturnedEvent>().Count(entry => entry.FrameId == frame.Id && entry.DepositId == receipt.Deposits[receipt.Cursor] && entry.Holder == receipt.Holder && entry.Count == receipt.Payment.Count && entry.Before == receipt.Before && entry.After == receipt.After) == 1 && receipt.Payment.All(material => material.From == BoundaryZfDeposit(receipt.Deposits[receipt.Cursor]).Location && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After && move.CardId == material.CardId && move.From == material.From && move.Reason.Value == BoundaryZfReturnReason && move.To == receipt.Location));
            if (receipt.Stage == BoundaryZhouFeiStage.UseChoice)
                return receipt.Native is null;
            return receipt.Native is { } native && receipt.Cursor < receipt.Deposits.Count && native.ParentId == frame.Id && native.Source == receipt.Source && native.Hash == receipt.Hash && native.DepositId == receipt.Deposits[receipt.Cursor] && native.From == BoundaryZfDeposit(native.DepositId).Location && (receipt.Stage == BoundaryZhouFeiStage.IssuingUse ? native.UseId == 0 && native.ActionId == 0 && _cardZonesOperations._cardZones.GetLocation(native.CardId) == native.From : native.UseId > 0 && native.ActionId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiNativeEvent>().Count(entry => entry == new BoundaryZhouFeiNativeEvent(frame.Id, native.DepositId, native.UseId, native.ActionId, native.CardId)) == 1);
        }

        if (receipt.Op != SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin || frame.SkillId != BoundaryZfLiangyin)
            return false;
        if (context.Window == SkillProgramTriggerWindow.CardsMoved)
        {
            if (context.MovementBatch is not { } batch || batch.Id != receipt.WindowId || !BoundaryZfMovementGroups(batch).Any(group => group.Encoded == context.OccurrenceIndex && group.Direction == receipt.Direction && group.Index == context.MovementIndex) || receipt.Stage is not (BoundaryZhouFeiStage.Target or BoundaryZhouFeiStage.Discard or BoundaryZhouFeiStage.DiscardChildren or BoundaryZhouFeiStage.Drawing or BoundaryZhouFeiStage.DrawChildren))
                return false;
            if (receipt.Stage == BoundaryZhouFeiStage.Target)
                return receipt.Target == -1;
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.Target) || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiTargetEvent>().Count(entry => entry == new BoundaryZhouFeiTargetEvent(frame.Id, frame.OwnerSeat, receipt.Round, receipt.Target, receipt.Direction == 0)) != 1)
                return false;
            if (receipt.Stage == BoundaryZhouFeiStage.DrawChildren)
                return receipt.Draw is { } draw && draw.RequestedCount == 1 && draw.RecipientSeat == receipt.Target && draw.SequenceBefore == receipt.Before && draw.SequenceAfter == receipt.After && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiDrawEvent>().Count(entry => entry == new BoundaryZhouFeiDrawEvent(frame.Id, receipt.Target, draw.AttemptFrameId, draw.ActualDrawIds.Count, receipt.Before, receipt.After)) == 1;
            if (receipt.Stage == BoundaryZhouFeiStage.DiscardChildren)
                return receipt.Payment is [var material] && material.From.OwnerSeat == receipt.Target && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(entry => entry.ParentFrameId == frame.Id && entry.InitiatorSeat == frame.OwnerSeat && entry.ExecutorSeat == receipt.Target && entry.SubjectSeat == receipt.Target && entry.ActualCount == 1 && entry.Reason.Value == BoundaryZfDiscardReason && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(paid => paid.InvocationId == entry.InvocationId && paid.CardId == material.CardId && paid.From == material.From && paid.CompletionSequence > receipt.Before && paid.CompletionSequence <= receipt.After));
            return receipt.Draw is null && receipt.Payment.Count == 0;
        }

        return context.Window == SkillProgramTriggerWindow.RoundEnding && receipt.Stage is BoundaryZhouFeiStage.RoundChoice or BoundaryZhouFeiStage.EffectChildren && (receipt.Stage == BoundaryZhouFeiStage.RoundChoice || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiPaidEvent>().Count(entry => entry.FrameId == frame.Id && entry.Target == receipt.Target && entry.Branch == (receipt.Direction == 2 ? "recover" : "lose-hp") && entry.Count == 1) == 1);
    }
    internal bool BoundaryZfChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (!ValidBoundaryZf(frame))
            return false;
        var receipt = frame.BoundaryZhouFei!;
        if (child is CardUseFrame use)
            return receipt.Stage == BoundaryZhouFeiStage.NativeUse && BoundaryZfNativeRidesOn(use, frame);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, frame))
            return receipt.Stage == BoundaryZhouFeiStage.Drawing && frame.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(frame, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return frame.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > receipt.Before && move.Sequence <= receipt.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == frame.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(entry => entry.Change.Id == state.Id && entry.Change.ParentFrameId == frame.Id));
    }

    internal bool BoundaryZfSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame frame && this.BoundaryZfChild(frame, _resolutionStackOperations2._resolutionStack()[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? BoundaryZfRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.BoundaryZfChild(frame, child), (index) => this.BoundaryZfSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54700)]
    internal bool BoundaryZfDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BoundaryZfRoot()is { } frame && _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54700)]
    internal bool BoundaryZfRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child)
    {
        if (!ValidBoundaryZf(frame))
            return false;
        if (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == frame.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == child.Id);
        return a >= 0 && b > a + 1 && BoundaryZfChild(frame, _resolutionStackOperations._resolutionStack[a + 1]) && BoundaryZfSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54700)]
    internal bool KeepBoundaryZf(ProgramSkillFrame frame) => frame.BoundaryZhouFei is not null ? ValidBoundaryZf(frame) : frame.SkillId == BoundaryZfKongsheng && frame.TriggerId == "return-held" && frame.WindowContext is { } context && IsIssuedBoundaryZf(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0), context);
    [ProgramContinuation(ProgramContinuationKind.Resume, 54700)]
    internal bool ResumeBoundaryZf(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhouFei: not null } frame || frame.Id != id)
            return false;
        ContinueBoundaryZf(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 54700)]
    internal bool ReturnBoundaryZfMovement(ProgramSkillFrame frame) => frame.PendingMovementContinuation is not null && ResumeBoundaryZf(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54700)]
    internal bool AdvanceBoundaryZfChild() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryZfRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
