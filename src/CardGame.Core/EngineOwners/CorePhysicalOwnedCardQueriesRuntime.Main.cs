#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePhysicalOwnedCardQueriesRuntime
{
    internal IReadOnlyList<Card> GetHand(CharacterState player) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(player.Seat));
    internal IReadOnlyList<Card> GetWoodenOxGrain(CharacterState player) => _cardZonesOperations._cardZones.CardsAt(CardLocation.WoodenOxGrain(player.Seat));
    internal IReadOnlyList<Card> GetBuquWounds(CharacterState player) => _cardZonesOperations._cardZones.CardsAt(CardLocation.BuquWound(player.Seat));
    internal IReadOnlyList<Card> GetAuthority(CharacterState player) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Authority(player.Seat));
    internal IReadOnlyList<Card> GetPlayableCards(CharacterState player)
    {
        IReadOnlyList<Card> cards = _coreCardDeclarationsOperations.AvailableDeclarationCard(player) is { } declared
            ? GetHand(player).Append(declared).ToArray()
            : _coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx &&
                _coreFieldEquipmentOperations.GetEquipmentWithPrintedEffects(player).Any(card => card.Kind == CardKind.WoodenOx)
                ? GetHand(player).Concat(GetWoodenOxGrain(player)).ToArray()
                : GetHand(player);
        cards = _coreRequestedDeckBasicsOperations.AppendRequestedDeckBasicCards(player, cards);
        cards = _characterFuRongOlOperations.AppendFuRongDiscardCards(player, cards);
        cards = _characterDuanJiongOlOperations.AppendDuanJiongSaoguDiscardCards(player, cards);
        cards = _characterYaDanOlOperations.AppendQingyaDiscardCards(player, cards);
        cards = _characterGongsunZanMouMobileOperations.AppendMouGszHuPlayable(player, cards);
        return _characterLuSuMobileShiOperations.AppendShiLsPlayableCards(player, _characterBaiLingYunAnniversaryOperations.AppendAnniversaryBaiLingYunPlayableCards(player, _characterDuKuiAnniversaryOperations.AppendDuKuiFanyinPlayableCards(player, cards)));
    }

    internal IReadOnlyList<Card> GetEquipment(CharacterState player) => GetEquipment(player.Seat);
    internal IReadOnlyList<Card> GetEquipment(int seat) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(seat));
    internal bool HasTargetCard(CharacterState player) => GetHand(player).Count > 0 || GetEquipment(player).Count > 0 || GetJudgment(player).Count > 0;
    internal (Card Card, CardLocation Location)? FindPublicTargetCard(CharacterState target, int cardId)
    {
        var equipment = GetEquipment(target).SingleOrDefault(card => card.Id == cardId);
        if (equipment is not null)
        {
            return (equipment, CardLocation.Equipment(target.Seat));
        }

        var judgment = GetJudgment(target).SingleOrDefault(card => card.Id == cardId);
        return judgment is null ? null : (judgment, CardLocation.Judgment(target.Seat));
    }

    internal IReadOnlyList<Card> GetJudgment(CharacterState player) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(player.Seat));
    internal IReadOnlyList<Card> GetJudgment(int seat) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(seat));
}
