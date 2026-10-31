#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQingheGongzhuOlRuntime
{
    internal Card? LatestSettledDodgeEntity()
    {
        foreach (var movement in _coreCardMovementJournalCardMovementsAccess._cardMovements.AsEnumerable().Reverse())
        {
            if (movement.To != CardLocation.DiscardPile || movement.Reason.Value != CardMoveReasons.ResponseFinished.Value)
                continue;
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).FirstOrDefault(item => item.Id == movement.CardId);
            if (card is null)
                continue;
            return _coreAdvancedLifecycleOperations.GetAdvancedCard(card.Id).Kind == CardKind.Dodge ? card : null;
        }

        return null;
    }

    internal IReadOnlyList<Card> NonBasicOwnedCards(int ownerSeat) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(ownerSeat)).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(ownerSeat))).Where(card => CardCatalog.Get(card.Kind).CategoryName != "基本牌").ToArray();
    SkillProgramStepOutcome global::CardGame.Core.IQingHeGongZhuProgramHost.ChangjiEndingDamageChoice(ProgramSkillFrame frame, SkillProgramEffect effect) => ChangjiProgramEndingDamageChoice(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IQingHeGongZhuProgramHost.ZengouNullifyDodge(ProgramSkillFrame frame, SkillProgramEffect effect) => ZengouProgramNullifyDodge(frame, effect);
}
