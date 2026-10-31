#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentRecastsEntries
{
    internal void BeginEquipmentRecast(CharacterState actor, Card card, CardKind effectiveKind) => _resolve().BeginEquipmentRecast(actor, card, effectiveKind);
    internal void ContinueEquipmentRecast(long frameId) => _resolve().ContinueEquipmentRecast(frameId);
}
