#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduBasicConversionsRuntime
{
    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 45000)]
    internal void AddGuanduBasicConversionActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        if (!_modeRulesOperations._modeRules.Cards.AllowsBasicPlayConversion(CardKind.Intoxication))
            return;
        foreach (var card in playable.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).DistinctBy(card => card.Id).Where(card => (selectedPhysicalCardId is null || card.Id == selectedPhysicalCardId) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)))
            foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.Intoxication, false))
                _coreGuanduCardsOperations.AddGuanduPhysicalOrConvertedActions(actions, actor, card, CardKind.Intoxication, source);
    }

    internal void AddGuanduZeroBasicActions(List<LegalAction> actions, CharacterState actor, CardConversionSource source) => _coreGuanduCardsOperations.AddGuanduPhysicalOrConvertedActions(actions, actor, new(0, CardKind.Intoxication, Suit.None, 0), CardKind.Intoxication, source);
}
