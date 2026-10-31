#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeEquipmentUseEntries
{
    internal void CompleteEquipmentUse(CharacterState source, Card equipment, long resolutionId) => _resolve().CompleteEquipmentUse(source, equipment, resolutionId);
    internal void ResolveEquip(CharacterState source, Card equipment) => _resolve().ResolveEquip(source, equipment);
}
