#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSoleTargetProtectionObserversRuntime
{
    internal bool SoleTargetProtectionFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!_coreSoleTargetProtectionOperations.Value.HasExactSoleProtectionRoot(f) || f.SoleTargetProtection is not { Cost: not null } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.ResumeProgramFrameId is not null || moved.Id != moved.Batch.Id || moved.Batch.ParentFrameId != f.Id || moved.Batch.AwaitingProgramFrameId != f.Id || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (r.Stage switch
            {
                SoleTargetProtectionStage.CostChildren => m.Sequence > r.CostSequenceBefore && m.Sequence <= r.CostSequenceAfter && m.CardId == r.Cost.CardId && m.From == r.Cost.From && m.Reason.Value == "skill-program.sole-target-protection.cost",
                SoleTargetProtectionStage.DrawChildren => m.Sequence > r.RewardSequenceBefore && m.Sequence <= r.RewardSequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.sole-target-protection.draw",
                SoleTargetProtectionStage.QuarantineChildren => _coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId)?.SoleTargetQuarantine is { } q && m.Sequence > q.QuarantineSequenceBefore && m.Sequence <= q.QuarantineSequenceAfter && m.From == CardLocation.Processing && m.To == CardLocation.OutsideGame && m.Reason.Value == "skill-program.sole-target-protection.quarantine" && q.Materials.Any(x => x.CardId == m.CardId),
                _ => false
            }));
        }

        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == SoleTargetProtectionStage.CostChildren && r.Cost.Kind == CardKind.SilverLion && r.Cost.From == CardLocation.Equipment(f.OwnerSeat) && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
    }

    internal ProgramSkillFrame? SoleTargetProtectionObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.SoleTargetProtectionFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 40)]
    internal bool TryAdvanceSoleTargetProtectionSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SoleTargetProtectionObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
