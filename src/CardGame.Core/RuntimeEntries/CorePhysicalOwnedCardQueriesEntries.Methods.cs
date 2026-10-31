#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePhysicalOwnedCardQueriesEntries
{
    internal (Card Card, CardLocation Location)? FindPublicTargetCard(CharacterState target, int cardId) => _resolve().FindPublicTargetCard(target, cardId);
    internal IReadOnlyList<Card> GetAuthority(CharacterState player) => _resolve().GetAuthority(player);
    internal IReadOnlyList<Card> GetBuquWounds(CharacterState player) => _resolve().GetBuquWounds(player);
    internal IReadOnlyList<Card> GetEquipment(CharacterState player) => _resolve().GetEquipment(player);
    internal IReadOnlyList<Card> GetEquipment2(int seat) => _resolve().GetEquipment(seat);
    internal IReadOnlyList<Card> GetHand(CharacterState player) => _resolve().GetHand(player);
    internal IReadOnlyList<Card> GetJudgment(CharacterState player) => _resolve().GetJudgment(player);
    internal IReadOnlyList<Card> GetJudgment2(int seat) => _resolve().GetJudgment(seat);
    internal IReadOnlyList<Card> GetPlayableCards(CharacterState player) => _resolve().GetPlayableCards(player);
    internal IReadOnlyList<Card> GetWoodenOxGrain(CharacterState player) => _resolve().GetWoodenOxGrain(player);
    internal bool HasTargetCard(CharacterState player) => _resolve().HasTargetCard(player);
}
