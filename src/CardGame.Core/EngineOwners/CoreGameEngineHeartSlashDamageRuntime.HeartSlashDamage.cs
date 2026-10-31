#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHeartSlashDamageRuntime
{
    internal bool MatchesOwnerUsedHeartSlash(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack is global::CardGame.Core.NativeOwners.CardAttackHandle && !attack.IsSourceLess && !attack.IsChainPropagation && !attack.IsDelayedJudgmentDamage && !attack.IsProgramJudgmentDamage && attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is { } kind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == attack.ResolutionId)is CardUseFrame use && use.SourceSeat == attack.SourceSeat && use.CardKind == kind && use.Action is { Type: CardActionType.Use, EffectiveSuit: Suit.Heart } action && action.ActorSeat == attack.SourceSeat;
    internal Suit PublicProposedSlashSuit(GameSnapshot view, int actorSeat, LegalAction action, CardKind kind)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || action.MaxCardCount > 1 || action.CardId is not { } id)
            return Suit.None;
        var actor = view.Players.Single(player => player.Seat == actorSeat);
        var material = actor.Hand.Concat(actor.WoodenOxGrain ?? []).Concat(actor.Equipment).Concat(actor.AuthorityCards ?? []).FirstOrDefault(card => card.Id == id);
        // A proposed action supplies no authority to inspect a foreign entity.
        if (material is null)
            return Suit.None;
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreCardAttackStateOperations.GetAttackCard(id), action.ConversionSource);
        var sources = (action.ConversionSource is { } primary ? new[]
        {
            primary
        }

        : Array.Empty<CardConversionSource>()).Concat(action.AdditionalConversionSources ?? []).ToArray();
        return _characterWuJingOlOperations.WuJingLiubingUsedSuit(actorSeat, appearance, kind, [id], sources, _corePlayCardHistoryOperations.CaptureUsedCardSuit(actorSeat, [id], appearance)) ?? Suit.None;
    }
}
