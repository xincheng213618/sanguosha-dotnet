#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePhysicalOwnedCardQueriesEntries
{
    internal (global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation Location)? FindPublicTargetCard(global::CardGame.Core.CharacterState target, int cardId) => _resolve().FindPublicTargetCard(target, cardId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetAuthority(global::CardGame.Core.CharacterState player) => _resolve().GetAuthority(player);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBuquWounds(global::CardGame.Core.CharacterState player) => _resolve().GetBuquWounds(player);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => _resolve().GetEquipment(player);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment2(int seat) => _resolve().GetEquipment(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => _resolve().GetHand(player);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player) => _resolve().GetJudgment(player);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment2(int seat) => _resolve().GetJudgment(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player) => _resolve().GetPlayableCards(player);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetWoodenOxGrain(global::CardGame.Core.CharacterState player) => _resolve().GetWoodenOxGrain(player);
    internal bool HasTargetCard(global::CardGame.Core.CharacterState player) => _resolve().HasTargetCard(player);
}
