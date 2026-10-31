#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27470)]
    internal void AssertCuiYan(ProgramSkillFrame f)
    {
        if (f.CuiYan is not { } r)
            return;
        if (f.InstructionIndex != 1 || f.TriggerId is null || r.Source != CuiYanSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || context.OwnerSeat != f.OwnerSeat || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !Enum.IsDefined(r.Stage) || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanStartedEvent>().Count(e => e == new CuiYanStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.TurnOwner, r.UseId, r.ActionId, r.Suit, r.DrawQualified)) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanFrozenTargetEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.TargetSeat).SequenceEqual(r.Targets))
            throw new InvalidOperationException("Cui Yan lost his exact original source, boundary, frozen targets or started instruction.");
        if (r.Op == SkillProgramEffectOp.ResolveShutongFinalizedUse)
        {
            if (r.Stage != CuiYanStage.DrawChildren || context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || context.CardUse is not { } identity || identity.ParentCardUseFrameId != r.UseId || identity.CardActionId != r.ActionId || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } w || w.ParentFrameId != r.UseId || w.Action.ActionId != r.ActionId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex])) || f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualDraw is < 0 or > 1 || !r.DrawQualified && r.ActualDraw != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanDrawIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var invoice] || invoice != new CuiYanDrawIssuedEvent(f.Id, r.Before, r.After, r.ActualDraw) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == CuiYanDrawReason) != r.ActualDraw || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).Any(m => !CuiYanDrawMovement(f, m)))
                throw new InvalidOperationException("Shutong lost its real native target-finalized parent or once-issued Draw1 interval.");
            var grants = _coreProgramEventHistoryOperations.ProgramEventHistory<ShutongNextUseGrantedEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (grants.Select(e => e.RecipientSeat).Distinct().Count() != grants.Length || grants.Any(e => e.Source != r.Source || e.GameplayHash != r.GameplayHash || e.UseId != r.UseId || e.ActionId != r.ActionId || e.ActualTurn != r.ActualTurn || e.TurnOwner != r.TurnOwner || e.Suit != r.Suit || !r.Targets.Append(f.OwnerSeat).Contains(e.RecipientSeat)) || r.Suit == Suit.None && grants.Length != 0)
                throw new InvalidOperationException("Shutong cannot invent, duplicate or recolor an issued next-use obligation.");
        }
        else
        {
            if (context.Window != SkillProgramTriggerWindow.AfterTurnEnded || r.UseId != 0 || r.ActionId != 0 || r.Targets.Count != 0 || r.Stage == CuiYanStage.DrawChildren || f.PendingMovementContinuation is not null || !_coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanEndQualifiedEvent>().Any(e => e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner && e.OwnerSeat == f.OwnerSeat && e.UsedOrPlayed && e.WasTargeted) || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == r.WindowId)is not { } parent || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent)is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate))
                throw new InvalidOperationException("Zhijie lost its actual ending qualification and typed native parent.");
            if (r.Stage == CuiYanStage.DamageChildren && (r.DamageTarget is not { } target || _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanDamageIssuedEvent>().Count(e => e == new CuiYanDamageIssuedEvent(f.Id, f.OwnerSeat, target)) != 1))
                throw new InvalidOperationException("Zhijie cannot repeat or replace its issued native damage.");
        }

        if (r.Stage == CuiYanStage.DamageChoice && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } choice && (choice.PlayerSeat != f.OwnerSeat || choice.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(choice.Choices, CuiYanDamageChoices(f))))
            throw new InvalidOperationException("Zhijie changed its public actual attack-range target choices.");
    }

    internal bool CuiYanDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => f.CuiYan is { Stage: CuiYanStage.DrawChildren } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == CuiYanDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 420)]
    internal bool IsCuiYanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.CuiYan is { Stage: CuiYanStage.DrawChildren } && effect?.Op == SkillProgramEffectOp.ResolveShutongFinalizedUse && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool CuiYanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.CuiYan is not { } r)
            return false;
        AssertCuiYan(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == CuiYanStage.DrawChildren && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => CuiYanDrawMovement(f, m));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? CuiYanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterCuiYanOlOperations.CuiYanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 930)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1040)]
    internal bool HasCuiYanDamageObserver(long id) => CuiYanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1070)]
    internal bool HasCuiYanBeforeDamageObserver(long id) => HasCuiYanDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 900)]
    internal bool IsCuiYanDying() => IsCuiYanOriginalDyingRide() || _coreDyingCursorRuntimeOperations.ActiveDying is { } d && CuiYanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27470)]
    internal bool AdvanceCuiYanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CuiYanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
