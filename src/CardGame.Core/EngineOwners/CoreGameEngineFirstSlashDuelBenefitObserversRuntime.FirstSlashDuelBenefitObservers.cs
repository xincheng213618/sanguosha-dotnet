#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitObserversRuntime
{
    internal bool ValidFirstSlashDuelBenefit(ProgramSkillFrame f)
    {
        if (f.FirstSlashDuelBenefit is not { } r || r.InstructionIndex != f.InstructionIndex || f.TriggerId is null || r.Source != _coreFirstSlashDuelBenefitsOperations.FirstSlashDuelSource(f) || r.GameplayHash != f.GameplayHash || r.FrozenDiamondCount < 0 || r.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.WindowContext?.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || f.WindowContext.ParentFrameId != r.WindowFrameId || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ChooseFirstSlashDuelDiamondBenefit)
            return false;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowFrameId);
        var legacy = _resolutionStackOperations._resolutionStack.OfType<FirstSlashDuelLegacyWindowFrame>().SingleOrDefault(w => w.Id == r.WindowFrameId);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId);
        var candidateMatches = window is not null && r.LegacyProducerFrameId is null && window.ParentFrameId == use?.Id && window.Action.ActionId == r.ActionId && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || legacy is not null && use is not null && r.ActionId is null && r.LegacyProducerFrameId == legacy.ProducerFrameId && legacy.ParentFrameId == use.Id && legacy.CandidateIndex >= 0 && legacy.CandidateIndex < legacy.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, legacy.Candidates[legacy.CandidateIndex]) && _coreFirstSlashDuelLegacyWindowsOperations.IsFirstLegacySlashUse(use, legacy.ProducerFrameId, requireSingle: r.SelectedBranch is null);
        if (!candidateMatches || use is null || use.Action?.ActionId != r.ActionId || use.SourceSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime.IsFirstSlashDuelKind(use.CardKind) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstSlashDuelUseRecordedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ActionId == r.ActionId && e.Ordinal == 1 && e.ActorSeat == f.OwnerSeat && e.TurnNumber == r.TurnNumber && e.TurnOwnerSeat == r.TurnOwnerSeat))
            return false;
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstSlashDuelBenefitIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (r.SelectedBranch is null)
            return issued.Length == 0 && use.TargetSeats.SequenceEqual([r.TargetSeat]);
        if (r.SelectedBranch != "draw" || issued is not [var fact] || fact.Receipt != r || r.DrawCount < 0 || r.DrawCount > r.FrozenDiamondCount || r.MovementBefore < 0 || r.MovementAfter < r.MovementBefore || r.MovementAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat)
            return false;
        var draws = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.first-slash-duel.draw").ToArray();
        return draws.Length == r.DrawCount;
    }

    internal void RequireFirstSlashDuelBenefit(ProgramSkillFrame f)
    {
        if (!ValidFirstSlashDuelBenefit(f))
            throw new InvalidOperationException("The first Slash/Duel benefit lost its exact declared use, candidate, source or once-issued draw.");
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 80)]
    internal void AssertRegisteredFirstSlashDuelBenefit(ProgramSkillFrame frame)
    {
        if (frame.FirstSlashDuelBenefit is not null)
            RequireFirstSlashDuelBenefit(frame);
    }

    internal bool FirstSlashDuelFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!_coreFirstSlashDuelBenefitsOperations.CanContinueFirstSlashDuelBenefit(root) || root.FirstSlashDuelBenefit is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == "skill-program.first-slash-duel.draw");
    }

    internal ProgramSkillFrame? FirstSlashDuelObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreFirstSlashDuelBenefitObserversOperations.FirstSlashDuelFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1590)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 130)]
    internal bool HasFirstSlashDuelDamageObserver(long id) => FirstSlashDuelObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10, group: 140)]
    internal bool IsFirstSlashDuelDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && FirstSlashDuelObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 100)]
    internal bool TryAdvanceFirstSlashDuelSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FirstSlashDuelObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
