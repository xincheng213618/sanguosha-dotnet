#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNationalGeneralRevealEntries
{
    internal IReadOnlyList<LegalAction> BuildNationalRevealActions(CharacterState actor) => _resolve().BuildNationalRevealActions(actor);
    internal void RevealNationalGeneral(CharacterState actor, GeneralSelectionSlot slot) => _resolve().RevealNationalGeneral(actor, slot);
    internal void RevealNationalInformation(CharacterState player) => _resolve().RevealNationalInformation(player);
}
