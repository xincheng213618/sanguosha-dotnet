#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramCardTargetEffectsRuntime
{
    internal void NullifySelectedProgramCardEffects(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseBeforeTargetEffects, CardUse: { } cardUse } context || active.SelectedTargetSeats.Count == 0 || active.SelectedTargetSeats.Distinct().Count() != active.SelectedTargetSeats.Count)
            throw new InvalidOperationException("Selected card-effect nullification requires a frozen nonempty target set.");
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is not { } window || window.Id != context.ParentFrameId || window.Action.ActionId != cardUse.CardActionId || window.ParentFrameId != cardUse.ParentCardUseFrameId || window.Action.Type != CardActionType.Use || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(item => item.Id == window.ParentFrameId)is not { } parent || parent.Action?.ActionId != cardUse.CardActionId || parent.Action.EffectiveKind != cardUse.EffectiveKind || parent.Action.ActorSeat != cardUse.ActorSeat)
            throw new InvalidOperationException("Selected card-effect nullification lost its frozen parent card action.");
        var designated = (cardUse.DesignatedTargetSeats ?? parent.Action.EffectiveDesignatedTargetSeats).ToHashSet();
        if (active.SelectedTargetSeats.Any(seat => !designated.Contains(seat)))
            throw new InvalidOperationException("A selected seat was never a target of the current card use.");
        var newlyNullified = active.SelectedTargetSeats.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && parent.IneffectiveTargetSeats?.Contains(seat) != true).ToArray();
        foreach (var seat in newlyNullified)
            MarkCardEffectIneffective(parent.Id, seat);
        if (newlyNullified.Length == 0)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSelectedCardEffectsNullifiedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, cardUse.ActorSeat, parent.Id, cardUse.EffectiveKind, newlyNullified));
        foreach (var seat in newlyNullified)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(parent.Id, cardUse.ActorSeat, seat, cardUse.EffectiveKind, CardEffectSkipReason.SkillNullified));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，" + $"令【{(CardCatalog.Get(cardUse.EffectiveKind).DisplayName)}】对" + $"{(string.Join("、", newlyNullified.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)))}无效。", active.OwnerSeat, cardUse.ActorSeat);
    }

    internal void NullifyCurrentProgramCardEffect(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var context = active.WindowContext;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseBeforeTargetEffects, CardUse: { } cardUse, TargetSeat: var targetSeat } || targetSeat != active.OwnerSeat)
            throw new InvalidOperationException("Current card-effect nullification requires the target owner's finalized-target or before-target-effects context.");
        var cardUseIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(item => item.Id == cardUse.ParentCardUseFrameId && item is CardUseFrame);
        if (cardUseIndex < 0 || _resolutionStackOperations._resolutionStack[cardUseIndex] is not CardUseFrame parent || parent.Action?.ActionId != cardUse.CardActionId || !parent.TargetSeats.Contains(active.OwnerSeat) || parent.Action.ActorSeat != cardUse.ActorSeat || parent.Action.EffectiveKind != cardUse.EffectiveKind)
            throw new InvalidOperationException("Current card-effect nullification lost its frozen parent card action.");
        MarkCardEffectIneffective(parent.Id, active.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardEffectNullifiedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, cardUse.ActorSeat, parent.Id, cardUse.EffectiveKind));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(parent.Id, cardUse.ActorSeat, active.OwnerSeat, cardUse.EffectiveKind, CardEffectSkipReason.SkillNullified));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，" + $"令【{(CardCatalog.Get(cardUse.EffectiveKind).DisplayName)}】对自己无效。", active.OwnerSeat, cardUse.ActorSeat);
    }

    internal void MarkCardEffectIneffective(long resolutionId, int targetSeat)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == resolutionId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame cardUse)
            throw new InvalidOperationException("Card-effect nullification requires an active CardUse frame.");
        var seats = (cardUse.IneffectiveTargetSeats ?? []).Append(targetSeat).Distinct().Order().ToArray();
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, cardUse with { IneffectiveTargetSeats = seats });
    }

    internal bool IsCardEffectIneffective(long resolutionId, int targetSeat)
    {
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == resolutionId);
        if (_coreProgramCardUseNullificationOperations.Value.IsProgramUseIneffective(frame) || _coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(frame) || _characterXinPiOlOperations.IsXinPiUseIneffective(frame) || _characterZhouQunOlOperations.Value.IsNingwuUseIneffective(frame) || _characterZhangXiuMouOlOperations.Value.IsMouZhangXiuOlQiulieIneffective(frame))
            return true;
        if (frame.IneffectiveTargetSeats?.Contains(targetSeat) == true)
            return true;
        if (frame.Action is not { } action || !GrantTurnCardEffectImmunityProgramOperationDescriptor.CardEffectImmunityKinds.Contains(action.EffectiveKind) || _turnCardUseEffectsOperations._turnCardUseEffects.GetRuleModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, targetSeat, SkillRuleQuery.CardEffectImmunity, action.EffectiveKind).Count == 0)
            return false;
        MarkCardEffectIneffective(resolutionId, targetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(resolutionId, action.ActorSeat, targetSeat, action.EffectiveKind, CardEffectSkipReason.SkillNullified));
        return true;
    }

    internal void CompleteIneffectiveTrickTarget(NullificationWindowFrame pending, int targetSeat)
    {
        if (_coreNativeSlashTargetDuelOperations.TryCompleteCanceledNativeSlashTargetDuel(pending))
            return;
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ParentFrameId, ResolutionFrameStep.ResolvingEffect);
        foreach (var physicalCard in _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.ParentFrameId))
        {
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.ParentFrameId, physicalCard);
        }

        _coreMatchLogOperations.AddLog("CardEffect", $"【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】对 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 无效。", targetSeat, pending.SourceSeat);
        _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind);
    }
}
