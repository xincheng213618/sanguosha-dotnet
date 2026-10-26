namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Appearance belongs to the native use, before a nested effect can change
    // its material's zone or the actor's effective-suit rules.
    private bool MatchesOwnerUsedHeartSlash(IDamageAttempt attack) =>
        attack is CardAttackHandle && !attack.IsSourceLess && !attack.IsChainPropagation &&
        !attack.IsDelayedJudgmentDamage && !attack.IsProgramJudgmentDamage &&
        attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is { } kind && IsSlashCard(kind) &&
        _resolutionStack.SingleOrDefault(frame => frame.Id == attack.ResolutionId) is CardUseFrame use &&
        use.SourceSeat == attack.SourceSeat && use.CardKind == kind &&
        use.Action is { Type: CardActionType.Use, EffectiveSuit: Suit.Heart } action &&
        action.ActorSeat == attack.SourceSeat;

    private Suit PublicProposedSlashSuit(GameSnapshot view, int actorSeat, LegalAction action, CardKind kind)
    {
        if (!IsSlashCard(kind) || action.MaxCardCount > 1 || action.CardId is not { } id) return Suit.None;
        var actor = view.Players.Single(player => player.Seat == actorSeat);
        var material = actor.Hand.Concat(actor.WoodenOxGrain ?? []).Concat(actor.Equipment)
            .Concat(actor.AuthorityCards ?? []).FirstOrDefault(card => card.Id == id);
        // A proposed action supplies no authority to inspect a foreign entity.
        if (material is null) return Suit.None;
        var appearance = ApplyProgramUseAppearance(_players[actorSeat], GetAttackCard(id), action.ConversionSource);
        var sources = (action.ConversionSource is { } primary ? new[] { primary } : Array.Empty<CardConversionSource>())
            .Concat(action.AdditionalConversionSources ?? []).ToArray();
        return WuJingLiubingUsedSuit(actorSeat, appearance, kind, [id], sources,
            CaptureUsedCardSuit(actorSeat, [id], appearance)) ?? Suit.None;
    }
}
