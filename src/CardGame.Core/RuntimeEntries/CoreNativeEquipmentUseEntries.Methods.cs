#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeEquipmentUseEntries
{
    internal void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId) => _resolve().CompleteEquipmentUse(source, equipment, resolutionId);
    internal void ResolveEquip(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment) => _resolve().ResolveEquip(source, equipment);
}
