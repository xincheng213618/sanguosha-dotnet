#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSingleCardTrickConversionsEntries
{
    internal void AddSingleCardTrickConversionActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddSingleCardTrickConversionActions(actions, actor, playable, selectedPhysicalCardId);
    internal bool TryExecuteSingleCardTrickConversion(CharacterState actor, Card card, LegalAction action) => _resolve().TryExecuteSingleCardTrickConversion(actor, card, action);
}
