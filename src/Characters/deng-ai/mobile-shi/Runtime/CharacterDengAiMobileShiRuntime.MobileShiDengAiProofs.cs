#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiMobileShiRuntime
{
    internal bool ShiDaWindowMatches(ProgramSkillFrame f, SkillProgramTriggerWindow expected)
    {
        if (f.WindowContext is not { } context || context.Window != expected || context.OwnerSeat != f.OwnerSeat || f.TriggerId is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId);
        if (parent is DeferredTurnEndFrame ended)
            return expected == SkillProgramTriggerWindow.AfterTurnEnded && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(ended, f, context);
        if (parent is ProgramLifecycleTriggerWindowFrame lifecycle)
            return lifecycle.Window == expected && lifecycle.CandidateIndex >= 0 && lifecycle.CandidateIndex < lifecycle.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, lifecycle.Candidates[lifecycle.CandidateIndex]);
        if (parent is CardsMovedTriggerWindowFrame moved)
            return expected == SkillProgramTriggerWindow.CardsMoved && context.MovementBatch is { } batch && batch.Id == moved.Batch.Id && batch.Movements.SequenceEqual(moved.Batch.Movements) && moved.CandidateIndex >= 0 && moved.CandidateIndex < moved.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, moved.Candidates[moved.CandidateIndex]);
        return parent is ProgramCardTriggerWindowFrame card && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(card) == expected && card.CandidateIndex >= 0 && card.CandidateIndex < card.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(card.Candidates[card.CandidateIndex])) && context.CardUse?.CardActionId == card.Action.ActionId;
    }

    internal bool ValidShiDa(ProgramSkillFrame f)
    {
        if (f.ShiDengAi is not { } r || r.Source != ShiDaSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || f.InstructionIndex != 1 || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.WindowId != (f.WindowContext?.ParentFrameId ?? 0) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaStartedEvent>().Count(e => e == new ShiDaStartedEvent(f.Id, r.Source, r.Hash, r.Op, r.Turn, r.TurnOwner, r.WindowId)) != 1)
            return false;
        return r.Op switch
        {
            SkillProgramEffectOp.ResolveMobileShiDengAiFull => f.SkillId == ShiDaTuntian && f.TriggerId == "full-charge" && ShiDaWindowMatches(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),
            SkillProgramEffectOp.ResolveMobileShiDengAiTuntian => f.SkillId == ShiDaTuntian && f.TriggerId is null && f.ActivationId == "spend-charge" && f.WindowContext is null && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == f.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play,
            SkillProgramEffectOp.ResolveMobileShiDengAiJixi => f.SkillId == ShiDaJixi && f.TriggerId == "ending-jixi" && ShiDaWindowMatches(f, SkillProgramTriggerWindow.AfterTurnEnded),
            _ => false
        };
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 50300)]
    internal void RequireShiDa(ProgramSkillFrame f)
    {
        if (f.ShiDengAi is not { } r)
            return;
        if (!ValidShiDa(f) || r.Targets.Count != r.Targets.Distinct().Count() || r.Targets.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || r.History.Count != r.History.Distinct().Count() || r.History.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || seat == f.OwnerSeat) || r.Cursor < 0 || r.Cursor > r.Targets.Count || r.RewardCursor is < 0 or > 3 || r.Amount < -1 || r.Op == SkillProgramEffectOp.ResolveMobileShiDengAiTuntian && (r.Stage != ShiDaStage.Amount && (r.Amount < 0 || r.Targets.Count > r.Amount) || r.Paid && (_coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaSpentEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.Source != r.Source || paid.Amount != r.Amount || paid.Recipients != r.Targets.Count || paid.Zaoxian != r.Zaoxian || !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaRecipientEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Targets))) || r.Op == SkillProgramEffectOp.ResolveMobileShiDengAiJixi && r.Targets.Any(seat => !r.History.Contains(seat)))
            throw new InvalidOperationException("Shi Deng Ai lost its exact original boundary, charge payment or selected historical targets.");
        if (r.Draw is { } draw && (r.Op != SkillProgramEffectOp.ResolveMobileShiDengAiFull || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaDrawPaidEvent>().Count(e => e == new ShiDaDrawPaidEvent(f.Id, draw.AttemptFrameId, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter)) != 1))
            throw new InvalidOperationException("Tuntian's full-charge draw must retain its exact one native outcome.");
        if (r.Op == SkillProgramEffectOp.ResolveMobileShiDengAiJixi && r.Paid && (r.CardFrom is not { } from || !ShiDaOwns(from, r.TurnOwner) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaCostPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var cost] || cost.Owner != r.TurnOwner || cost.CardId != r.CardId || cost.From != from || cost.Sequence != r.PaymentSequence || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == cost.Sequence && m.CardId == cost.CardId && m.From == cost.From && m.To == cost.To && m.Reason.Value == ShiDaCostReason)))
            throw new InvalidOperationException("Jixi cannot replace or repay its original real discard.");
        if (r.Stage == ShiDaStage.HeartChildren)
        {
            var candidates = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaRandomCandidateEvent>().Where(e => e.FrameId == f.Id && e.Ordinal == r.Cursor).OrderBy(e => e.Index).Select(e => e.CardId).ToArray();
            var generator = new DeterministicRandom(unchecked((int)r.RandomBefore));
            var index = candidates.Length == 0 ? -1 : generator.Next(candidates.Length);
            if (!candidates.SequenceEqual(r.Eligible) || r.RandomIndex != index || generator.State != r.RandomAfter || r.CardId != (index < 0 ? 0 : candidates[index]) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaRandomIssuedEvent>().Count(e => e == new ShiDaRandomIssuedEvent(f.Id, r.Cursor, r.Targets[r.Cursor], r.CardId, index, candidates.Length, r.RandomBefore, r.RandomAfter)) != 1)
                throw new InvalidOperationException("Tuntian's random Heart must retain the original issued sample without rerolling.");
        }

        if (r.NativeActionId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaSnatchIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.NativeUseId && e.ActionId == r.NativeActionId && e.Origin == r.Origin) != 1)
            throw new InvalidOperationException("Jixi lost its exact one whole native Snatch.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && f.Step == ResolutionFrameStep.AwaitingResponse && r.Stage is ShiDaStage.Amount or ShiDaStage.Recipients or ShiDaStage.Rewards or ShiDaStage.Cost or ShiDaStage.Targets && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || prompt.PlayerSeat != f.OwnerSeat || !prompt.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, ShiDaChoices(f))))
            throw new InvalidOperationException("Shi Deng Ai lost the current original unpaid prompt.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 50300)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 50300)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 50300)]
    internal bool ShiDaMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.ShiDengAi is { Paid: true } r && ValidShiDa(f) && effect?.Op == r.Op && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && r.Stage is ShiDaStage.DrawChildren or ShiDaStage.RewardChildren or ShiDaStage.HeartChildren or ShiDaStage.CostChildren or ShiDaStage.NativeChildren;
    internal bool ShiDaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ShiDengAi is not { Paid: true } r || !ValidShiDa(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == ShiDaStage.Drawing && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == 1 && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardUseFrame use)
            return r.Stage == ShiDaStage.NativeUse && use.Id == r.NativeUseId && ValidShiDaVirtualSnatchUse(use);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (f.PendingMovementContinuation is not { } pending || !ShiDaMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) || !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
                return false;
            if (r.Stage == ShiDaStage.NativeChildren)
                return r.NativeBatches.Contains(moved.Batch.Id) && moved.Batch.ParentFrameId == r.NativeUseId && moved.Batch.AwaitingProgramFrameId is null && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaNativeBatchEvent>().Count(e => e == new ShiDaNativeBatchEvent(f.Id, r.NativeUseId, moved.Batch.Id, moved.Batch.ParentBatchId, moved.Batch.Movements.Count)) == 1;
            return moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        }

        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ShiDaNativeEdge(int i)
    {
        if (_resolutionStackOperations._resolutionStack[i - 1] is CardUseFrame use && ValidShiDaVirtualSnatchUse(use) && _resolutionStackOperations._resolutionStack[i] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[i - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { } native && ValidShiDaVirtualSnatchUse(native) && _resolutionStackOperations._resolutionStack[i] is ProgramSkillFrame f && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext(parent, parent.Candidates[parent.CandidateIndex]);
        return false;
    }

    internal bool ShiDaChildEdge(int i) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || ShiDaNativeEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiDaFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiDaChildSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterDengAiMobileShiOperations.ShiDaChildEdge(n), (n, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? ShiDaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterDengAiMobileShiOperations.ShiDaFirstChild(f, child), (i) => _characterDengAiMobileShiOperations.ShiDaChildSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 50300)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 50300)]
    internal bool ShiDaDamageObserver(long id) => ShiDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 50300)]
    internal bool ShiDaBeforeDamageObserver(long id) => ShiDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 50300)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 50300)]
    internal bool ShiDaDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ShiDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 50300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 50300)]
    internal bool ShiDaRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiDaFirstChild(f, recovery))
            return true;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && ShiDaFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && ShiDaChildSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 50300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 50300)]
    internal bool KeepPaidShiDa(ProgramSkillFrame f) => f.ShiDengAi is { Paid: true } && ValidShiDa(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 50300)]
    internal bool AdvanceShiDaChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiDaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
