#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeLegalActionsEntries
{
    internal IReadOnlyList<LegalAction> BuildLegalActions(CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => _resolve().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
    internal IReadOnlyList<LegalAction> BuildLegalActionsCore(CharacterState actor, bool includeProgramActions, int? selectedPhysicalCardId, Card? nativeOriginalCard, bool ignoreForcedHandSelection = false) => _resolve().BuildLegalActionsCore(actor, includeProgramActions, selectedPhysicalCardId, nativeOriginalCard, ignoreForcedHandSelection);
}
