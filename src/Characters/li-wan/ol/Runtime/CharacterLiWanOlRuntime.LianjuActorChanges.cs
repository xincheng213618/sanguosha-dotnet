#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiWanOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 26720)]
    internal void FreezeLianjuActorChange(IGameEvent fact)
    {
        if (!HasLiWanCapability || fact is not ProgramCardUseActorReplacedEvent change || _coreCardUseLifecycleOperations.LifecycleCardUse(change.CardUseFrameId)is not { Action: { Type: CardActionType.Use } action } use)
            return;
        var originals = _coreProgramEventHistoryOperations.ProgramEventHistory<LianjuEntityUsedEvent>().Where(e => e.UseFrameId == use.Id && e.ActionId == action.ActionId).ToArray();
        if (originals.Length == 0)
            return;
        if (originals.Any(e => e.ActorSeat != originals[0].ActorSeat || LianjuFinalUseActor(e) != change.PreviousActorSeat) || change.ActorSeat != use.SourceSeat || change.ActorSeat != action.ActorSeat || change.ProviderSeat != action.ProviderSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == change.FrameId)is not { } producer || producer.SkillId != change.SkillId || producer.OwnerSeat != change.OwnerSeat || change.OwnerSeat != change.PreviousActorSeat || producer.InstructionIndex < 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(producer)?.Op != SkillProgramEffectOp.ReplaceCurrentCardUseActor || producer.SelectedTargetSeats is not [var target] || target != action.ActorSeat || producer.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context } window || context.ParentCardUseFrameId != use.Id || context.CardActionId != action.ActionId || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == window.ParentFrameId)is not { } parent || parent.ParentFrameId != use.Id || parent.Action.ActionId != action.ActionId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(producer, CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) || _coreProgramEventHistoryOperations.ProgramEventHistory<LianjuUseActorChangedEvent>().Any(e => e.UseFrameId == use.Id && e.ProducerFrameId == producer.Id && e.InstructionIndex == producer.InstructionIndex))
            throw new InvalidOperationException("Lianju cannot acknowledge an unrelated use, provider or native role-change producer.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianjuUseActorChangedEvent(use.Id, action.ActionId, producer.Id, LiWanSource(producer), producer.GameplayHash, producer.InstructionIndex, change.PreviousActorSeat, change.ActorSeat, action.ProviderSeat));
    }

    internal int LianjuFinalUseActor(LianjuEntityUsedEvent origin)
    {
        // A response's distinct action id never inherits changes to its parent Slash's actor.
        if (origin.ActionId is not { } actionId)
            return origin.ActorSeat;
        var actor = origin.ActorSeat;
        foreach (var proof in _coreProgramEventHistoryOperations.ProgramEventHistory<LianjuUseActorChangedEvent>().Where(e => e.UseFrameId == origin.UseFrameId && e.ActionId == actionId))
        {
            var source = proof.Producer;
            if (proof.PreviousActor != actor || source.OwnerSeat != actor || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(proof.Actor) || proof.Actor == actor || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program is not { } p || p.GameplayHash != proof.GameplayHash || ProgramInstructionResolver.Default.Find(p, ProgramInstructionSourceKind.Trigger, source.BindingId)is not { } plan || plan.Trigger?.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || proof.InstructionIndex < 1 || proof.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(proof.InstructionIndex).Effect.Op != SkillProgramEffectOp.ReplaceCurrentCardUseActor || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Count(e => e.CardUseFrameId == proof.UseFrameId && e.FrameId == proof.ProducerFrameId && e.SkillId == source.SkillId && e.OwnerSeat == source.OwnerSeat && e.PreviousActorSeat == proof.PreviousActor && e.ActorSeat == proof.Actor && e.ProviderSeat == proof.Provider) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == proof.ProducerFrameId && e.OwnerSeat == source.OwnerSeat && e.SkillId == source.SkillId && e.BindingId == source.BindingId && e.SkillInstanceId == source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == actionId && e.Action.Type == CardActionType.Use && e.Action.ActorSeat == origin.ActorSeat && e.Action.ProviderSeat == proof.Provider && e.Action.PhysicalCards.Any(c => c.CardId == origin.CardId)))
                throw new InvalidOperationException("Lianju lost its original accepted use or exact native actor-replacement chain.");
            actor = proof.Actor;
        }

        return actor;
    }
}
