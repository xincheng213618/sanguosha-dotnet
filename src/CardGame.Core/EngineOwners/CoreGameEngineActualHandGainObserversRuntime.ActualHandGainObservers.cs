#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualHandGainObserversRuntime
{
    internal bool ValidActualHandGain(ProgramSkillFrame f)
    {
        if (f.ActualHandGain is not { } r || r.InstructionIndex != f.InstructionIndex || _coreActualHandGainProgramsOperations.ActualHandGainPausedEffect(f)?.Op != r.Operation || r.Source != global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime.ActualHandGainSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActualTurnOwner) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.PaidIds.Distinct().Count() != r.PaidIds.Count || r.Gains.Select(g => g.CardId).Distinct().Count() != r.Gains.Count || f.WindowContext is not { } context || context.ParentFrameId != r.OriginalParentId)
            return false;
        if (r.Operation == SkillProgramEffectOp.DrawAfterActualOwnHandGain)
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault(w => w.Id == r.OriginalParentId);
            if (parent is null || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || context.MovementBatch is not { } batch || batch.Id != parent.Batch.Id || !batch.Movements.SequenceEqual(parent.Batch.Movements) || r.OriginalBatchId != parent.Batch.Id || parent.Batch.TurnNumber != r.ActualTurn || parent.Batch.MovementTiming is not { } timing || timing.ActualTurnOwnerSeat != f.OwnerSeat || timing.Phase is TurnPhase.NotStarted or TurnPhase.Finished || r.Gains.Count == 0 || !_coreActualHandGainProgramsOperations.HasRecordedActualOwnHandGain(parent.Candidates[parent.CandidateIndex], parent.Batch) || r.ActualTurnOwner != f.OwnerSeat || r.Category is not null || r.RecipientSeat != -1 || _coreActualHandGainProgramsOperations.IsActualHandGainOwnDrawBatch(parent.Batch, f.OwnerSeat, f.SkillId, f.GameplayHash) || !r.Gains.SequenceEqual(_coreActualHandGainProgramsOperations.ActualHandGainEntities(parent.Batch, f.OwnerSeat)))
                return false;
        }
        else if (r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains)
        {
            var ended = _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault(w => w.Id == r.OriginalParentId);
            if (ended is null || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended)) || ended.OwnerSeat != r.ActualTurnOwner || ended.TurnNumber != r.ActualTurn || ended.OwnerSeat == f.OwnerSeat || r.OriginalBatchId != 0 || r.Category is not null || r.RecipientSeat != -1 || !r.Gains.SequenceEqual(_coreActualHandGainProgramsOperations.ForeignHandGainRoster(f.OwnerSeat, f.SkillId, f.GameplayHash, r.ActualTurn, r.ActualTurnOwner)) || r.Gains.Any(g => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == g.GainSequence && m.CardId == g.CardId && m.TurnNumber == r.ActualTurn && m.To == CardLocation.Hand(f.OwnerSeat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m) != CardLocation.Hand(f.OwnerSeat))) || r.PaidIds.Any(id => !r.Gains.Any(g => g.CardId == id)))
                return false;
        }
        else if (r.Operation == SkillProgramEffectOp.GiveSameCategoryFromDeck)
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == r.OriginalParentId);
            if (parent is null || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) || !_coreActualHandGainProgramsOperations.ExactActualHandGainUse(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]), context, out _, out var use) || r.OriginalCardUseFrameId != use.Id || r.OriginalActionId != parent.Action.ActionId || r.ActualTurnOwner != f.OwnerSeat || r.Category != global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime.ActualHandGainCategory(parent.Action.EffectiveKind) || r.Gains.Count != 0 || r.OriginalBatchId != 0 || r.PaidIds.Count > 1 || r.Stage == ActualHandGainProgramStage.MovementChildren && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.RecipientSeat) || r.RecipientSeat == f.OwnerSeat))
                return false;
        }
        else
            return false;
        if (r.Stage == ActualHandGainProgramStage.Choosing)
        {
            if (r.Operation != SkillProgramEffectOp.GiveSameCategoryFromDeck || r.Issued || r.PaidIds.Count != 0 || r.Before != 0 || r.After != 0 || r.RecipientSeat != -1 || f.PendingMovementContinuation is not null || _coreActualHandGainProgramsOperations.SameCategoryGiftUsed(f.OwnerSeat, f.SkillId, r.ActualTurn, r.Category!.Value))
                return false;
            return _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p && p.PlayerSeat == f.OwnerSeat && p.TargetSeat == f.OwnerSeat && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, _coreActualHandGainProgramsOperations.ActualHandGainChoices(f));
        }

        if (r.Stage != ActualHandGainProgramStage.MovementChildren || !r.Issued || f.PendingMovementContinuation is not { } pending || !_coreActualHandGainProgramsOperations.IsActualHandGainMovement(f, _coreActualHandGainProgramsOperations.ActualHandGainPausedEffect(f), pending))
            return false;
        var reason = r.Operation == SkillProgramEffectOp.DrawAfterActualOwnHandGain ? "program.actual-hand-gain.draw" : r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains ? "program.foreign-turn-hand-gain.cleanup" : "program.same-category-deck.gift";
        var movements = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == reason).ToArray();
        if (!movements.Select(m => m.CardId).SequenceEqual(r.PaidIds) || movements.Any(m => r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains ? m.From != CardLocation.Hand(f.OwnerSeat) || m.To != CardLocation.DiscardPile : m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(r.Operation == SkillProgramEffectOp.DrawAfterActualOwnHandGain ? f.OwnerSeat : r.RecipientSeat) || r.Operation == SkillProgramEffectOp.GiveSameCategoryFromDeck && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(m.CardKind, r.Category!.Value)))
            return false;
        if (r.Operation == SkillProgramEffectOp.DrawAfterActualOwnHandGain)
        {
            var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualHandGainDrawPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
            return r.PaidIds.Count <= 1 && facts is [var e] && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.OriginalBatchId == r.OriginalBatchId && e.ActualTurn == r.ActualTurn && e.ActualTurnOwner == r.ActualTurnOwner && e.Before == r.Before && e.After == r.After && e.ActualCount == r.PaidIds.Count;
        }

        if (r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains)
        {
            var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ForeignTurnHandGainsCleanupPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
            return facts is [var e] && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ActualTurn == r.ActualTurn && e.ActualTurnOwner == r.ActualTurnOwner && e.CardIds.SequenceEqual(r.PaidIds) && e.Before == r.Before && e.After == r.After;
        }

        var gifts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameCategoryDeckGiftIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        return gifts is [var gift] && gift.Source == r.Source && gift.GameplayHash == r.GameplayHash && gift.ActualTurn == r.ActualTurn && gift.ActualTurnOwner == r.ActualTurnOwner && gift.CardUseFrameId == r.OriginalCardUseFrameId && gift.ActionId == r.OriginalActionId && gift.Category == r.Category && gift.RecipientSeat == r.RecipientSeat && gift.ActualCount == r.PaidIds.Count && gift.Before == r.Before && gift.After == r.After && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameCategoryDeckGiftIssuedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.ActualTurn == r.ActualTurn && e.Category == r.Category) == 1;
    }

    internal bool ActualHandGainFirstChild(ProgramSkillFrame root, ResolutionFrame child) => ValidActualHandGain(root) && root.ActualHandGain is { Stage: ActualHandGainProgramStage.MovementChildren } r && root.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame moved && moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    internal ProgramSkillFrame? ActualHandGainObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreActualHandGainObserversOperations.ActualHandGainFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasHandGainDebtObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 190)]
    internal bool IsActualHandGainProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ActualHandGainObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1720)]
    internal bool HasActualHandGainDamageObserver(long window) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == window && f is DamageTriggerWindowFrame) && ActualHandGainObserverRoot()is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 500)]
    internal bool TryAdvanceActualHandGainSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || ActualHandGainObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: false);
    }
}
