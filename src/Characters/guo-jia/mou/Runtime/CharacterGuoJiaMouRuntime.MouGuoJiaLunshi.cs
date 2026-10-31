#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMouRuntime
{
    internal bool MouGjLunshiEligible(CharacterState actor, IReadOnlyList<int> cards)
    {
        if (!actor.IsAlive || cards is not [var card] || _cardZonesOperations._cardZones.GetLocation(card) != CardLocation.Hand(actor.Seat) || !_coreNationalSkillsOperations.HasRuntimeSkill(actor, MouGuoJiaRuntime.Lunshi) || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } w || w.DelayedReturn is not null || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(w.EffectCardKind) || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } action } use || use.CardKind != w.EffectCardKind || action.ActorSeat == actor.Seat || use.SourceSeat != action.ActorSeat || !w.TargetSeats.Any(t => t != action.ActorSeat) || !w.TargetSeats.All(use.TargetSeats.Contains))
            return false;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(actor);
        return hand.Count(c => _coreNativeCardAppearanceOperations.EffectiveSuit(actor, c)is Suit.Heart or Suit.Diamond) == hand.Count(c => _coreNativeCardAppearanceOperations.EffectiveSuit(actor, c)is Suit.Spade or Suit.Club);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 37100)]
    internal bool MouGjLunshiBan(ProgramViewAsLegalityQuery q) => q.Source.SkillId == MouGuoJiaRuntime.Lunshi && (q.Source.BindingId != "balanced-hand-nullification" || q.Kind != CardKind.Nullification || !q.ForResponse || q.DyingUse || !MouGjLunshiEligible(q.Owner, q.CardIds));
    internal CardConversionSource? MouGjLunshiSource(CharacterState owner, IReadOnlyList<int> cards, CardConversionSource? selected) => selected is { SkillId: MouGuoJiaRuntime.Lunshi, BindingId: "balanced-hand-nullification" } && selected.OwnerSeat == owner.Seat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, selected.SkillId, selected.SkillInstanceId) && MouGjLunshiEligible(owner, cards) ? selected : null;
}
