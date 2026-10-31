#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAssistedSlashPaymentRuntime
{
    internal (IReadOnlyList<Card> Cards, CardKind Kind, CardConversionSource? Conversion) ReadAssistedSlashPayment(CharacterState actor, PromptChoice selected)
    {
        var kind = Enum.Parse<CardKind>(selected.Parameters["effective-kind"]);
        IReadOnlyList<Card> cards;
        CardConversionSource? conversion;
        if (selected.Cards.Count > 1 || CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out var selectedSource) && _characterKongShuOlOperations.IsLeiluanSource(selectedSource))
        {
            if (selected.Parameters.GetValueOrDefault("equipment") == CardKind.ZhangbaSerpentSpear.ToString())
            {
                cards = _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor).Single(pair => pair.Select(card => card.Id).SequenceEqual(selected.Cards));
                conversion = null;
            }
            else
            {
                if (!CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out conversion) || conversion is null)
                    throw new InvalidOperationException("A multi-card Slash lost its conversion source.");
                cards = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, selected.Cards, kind, false, conversion)?.Cards ?? throw new InvalidOperationException("The physical Slash source cards became unavailable.");
            }
        }
        else
        {
            var card = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(actor)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileSlashUseCards(actor)).Single(item => item.Id == selected.Cards.Single());
            CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._selectedUseConversion);
            _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._hasSelectedUseConversionChoice = true;
            conversion = _coreCardConversionsOperations.GetSelectedUseConversion(actor, actor, card, kind);
            if (conversion is null)
            {
                _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._selectedUseConversion = null;
                _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._hasSelectedUseConversionChoice = true;
            }

            cards = [card];
        }

        return (cards, kind, conversion);
    }
}
