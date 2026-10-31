#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentRecastsEntries
{
    internal void BeginEquipmentRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind) => _resolve().BeginEquipmentRecast(actor, card, effectiveKind);
    internal void ContinueEquipmentRecast(long frameId) => _resolve().ContinueEquipmentRecast(frameId);
}
