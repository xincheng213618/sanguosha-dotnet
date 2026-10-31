#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    internal bool ValidQiqinReceipt(ProgramSkillFrame f)
    {
        if (f.Qiqin is not { } r || f.SkillId != "le:qiqin" || f.TriggerId != "collect-discard-qin" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.Source != QiqinSource(f) || r.GameplayHash != f.GameplayHash || f.WindowContext?.ParentFrameId != r.WindowId || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || r.ActualTurn <= 0 || r.TurnOwner != f.OwnerSeat || r.Stage != QiqinStage.ObtainChildren || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Entities is not IList { IsReadOnly: true } || r.Entities.Select(e => e.CardId).Distinct().Count() != r.Entities.Count || r.Entities.Any(e => !HasQiqinEntityMark(e.CardId) || _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Kind != e.PrintedKind) || _coreProgramEventHistoryOperations.ProgramEventHistory<QiqinCollectStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued != new QiqinCollectStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.ActualTurn, r.TurnOwner, r.Entities.Count, r.SequenceBefore))
            return false;
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<QiqinCollectEntityIssuedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.Entities.Count || selected.Where((e, i) => e != new QiqinCollectEntityIssuedEvent(f.Id, i, r.Entities[i].CardId, r.Entities[i].PrintedKind)).Any())
            return false;
        var moved = _coreProgramEventHistoryOperations.ProgramEventHistory<QiqinCollectedEntityEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<QiqinCollectPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Entities.Count == 0)
            return r.BatchId == 0 && r.SequenceBefore == r.SequenceAfter && moved.Length == 0 && paid.Length == 0;
        return r.BatchId > f.Id && r.SequenceAfter == r.SequenceBefore + r.Entities.Count && paid is [var fact] && fact == new QiqinCollectPaidEvent(f.Id, r.BatchId, r.Entities.Count, r.SequenceBefore, r.SequenceAfter) && moved.Length == r.Entities.Count && !moved.Where((e, i) => e.Index != i || e.CardId != r.Entities[i].CardId || e.PrintedKind != r.Entities[i].PrintedKind || e.MovementSequence != r.SequenceBefore + i + 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == QiqinObtainReason) != 1).Any();
    }
    internal bool QiqinBatchMatches(ProgramSkillFrame f, CardMovementBatchContext batch)
    {
        if (f.Qiqin is not { } r || !ValidQiqinReceipt(f))
            return false;
        return batch.Id == r.BatchId && batch.ParentFrameId == f.Id && batch.AwaitingProgramFrameId == f.Id && batch.OriginOwnerSeat == f.OwnerSeat && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId && batch.Movements.Count == r.Entities.Count && !batch.Movements.Where((m, i) => m.CardId != r.Entities[i].CardId || m.CardKind != r.Entities[i].PrintedKind || m.Sequence != r.SequenceBefore + i + 1 || m.From != CardLocation.DiscardPile || m.To != CardLocation.Hand(f.OwnerSeat) || m.Reason.Value != QiqinObtainReason).Any();
    }

    internal bool QiqinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidQiqinReceipt(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && QiqinBatchMatches(f, moved.Batch);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
    }

    internal bool QiqinObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations.IsMovementObserverEdge(next) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(next, dying));
    internal ProgramSkillFrame? QiqinObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterXiaoQiaoLeOperations.QiqinFirstChild(f, child), (index) => _characterXiaoQiaoLeOperations.QiqinObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 420)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 410)]
    internal bool IsQiqinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(s => s.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(s => s.Id == recovery.Id);
        return index >= 0 && child > index + 1 && QiqinFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && QiqinObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28610)]
    internal bool AdvanceQiqinChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QiqinObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
