#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSingleCardTrickConversionsEntries
{
    internal void AddSingleCardTrickConversionActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddSingleCardTrickConversionActions(actions, actor, playable, selectedPhysicalCardId);
    internal bool TryExecuteSingleCardTrickConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => _resolve().TryExecuteSingleCardTrickConversion(actor, card, action);
}
