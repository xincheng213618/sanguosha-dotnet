#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoYunMobileShiRuntime
{
    internal const string MobileShiCuifengSkill = "mobile:shi-cuifeng";
    internal const string MobileShiCuifengBinding = "cuifeng-turn-ending";
    internal bool HasMobileShiCuifengCapability => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(MobileShiCuifengSkill, out var definition) && definition.Program is not null;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67201, SkillProgramEffectOp.UseVirtualOrdinaryTrick)]
    internal bool CanOfferMobileShiCuifeng(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != MobileShiCuifengSkill)
            return true;
        return gate.Trigger.Id == MobileShiCuifengBinding && gate.Owner.IsAlive && gate.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } context && context.SourceSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && context.TargetSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && gate.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, gate.OwnerSeat);
    }

    internal bool IsExactMobileShiCuifengProgram(ProgramSkillFrame frame)
    {
        if (frame.SkillId != MobileShiCuifengSkill || frame.TriggerId != MobileShiCuifengBinding || frame.ActivationId != frame.TriggerId || frame.SelectedCardIds.Count != 0 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || frame.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || frame.SelectedTargetSeats is not [var target] || target != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not TurnEndingBoundaryFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex] is not { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate })
            return false;
        return candidate.OwnerSeat == frame.OwnerSeat && candidate.SkillId == frame.SkillId && candidate.BindingId == frame.TriggerId && candidate.SkillInstanceId == frame.SkillInstanceId && candidate.GameplayHash == frame.GameplayHash && candidate.OccurrenceIndex == context.OccurrenceIndex;
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 67201)]
    internal int MobileShiCuifengDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || attack.EffectiveCardKind != CardKind.Duel || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { VirtualOrdinaryTrickOrigin: { SkillId: MobileShiCuifengSkill } origin } use)
            return 0;
        if (origin.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || origin.BindingId != MobileShiCuifengBinding || origin.OutputKind != CardKind.Duel || !_coreVirtualOrdinaryTrickUsesOperations.ValidProgramVirtualOrdinaryTrickUse(use))
            throw new InvalidOperationException("Cuifeng damage lost its originally issued actual-turn Duel.");
        // One accepted response is one played Slash, including a zero-card or
        // many-material conversion. CardRespondedEvent is material-oriented and
        // would count a two-card conversion twice. Uses remain excluded. Read
        // at damage time so Slash responses to this very Duel also increase X,
        // regardless of which duelist ultimately becomes the damage source.
        return _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent or TurnEndedEvent).OfType<CardActionAcceptedEvent>().Where(item => item.Action.Type == CardActionType.Response && CoreNativeCardClassificationsRuntime.IsSlashCard(item.Action.EffectiveKind)).Select(item => item.Action.ActionId).Distinct().Count();
    }
}
