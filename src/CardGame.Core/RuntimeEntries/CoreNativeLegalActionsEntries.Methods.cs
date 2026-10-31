#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeLegalActionsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => _resolve().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActionsCore(global::CardGame.Core.CharacterState actor, bool includeProgramActions, int? selectedPhysicalCardId, global::CardGame.Core.Card? nativeOriginalCard, bool ignoreForcedHandSelection = false) => _resolve().BuildLegalActionsCore(actor, includeProgramActions, selectedPhysicalCardId, nativeOriginalCard, ignoreForcedHandSelection);
}
