#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuJunOlRuntime
{

    internal bool TryBeginZhuJunLegacyAlcoholUse(long useId, long programParentId, long dyingId)
    {
        if (!HasZhuJunCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: null, CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } use || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhuJunCuipoQualifiedEvent>().Any(e => e.OriginFrameId == use.Id && e.ActionId is null && !e.Damage && e.EffectiveKind == CardKind.Alcohol && e.Source.OwnerSeat == use.SourceSeat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], e.Source.SkillId, e.Source.SkillInstanceId)))
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == useId);
        if (at < 1 || at != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame parent || parent.Id != programParentId || parent.OwnerSeat != use.SourceSeat || parent.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dyingId || _coreDyingCursorRuntimeOperations.Value.ActiveDying?.FrameId != dyingId || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            throw new InvalidOperationException("Cuipo legacy Alcohol lost its actual declared zero-entity dying producer.");
        use = _coreActualTurnLegacyCompletionOperations.NormalizeActualTurnLegacyCompletedUse(use, parent.Id);
        var returned = new ZhuJunLegacyAlcoholReturn(use.Id, parent.Id, dyingId, parent.InstructionIndex, new(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId), parent.GameplayHash, use.Action!.ActionId);
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
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || r.UseId != use.Id || use.CardId != 0 || use.CardKind != CardKind.Alcohol || use.PhysicalCardIds is not { Count: 0 } || frame.Action.ActionId != r.ActionId || use.Action?.ActionId != r.ActionId || frame.Action.ActorSeat != use.SourceSeat || frame.AttackOwnerFrameId is not null || frame.CompletedResponseReturn is not null || frame.TrickContinuation is not null || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramParentId)is not ProgramSkillFrame parent || parent.InstructionIndex != r.InstructionIndex || parent.GameplayHash != r.ProducerGameplayHash || r.ProducerSource != new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || parent.WindowContext?.ParentFrameId != r.DyingId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhuJunLegacyAlcoholWindowIssuedEvent>().Count(e => e.Return == r) != 1 || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || !frame.Candidates.Any(c => c.SkillId == "ol:cuipo" && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhuJunCuipoDrawCompletedEvent>().Any(e => e.OriginFrameId == use.Id && e.ActionId is null)))
            throw new InvalidOperationException("Cuipo legacy Alcohol lost its exact completed draw and original native recovery return.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZhuJunLegacyAlcohol = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuJunLegacyAlcoholReturnedEvent(use.Id, frame.Id, r.ProgramParentId, r.DyingId));
        _coreDyingProgramsOperations.ResumeProgramVirtualDyingAlcoholRecovery(use.Id, r.ProgramParentId, r.DyingId);
        return true;
    }
}
