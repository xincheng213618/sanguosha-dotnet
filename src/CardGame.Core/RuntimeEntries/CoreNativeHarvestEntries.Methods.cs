#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeHarvestEntries
{
    internal void BeginHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().BeginHarvestSelection(group);
    internal void FinishFiveGrains(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().FinishFiveGrains(group);
    internal void ResolveHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState picker, int cardId) => _resolve().ResolveHarvestSelection(group, picker, cardId);
}
