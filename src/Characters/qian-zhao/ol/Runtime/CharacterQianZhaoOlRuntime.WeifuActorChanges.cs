#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 26622)]
    internal void FreezeWeifuActorChange(IGameEvent fact)
    {
        if (fact is not ProgramCardUseActorReplacedEvent change || _coreCardUseLifecycleOperations.LifecycleCardUse(change.CardUseFrameId)is not { WeifuUse: { } r, Action: { } action } use)
            return;
        if (action.ActionId != r.ActionId || change.ActorSeat != use.SourceSeat || change.ActorSeat != action.ActorSeat || change.ProviderSeat != r.ProviderSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == change.FrameId)is not { } producer || producer.SkillId != change.SkillId || producer.OwnerSeat != change.OwnerSeat || change.OwnerSeat != change.PreviousActorSeat || producer.InstructionIndex < 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(producer)?.Op != SkillProgramEffectOp.ReplaceCurrentCardUseActor || producer.SelectedTargetSeats is not [var target] || target != action.ActorSeat || producer.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context } window || context.ParentCardUseFrameId != use.Id || context.CardActionId != action.ActionId || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == window.ParentFrameId)is not { } native || native.ParentFrameId != use.Id || native.Action.ActionId != action.ActionId || native.CandidateIndex < 0 || native.CandidateIndex >= native.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(producer, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(native.Candidates[native.CandidateIndex])) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuActorChangedEvent>().Any(e => e.UseId == use.Id && e.ProgramFrameId == producer.Id && e.InstructionIndex == producer.InstructionIndex))
            throw new InvalidOperationException("Weifu cannot acknowledge an unrelated native actor replacement.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuActorChangedEvent(use.Id, action.ActionId, producer.Id, QianZhaoSource(producer), producer.GameplayHash, producer.InstructionIndex, change.PreviousActorSeat, change.ActorSeat, change.ProviderSeat));
    }

    internal bool WeifuCurrentActorMatches(CardUseFrame use, WeifuUseReceipt r)
    {
        if (use.Action is not { Type: CardActionType.Use } action || action.ActionId != r.ActionId || action.ProviderSeat != r.ProviderSeat || action.ActorSeat != use.SourceSeat)
            return false;
        var changes = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        var proofs = _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuActorChangedEvent>().Where(e => e.UseId == use.Id).ToArray();
        if (changes.Length != proofs.Length)
            return false;
        if (changes.Length == 0)
            return action.ActorSeat == r.ActorSeat;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == r.ActorSeat && e.CardId == use.CardId) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.Type == CardActionType.Use && e.Action.ActorSeat == r.ActorSeat && e.Action.ProviderSeat == r.ProviderSeat && e.Action.PhysicalCards.SequenceEqual(action.PhysicalCards)))
            return false;
        var actor = r.ActorSeat;
        for (var i = 0; i < changes.Length; i++)
        {
            var change = changes[i];
            var proof = proofs[i];
            var source = proof.Producer;
            if (change.FrameId != proof.ProgramFrameId || proof.ActionId != action.ActionId || proof.PreviousActor != actor || change.PreviousActorSeat != actor || change.OwnerSeat != actor || source.OwnerSeat != actor || change.SkillId != source.SkillId || change.ActorSeat != proof.Actor || change.ProviderSeat != proof.Provider || proof.Provider != r.ProviderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(proof.Actor) || proof.Actor == actor || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program is not { } p || p.GameplayHash != proof.GameplayHash || ProgramInstructionResolver.Default.Find(p, ProgramInstructionSourceKind.Trigger, source.BindingId)is not { } plan || plan.Trigger?.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || proof.InstructionIndex < 1 || proof.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(proof.InstructionIndex).Effect.Op != SkillProgramEffectOp.ReplaceCurrentCardUseActor || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == proof.ProgramFrameId && e.SkillId == source.SkillId && e.OwnerSeat == actor && e.SkillInstanceId == source.SkillInstanceId && e.BindingId == source.BindingId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) != 1)
                return false;
            actor = proof.Actor;
        }

        return actor == action.ActorSeat;
    }
}
