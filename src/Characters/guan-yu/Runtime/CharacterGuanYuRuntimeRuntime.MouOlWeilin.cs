#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    internal bool GuanConversionAvailable(int owner, string skill) => skill != "mou-ol:weilin" || !_corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActorSeat == owner && e.Action.Type == CardActionType.Use && e.Action.ConversionChain.Any(s => s.OwnerSeat == owner && s.SkillId == skill && s.BindingId != "weilin-hand-slash"));
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WeilinIdentity(CharacterState owner, Card card)
    {
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Hand(owner.Seat))
            return null;
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card);
        if (suit == Suit.None)
            return null;
        var policy = _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<WeilinHandColorEvent>().LastOrDefault(e => e.TargetSeat == owner.Seat && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.IsRed == global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(suit));
        return policy is null ? null : new(_contentRegistryOperations._contentRegistry.GetSkill(policy.Source.SkillId).Program!, new SkillProgramCardIdentity("weilin-hand-slash", [CardZoneKind.Hand], [], [], CardKind.Slash, new(SkillProgramConditionKind.Always, 0, [])), policy.Source with { BindingId = "weilin-hand-slash" });
    }

    [ProgramCardLegality(ProgramCardLegalityStage.NamedConversionBan, 20)]
    internal bool GuanConversionUnavailable(ProgramConfiguredViewAsQuery query) => !GuanConversionAvailable(query.Owner.Seat, query.Instance.SkillId);
}
