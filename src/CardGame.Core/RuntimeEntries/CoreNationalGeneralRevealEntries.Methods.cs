#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNationalGeneralRevealEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildNationalRevealActions(global::CardGame.Core.CharacterState actor) => _resolve().BuildNationalRevealActions(actor);
    internal void RevealNationalGeneral(global::CardGame.Core.CharacterState actor, global::CardGame.Core.GeneralSelectionSlot slot) => _resolve().RevealNationalGeneral(actor, slot);
    internal void RevealNationalInformation(global::CardGame.Core.CharacterState player) => _resolve().RevealNationalInformation(player);
}
