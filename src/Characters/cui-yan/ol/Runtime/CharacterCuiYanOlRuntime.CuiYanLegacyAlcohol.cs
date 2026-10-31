#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    internal bool IsCuiYanLegacyAlcoholUse(long id)
    {
        if (!HasCuiYanCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, CuiYanLegacyAlcohol: { } r, Action: { Type: CardActionType.Use } a } u || r.UseId != id || a.ActionId != r.ActionId || !Enum.IsDefined(r.Stage) || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(u) || !u.TargetSeats.SequenceEqual([u.SourceSeat]) || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame p || p.Id != r.ProgramParentId || p.InstructionIndex != r.InstructionIndex || p.GameplayHash != r.ProducerHash || p.WindowContext?.ParentFrameId != r.DyingId || p.OwnerSeat != u.SourceSeat || r.ProducerSource != new CardConversionSource(p.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p), p.OwnerSeat, p.SkillInstanceId) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)?.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanLegacyAlcoholWindowIssuedEvent>().Count(e => e == new CuiYanLegacyAlcoholWindowIssuedEvent(id, r.ProgramParentId, r.DyingId, r.InstructionIndex, r.ProducerSource, r.ProducerHash, r.ActionId)) != 1)
            return false;
        return true;
    }

    [NativeUseValidation(27471)]
    internal void ValidateCuiYanLegacyAlcohol(CardUseFrame use)
    {
        if (use.CuiYanLegacyAlcohol is not null && !IsCuiYanLegacyAlcoholUse(use.Id))
            throw new InvalidOperationException("Shutong's legacy Wine lost its exact native producer and return.");
    }

    internal bool TryBeginCuiYanLegacyAlcoholUse(long id, long parentId, long dyingId)
    {
        if (!HasCuiYanCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: null, CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } u || !_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseTargetsFinalized).Any(b => b.Trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShutongFinalizedUse)))
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (i < 1 || i != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame p || p.Id != parentId || p.OwnerSeat != u.SourceSeat || p.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dyingId || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dyingId || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(u))
            throw new InvalidOperationException("Shutong's legacy Wine requires its genuine accepted self-rescue producer.");
        u = _coreActualTurnLegacyCompletionOperations.NormalizeActualTurnLegacyCompletedUse(u, parentId);
        var r = new CuiYanLegacyAlcoholReturn(id, parentId, dyingId, p.InstructionIndex, new(p.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p), p.OwnerSeat, p.SkillInstanceId), p.GameplayHash, u.Action!.ActionId, CuiYanLegacyAlcoholStage.Targets);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { CuiYanLegacyAlcohol = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanLegacyAlcoholWindowIssuedEvent(id, parentId, dyingId, p.InstructionIndex, r.ProducerSource, r.ProducerHash, r.ActionId));
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, u.Action, SkillProgramTriggerWindow.CardUseTargetsFinalized, [u.SourceSeat], ProgramCardContinuation.FinalizedSimpleCard))
            return true;
        ContinueCuiYanLegacyAlcohol(id);
        return true;
    }

    internal void ContinueCuiYanLegacyAlcohol(long id)
    {
        if (!IsCuiYanLegacyAlcoholUse(id) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Shutong's legacy Wine lost its exact native target/commit return.");
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var r = u.CuiYanLegacyAlcohol!;
        if (r.Stage == CuiYanLegacyAlcoholStage.Targets)
        {
            r = r with
            {
                Stage = CuiYanLegacyAlcoholStage.Committed
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { CuiYanLegacyAlcohol = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, u.Action!, SkillProgramTriggerWindow.CardUseCommitted, [u.SourceSeat], ProgramCardContinuation.CommittedSimpleCard))
                return;
        }

        r = r with
        {
            Stage = CuiYanLegacyAlcoholStage.Recovery
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { CuiYanLegacyAlcohol = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
        _coreDyingProgramsOperations.ResumeProgramVirtualDyingAlcoholRecovery(id, r.ProgramParentId, r.DyingId);
    }

    internal bool TryReturnCuiYanLegacyAlcohol(ProgramCardTriggerWindowFrame w)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)?.CuiYanLegacyAlcohol is not { } r)
            return false;
        if (!IsCuiYanLegacyAlcoholUse(w.ParentFrameId) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != w.ParentFrameId || w.SimpleContinuation is not null || w.AttackOwnerFrameId is not null || w.CompletedResponseReturn is not null || w.Action.ActionId != r.ActionId || w.Continuation != (r.Stage == CuiYanLegacyAlcoholStage.Targets ? ProgramCardContinuation.FinalizedSimpleCard : ProgramCardContinuation.CommittedSimpleCard) || r.Stage == CuiYanLegacyAlcoholStage.Recovery)
            throw new InvalidOperationException("Shutong's legacy Wine cannot replay a target or commit return.");
        ContinueCuiYanLegacyAlcohol(w.ParentFrameId);
        return true;
    }
}
