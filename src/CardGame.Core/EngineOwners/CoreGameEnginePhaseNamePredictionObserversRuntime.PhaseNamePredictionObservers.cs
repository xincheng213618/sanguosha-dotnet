#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseNamePredictionObserversRuntime
{
    internal bool PhaseNamePredictionFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.PhaseNamePrediction is not { Stage: PhaseNamePredictionStage.CostChildren or PhaseNamePredictionStage.ClaimChildren } r || root.PendingMovementContinuation is null || r.SequenceAfter <= r.SequenceBefore)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        var reason = r.Stage == PhaseNamePredictionStage.CostChildren ? "program.phase-name-prediction.cost" : "program.phase-name-prediction.claim";
        bool MovedEquipment(PhaseNamePredictionMaterial material, CardKind kind) => r.Selected.Contains(material.CardId) && material.From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(material.CardId).Kind == kind && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == material.CardId && m.From == material.From && m.Reason.Value == reason);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.To == CardLocation.DiscardPile && r.Eligible.Any(material => MovedEquipment(material, CardKind.WoodenOx) && m.From == CardLocation.WoodenOxGrain(material.From.OwnerSeat!.Value))));
        var lionOwner = r.Eligible.Where(material => MovedEquipment(material, CardKind.SilverLion)).Select(material => material.From.OwnerSeat).FirstOrDefault();
        if (lionOwner is not { } armorOwner)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == armorOwner && hp.Change.TargetSeat == armorOwner && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == armorOwner && recovery.Attempt.TargetSeat == armorOwner && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason?.Value == reason;
    }

    internal ProgramSkillFrame? PhaseNamePredictionObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.PhaseNamePredictionFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasHandGainDebtObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 70)]
    internal bool TryAdvancePhaseNamePredictionSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || PhaseNamePredictionObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
