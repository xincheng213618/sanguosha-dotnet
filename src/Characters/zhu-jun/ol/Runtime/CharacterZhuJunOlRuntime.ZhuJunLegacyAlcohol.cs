#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuJunOlRuntime
{
    internal bool IsZhuJunLegacyAlcoholPreRecoveryRide()
    {
        if (!HasZhuJunCapability || _coreDyingCursorRuntimeOperations.ActiveDying is not { } active)
            return false;
        var dyingIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == active.FrameId);
        if (dyingIndex < 0 || dyingIndex + 3 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[dyingIndex] is not DyingFrame dying || dying.ResponderIndex < 0 || dying.ResponderIndex >= dying.ResponderSeats.Count || _resolutionStackOperations._resolutionStack[dyingIndex + 1] is not ProgramSkillFrame producer || producer.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dying.Id || context.TargetSeat != dying.VictimSeat || context.OwnerSeat != producer.OwnerSeat || context.SourceSeat != (_coreNativeDamageHandlesOperations.GetDyingAttack(dying)is { IsSourceLess: false } originalAttack ? originalAttack.SourceSeat : (int? )null) || context.DamageFrameId != dying.DamageFrameId || producer.OwnerSeat != dying.VictimSeat || producer.OwnerSeat != dying.ResponderSeat || producer.TriggerId is null || producer.ActivationId != producer.TriggerId || _contentRegistryOperations._contentRegistry.GetSkill(producer.SkillId).Program is not { } definition || definition.GameplayHash != producer.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(producer)?.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || _resolutionStackOperations._resolutionStack[dyingIndex + 2] is not CardUseFrame { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, Action: { } action, ZhuJunLegacyAlcohol: { Returned: false } r } use || use.SourceSeat != dying.VictimSeat || !use.TargetSeats.SequenceEqual([dying.VictimSeat]) || use.DyingResponse is not null || use.VirtualBasicReturn is not null || r.UseId != use.Id || r.ActionId != action.ActionId || r.ProgramParentId != producer.Id || r.DyingId != dying.Id || r.InstructionIndex != producer.InstructionIndex || r.ProducerGameplayHash != producer.GameplayHash || r.ProducerSource != new CardConversionSource(producer.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(producer), producer.OwnerSeat, producer.SkillInstanceId) || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunLegacyAlcoholWindowIssuedEvent>().Count(e => e.Return == r) != 1 || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == producer.Id && e.OwnerSeat == producer.OwnerSeat && e.SkillId == producer.SkillId && e.SkillInstanceId == producer.SkillInstanceId && e.BindingId == producer.TriggerId && e.Window == context.Window) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == dying.VictimSeat && e.KillerSeat == dying.KillerSeat) != 1 || _resolutionStackOperations._resolutionStack[dyingIndex + 3] is not ProgramCardTriggerWindowFrame { Continuation: ProgramCardContinuation.CommittedSimpleCard, SimpleContinuation: null, TrickContinuation: null, CompletedResponseReturn: null, AttackOwnerFrameId: null } window || window.ParentFrameId != use.Id || window.Action.ActionId != action.ActionId || window.Action.ActorSeat != use.SourceSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } candidate || candidate.FrozenContext is not { } frozen || frozen.ParentFrameId != window.Id || frozen.CardUse?.ParentCardUseFrameId != use.Id || ZhuJunQualification(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate), frozen)is null)
            return false;
        if (dyingIndex + 4 == _resolutionStackOperations._resolutionStack.Count)
            return true;
        if (_resolutionStackOperations._resolutionStack[dyingIndex + 4] is not ProgramSkillFrame { ZhuJunCuipoDraw: not null } draw || draw.WindowContext != frozen || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(draw, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate)))
            return false;
        AssertZhuJunCuipo(draw);
        return dyingIndex + 5 == _resolutionStackOperations._resolutionStack.Count || ZhuJunObserverRoot()?.Id == draw.Id;
    }

    internal bool TryBeginZhuJunLegacyAlcoholUse(long useId, long programParentId, long dyingId)
    {
        if (!HasZhuJunCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: null, CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } use || !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoQualifiedEvent>().Any(e => e.OriginFrameId == use.Id && e.ActionId is null && !e.Damage && e.EffectiveKind == CardKind.Alcohol && e.Source.OwnerSeat == use.SourceSeat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], e.Source.SkillId, e.Source.SkillInstanceId)))
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == useId);
        if (at < 1 || at != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame parent || parent.Id != programParentId || parent.OwnerSeat != use.SourceSeat || parent.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dyingId || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dyingId || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            throw new InvalidOperationException("Cuipo legacy Alcohol lost its actual declared zero-entity dying producer.");
        use = _coreActualTurnLegacyCompletionOperations.NormalizeActualTurnLegacyCompletedUse(use, parent.Id);
        var returned = new ZhuJunLegacyAlcoholReturn(use.Id, parent.Id, dyingId, parent.InstructionIndex, new(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId), parent.GameplayHash, use.Action!.ActionId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZhuJunLegacyAlcohol = returned });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuJunLegacyAlcoholWindowIssuedEvent(returned));
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, use.Action, SkillProgramTriggerWindow.CardUseCommitted, [use.SourceSeat], ProgramCardContinuation.CommittedSimpleCard))
            return true;
        // A rejected/empty collection must never leave a callback or a pending
        // recovery. The original caller proceeds to its ordinary recovery.
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZhuJunLegacyAlcohol = null });
        return false;
    }

    internal bool TryReturnZhuJunLegacyAlcohol(ProgramCardTriggerWindowFrame frame)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(frame.ParentFrameId)is not { ZhuJunLegacyAlcohol: { Returned: false } r } use || frame.Continuation != ProgramCardContinuation.CommittedSimpleCard || frame.SimpleContinuation is not null)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || r.UseId != use.Id || use.CardId != 0 || use.CardKind != CardKind.Alcohol || use.PhysicalCardIds is not { Count: 0 } || frame.Action.ActionId != r.ActionId || use.Action?.ActionId != r.ActionId || frame.Action.ActorSeat != use.SourceSeat || frame.AttackOwnerFrameId is not null || frame.CompletedResponseReturn is not null || frame.TrickContinuation is not null || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramParentId)is not ProgramSkillFrame parent || parent.InstructionIndex != r.InstructionIndex || parent.GameplayHash != r.ProducerGameplayHash || r.ProducerSource != new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || parent.WindowContext?.ParentFrameId != r.DyingId || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunLegacyAlcoholWindowIssuedEvent>().Count(e => e.Return == r) != 1 || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || !frame.Candidates.Any(c => c.SkillId == "ol:cuipo" && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoDrawCompletedEvent>().Any(e => e.OriginFrameId == use.Id && e.ActionId is null)))
            throw new InvalidOperationException("Cuipo legacy Alcohol lost its exact completed draw and original native recovery return.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZhuJunLegacyAlcohol = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuJunLegacyAlcoholReturnedEvent(use.Id, frame.Id, r.ProgramParentId, r.DyingId));
        _coreDyingProgramsOperations.ResumeProgramVirtualDyingAlcoholRecovery(use.Id, r.ProgramParentId, r.DyingId);
        return true;
    }
}
