#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualTurnLegacyCompletionRuntime
{
    internal CardUseFrame NormalizeActualTurnLegacyCompletedUse(CardUseFrame use, long? programParentId)
    {
        if (!(_coreCurrentTurnUseKindsOperations.TracksActualTurnUseOrdinal || _characterCaiYongLeOperations.HasLeCaiYongFeibaiCapability || _characterCaoZhiLeOperations.HasLeCaoZhiWenlanCapability || _characterKongShuOlOperations.HasKongShuCapability || _characterLiuYanOlOperations.HasLiuYanCapability || _characterCuiYanOlOperations.HasCuiYanCapability || _characterHuangYueyingOlMouOperations.HasMouHuangYueyingBingcaiCapability) || use.Action is not null)
            return use;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.Id != programParentId || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) || parent.InstructionIndex < 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LegacyActualUseCompletionCapturedEvent>().Any(e => e.CardUseFrameId == use.Id))
            throw new InvalidOperationException("Legacy actual-use completion lost its exact zero-entity program producer.");
        var source = new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Id != use.Id)?.Action?.ActionId, CardActionType.Use, use.SourceSeat, use.SourceSeat, null, null, null, use.CardKind, use.TargetSeats, [], [source], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use = use with { Action = action });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LegacyActualUseCompletionCapturedEvent(use.Id, use.SourceSeat, use.CardKind, parent.Id, source, parent.GameplayHash, parent.InstructionIndex, action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        return use;
    }

    internal bool IsExactLegacyActualUseCompletion(CardUseFrame use)
    {
        if (use.Action is not { Type: CardActionType.Use } action || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LegacyActualUseCompletionCapturedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (facts is not [var fact] || fact.ProgramParentFrameId != parent.Id || fact.ProducerGameplayHash != parent.GameplayHash || fact.ProducerInstructionIndex != parent.InstructionIndex || fact.ActorSeat != use.SourceSeat || fact.EffectiveKind != use.CardKind || fact.Action.ActionId != action.ActionId || action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var source] || source != fact.ProducerSource || source != new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || !action.TargetSeats.SequenceEqual(use.TargetSeats) || !fact.Action.TargetSeats.SequenceEqual(use.TargetSeats))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.ActorSeat == use.SourceSeat && e.Action.Type == CardActionType.Use && e.Action.EffectiveKind == use.CardKind && e.Action.PhysicalCards.Count == 0);
    }

    internal int? CapturedNormalizedLegacyActualTurnOrdinal(CardActionContext action)
    {
        if (action.Type != CardActionType.Use)
            return null;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Action?.ActionId == action.ActionId);
        if (use is null || use.Action?.ActorSeat != action.ActorSeat || !(IsExactLegacyActualUseCompletion(use) || _coreActualHandGainProgramsOperations.IsNormalizedLegacyCompletedCategoryUse(use, action.ActorSeat, action.EffectiveKind, action.TargetSeats)))
            return null;
        var facts = _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.OriginFrameId == use.Id && e.CardActionId is null).ToArray();
        if (facts is not [var fact] || fact.ActualTurnActorUseOrdinal is not > 0)
            throw new InvalidOperationException("A normalized legacy completion lost its positive declared ordinal.");
        return fact.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && fact.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && fact.ActorSeat == action.ActorSeat && !fact.IsNullificationUse && fact.CardCategoryMask == 1 << (int)CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) ? fact.ActualTurnActorUseOrdinal : null;
    }

    internal bool FinishActualTurnLegacyDyingAlcohol(long useId, long programParentId)
    {
        if (!(_coreCurrentTurnUseKindsOperations.TracksActualTurnUseOrdinal || _characterCaiYongLeOperations.HasLeCaiYongFeibaiCapability || _characterCaoZhiLeOperations.HasLeCaoZhiWenlanCapability || _characterKongShuOlOperations.HasKongShuCapability || _characterLiuYanOlOperations.HasLiuYanCapability || _characterCuiYanOlOperations.HasCuiYanCapability || _characterHuangYueyingOlMouOperations.HasMouHuangYueyingBingcaiCapability))
            return false;
        var use = NormalizeActualTurnLegacyCompletedUse(_coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Legacy Alcohol completion lost its owning use."), programParentId);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.Id != programParentId)
            throw new InvalidOperationException("Legacy Alcohol completion lost its exact suspended program parent.");
        if (use.CardKind != CardKind.Alcohol || use.Action is null || !IsExactLegacyActualUseCompletion(use))
            throw new InvalidOperationException("Legacy Alcohol completion lost its exact recovery producer.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { LegacyDyingAlcoholReturn = new(parent.Id, parent.InstructionIndex, new(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId)) });
        _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, new Card(0, CardKind.Alcohol, Suit.None, 0), CardKind.Alcohol);
        return true;
    }

    [NativeUseReturn(320)]
    internal void ReturnLegacyDyingAlcoholCompletion(CardUseFrame completed)
    {
        if (completed.LegacyDyingAlcoholReturn is not { } returned)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || parent.Id != returned.ProgramFrameId || parent.InstructionIndex != returned.InstructionIndex || parent.OwnerSeat != completed.SourceSeat || returned.ProducerSource != new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || completed is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, Action: not null })
            throw new InvalidOperationException("Legacy Alcohol completion lost its exact paid program return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
    }
}
