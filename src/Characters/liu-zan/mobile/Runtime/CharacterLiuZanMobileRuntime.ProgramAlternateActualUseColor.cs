#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZanMobileRuntime
{
    internal bool TracksAlternateActualUseColor => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantAlternateActualUseColorDraw) == true;

    internal void RecordAlternateActualUseColor(CardActionContext action)
    {
        if (!TracksAlternateActualUseColor || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber < 1 || action.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !IsAlternateColorActualUse(action) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AlternateActualUseColorRecordedEvent>().Any(fact => fact.ActionId == action.ActionId))
            return;
        var previous = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AlternateActualUseColorRecordedEvent>().LastOrDefault(fact => fact.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && fact.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && fact.ActorSeat == action.ActorSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlternateActualUseColorRecordedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, action.ActorSeat, action.ActionId, previous?.ActionId) { IsRed = action.EffectiveIsRed });
    }

    internal bool IsAlternateColorActualUse(CardActionContext action) => action.Type == CardActionType.Use || action.Type == CardActionType.Response && action.ActorSeat == action.ProviderSeat && action.RequesterSeat is null && (action.EffectiveKind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action.EffectiveKind));
    internal (AlternateActualUseColorRecordedEvent Current, AlternateActualUseColorRecordedEvent Previous)? AlternateActualUseColorPair(int owner, ProgramSkillWindowContext context)
    {
        if (owner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } identity } || identity.ActorSeat != owner || context.SourceSeat != owner)
            return null;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AlternateActualUseColorRecordedEvent>().Where(fact => fact.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && fact.TurnOwnerSeat == owner && fact.ActorSeat == owner).ToArray();
        var current = history.SingleOrDefault(fact => fact.ActionId == identity.CardActionId);
        // The exact predecessor is frozen when this accepted use is recorded.
        // Nested actions and later target windows cannot rewrite that relation.
        var previous = current?.PreviousActionId is { } previousId ? history.SingleOrDefault(fact => fact.ActionId == previousId) : null;
        return current is not null && previous is not null && current.IsRed != previous.IsRed ? (current, previous) : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26000)]
    internal bool CanOfferAlternateActualUseColor(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.GrantAlternateActualUseColorDraw))
            return true;
        return AlternateActualUseColorPair(candidate.OwnerSeat, context)is { } pair && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AlternateActualUseColorDrawGrantedEvent>().Any(fact => fact.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && fact.OwnerSeat == candidate.OwnerSeat && fact.ActionId == pair.Current.ActionId && fact.SkillId == candidate.SkillId && fact.SkillInstanceId == candidate.SkillInstanceId);
    }

    internal void GrantAlternateActualUseColorDraw(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(frame);
        if (frame.InstructionIndex != 1 || frame.WindowContext is not { } context || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is not { } window || window.Id != context.ParentFrameId || window.Action.ActionId != context.CardUse?.CardActionId || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || !CanOfferAlternateActualUseColor(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0), trigger, context) || AlternateActualUseColorPair(frame.OwnerSeat, context)is not { } pair)
            throw new InvalidOperationException("An alternating-color draw requires its exact current actual-use pair and source instance.");
        // The following ordinary Draw instruction owns its movement and child return.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlternateActualUseColorDrawGrantedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, frame.OwnerSeat, pair.Current.ActionId, pair.Previous.ActionId, frame.SkillId, frame.SkillInstanceId));
    }

    void IAlternateActualUseColorProgramHost.GrantAlternateActualUseColorDraw(ProgramSkillFrame frame) => GrantAlternateActualUseColorDraw(frame);
}
