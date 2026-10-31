#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYanOlRuntime
{
    internal bool IsLiuYanLegacyAlcoholUse(long id)
    {
        if (!HasLiuYanCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, LiuYanLegacyAlcohol: { } r, Action: { Type: CardActionType.Use } a } u || r.UseId != id || a.ActionId != r.ActionId || !Enum.IsDefined(r.Stage) || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(u) || !u.TargetSeats.SequenceEqual([u.SourceSeat]) || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame p || p.Id != r.ProgramParentId || p.InstructionIndex != r.InstructionIndex || p.GameplayHash != r.ProducerHash || p.WindowContext?.ParentFrameId != r.DyingId || p.OwnerSeat != u.SourceSeat || r.ProducerSource != new CardConversionSource(p.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p), p.OwnerSeat, p.SkillInstanceId) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)?.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuYanLegacyAlcoholWindowIssuedEvent>().Count(e => e == new LiuYanLegacyAlcoholWindowIssuedEvent(id, r.ProgramParentId, r.DyingId, r.InstructionIndex, r.ProducerSource, r.ProducerHash, r.ActionId)) != 1)
            return false;
        return true;
    }

    [NativeUseValidation(27232)]
    internal void ValidateLiuYanLegacyAlcohol(CardUseFrame use)
    {
        if (use.LiuYanLegacyAlcohol is not null && !IsLiuYanLegacyAlcoholUse(use.Id))
            throw new InvalidOperationException("Tushe's legacy Wine lost its exact native producer and return.");
    }

    internal bool TryBeginLiuYanLegacyAlcoholUse(long id, long parentId, long dyingId)
    {
        if (!HasLiuYanCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: null, CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } u || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat], "ol:tushe") || !LiuYanNoBasicHand(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat]))
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (i < 1 || i != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame p || p.Id != parentId || p.OwnerSeat != u.SourceSeat || p.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dyingId || _coreDyingCursorRuntimeOperations.Value.ActiveDying?.FrameId != dyingId || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(u))
            throw new InvalidOperationException("Tushe's legacy Wine requires its genuine accepted self-rescue producer.");
        u = _coreActualTurnLegacyCompletionOperations.NormalizeActualTurnLegacyCompletedUse(u, parentId);
        var r = new LiuYanLegacyAlcoholReturn(id, parentId, dyingId, p.InstructionIndex, new(p.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p), p.OwnerSeat, p.SkillInstanceId), p.GameplayHash, u.Action!.ActionId, LiuYanLegacyAlcoholStage.Targets);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { LiuYanLegacyAlcohol = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanLegacyAlcoholWindowIssuedEvent(id, parentId, dyingId, p.InstructionIndex, r.ProducerSource, r.ProducerHash, r.ActionId));
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, u.Action, SkillProgramTriggerWindow.CardUseTargetsFinalized, [u.SourceSeat], ProgramCardContinuation.FinalizedSimpleCard))
            return true;
        ContinueLiuYanLegacyAlcohol(id);
        return true;
    }

    internal void ContinueLiuYanLegacyAlcohol(long id)
    {
        if (!IsLiuYanLegacyAlcoholUse(id) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Tushe's legacy Wine lost its exact native target/commit return.");
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var r = u.LiuYanLegacyAlcohol!;
        if (r.Stage == LiuYanLegacyAlcoholStage.Targets)
        {
            r = r with
            {
                Stage = LiuYanLegacyAlcoholStage.Committed
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { LiuYanLegacyAlcohol = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, u.Action!, SkillProgramTriggerWindow.CardUseCommitted, [u.SourceSeat], ProgramCardContinuation.CommittedSimpleCard))
                return;
        }

        r = r with
        {
            Stage = LiuYanLegacyAlcoholStage.Recovery
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { LiuYanLegacyAlcohol = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
        _coreDyingProgramsOperations.ResumeProgramVirtualDyingAlcoholRecovery(id, r.ProgramParentId, r.DyingId);
    }

    internal bool TryReturnLiuYanLegacyAlcohol(ProgramCardTriggerWindowFrame w)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)?.LiuYanLegacyAlcohol is not { } r)
            return false;
        if (!IsLiuYanLegacyAlcoholUse(w.ParentFrameId) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != w.ParentFrameId || w.SimpleContinuation is not null || w.AttackOwnerFrameId is not null || w.CompletedResponseReturn is not null || w.Action.ActionId != r.ActionId || w.Continuation != (r.Stage == LiuYanLegacyAlcoholStage.Targets ? ProgramCardContinuation.FinalizedSimpleCard : ProgramCardContinuation.CommittedSimpleCard) || r.Stage == LiuYanLegacyAlcoholStage.Recovery)
            throw new InvalidOperationException("Tushe's legacy Wine cannot replay a target or commit return.");
        ContinueLiuYanLegacyAlcohol(w.ParentFrameId);
        return true;
    }
}
