#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFangMobileRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 82300)]
    internal void AssertMobileDuanfa(ProgramSkillFrame f)
    {
        if (f.MobileDuanfa is not { } r)
            return;
        if (f.SkillId != MobileDuanfaId || f.ActivationId != "phase-black-recycle" || f.InstructionIndex != 1 || f.WindowContext is not null || r.Source != MobileDuanfaSource(f) || r.Hash != f.GameplayHash || r.Phase.PhaseActorSeat != f.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Cards.Distinct().Count() != r.Cards.Count || r.ActualCount < 0 || r.ActualCount > r.Cards.Count || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileDuanfaIssuedEvent>().Count(e => e == new MobileDuanfaIssuedEvent(f.Id, r.Source, r.Hash, r.Phase)) != 1)
            throw new InvalidOperationException("Mobile Duanfa lost its exact original source, actual Play phase or payment cursor.");
        if (r.Stage == MobileDuanfaStage.Cards)
            return;
        if (r.Cards.Count == 0 || r.Limit < 1 || r.SpentBefore < 0 || r.SpentBefore + r.Cards.Count > r.Limit || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileDuanfaPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid != new MobileDuanfaPaidEvent(f.Id, r.Source, r.Hash, r.Phase, r.Limit, r.SpentBefore, r.ActualCount, r.Before, r.After) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => r.Cards.Contains(m.CardId) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == MobileDuanfaDiscard && m.From.OwnerSeat == f.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame)) != r.ActualCount)
            throw new InvalidOperationException("Duanfa lost its exact paid black HE discard and phase budget invoice.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 82300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 82300)]
    internal bool KeepPaidMobileDuanfa(ProgramSkillFrame f)
    {
        if (f.MobileDuanfa is not { Stage: not MobileDuanfaStage.Cards, ActualCount: > 0 })
            return false;
        AssertMobileDuanfa(f);
        return true;
    }

    internal bool MobileDuanfaObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileDuanfa is not { Stage: MobileDuanfaStage.CostChildren } r)
            return false;
        AssertMobileDuanfa(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && r.Cards.Contains(m.CardId));
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.Kind == HpChangeKind.Recovery;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MobileDuanfaDiscard;
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }

    internal ProgramSkillFrame? MobileDuanfaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhouFangMobileOperations.MobileDuanfaObserverChild(f, child), (index) => _coreObserverSubtreesOperations2.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 82300)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 82300)]
    internal bool MobileDuanfaDyingProof() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MobileDuanfaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 82300)]
    internal bool AdvanceMobileDuanfaObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileDuanfaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
