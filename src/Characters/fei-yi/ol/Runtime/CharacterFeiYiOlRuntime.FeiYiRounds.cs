#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFeiYiOlRuntime
{
    internal void BindFeiYiRankEffects(long id, ProgramTrickContinuation trick)
    {
        if (!HasFeiYiHezhong || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { Type: CardActionType.Use } action } use || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || use.CardKind == CardKind.Nullification || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || action.EffectiveRank is not { } rank || use.EffectEpisodes?.Episodes.Any(episode => episode.Issue.Source.SkillId == "ol:hezhong" && episode.Started > 0) == true || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            return;
        var acceptedActor = FeiYiAcceptedActor(use);
        if (acceptedActor is not { } originalActor)
            throw new InvalidOperationException("Hezhong lost the original accepted actor or its exact native replacement chain.");
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<FeiYiRankEffectIssuedEvent>().Where(g => g.Source.OwnerSeat == originalActor && g.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && g.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && (g.Greater ? rank > g.Rank : rank < g.Rank) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<FeiYiRankEffectBoundEvent>().Any(e => e.GrantId == g.FrameId)).ToArray();
        foreach (var grant in grants)
        {
            // The singleton's real paid direction is a future-use business
            // grant. Its producing program may already have left the stack.
            _nativeCardEffectEpisodes.Issue(id, new(grant.FrameId, 0, 0, grant.Source, grant.GameplayHash, new(10)), trick);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeiYiRankEffectBoundEvent(grant.FrameId, id, action.ActionId, rank, originalActor));
        }
    }

    internal int? FeiYiAcceptedActor(CardUseFrame use)
    {
        var declared = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Where(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.CardKind == use.CardKind).ToArray();
        return declared is [var original] && use.Action is { Type: CardActionType.Use } action && action.ActorSeat == use.SourceSeat && _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, action.ProviderSeat) ? original.SourceSeat : null;
    }
}
