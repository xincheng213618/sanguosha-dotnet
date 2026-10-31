#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDragonBoatBasicConversionsRuntime
{
    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 43000)]
    internal void AddDragonBoatBasicConversionActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        if (!_modeRulesOperations._modeRules.Cards.AllowsBasicPlayConversion(CardKind.Zong) && !_modeRulesOperations._modeRules.Cards.AllowsBasicPlayConversion(CardKind.RealgarWine))
            return;
        foreach (var card in playable.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).DistinctBy(card => card.Id).Where(card => (selectedPhysicalCardId is null || card.Id == selectedPhysicalCardId) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)))
            foreach (var kind in new[]
            {
                CardKind.Zong,
                CardKind.RealgarWine
            }.Where(_modeRulesOperations._modeRules.Cards.AllowsBasicPlayConversion))
                foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false))
                    _coreDragonBoatCardsOperations.AddDragonBoatPhysicalOrConvertedActions(actions, actor, card, kind, source);
    }

    internal void AddDragonBoatZeroBasicActions(List<LegalAction> actions, CharacterState actor, CardConversionSource source, CardKind kind) => _coreDragonBoatCardsOperations.AddDragonBoatPhysicalOrConvertedActions(actions, actor, new(0, kind, Suit.None, 0), kind, source);
}
