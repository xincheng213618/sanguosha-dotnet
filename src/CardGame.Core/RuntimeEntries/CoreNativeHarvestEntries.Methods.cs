#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeHarvestEntries
{
    internal void BeginHarvestSelection(NativeOwners.GroupCardHandle group) => _resolve().BeginHarvestSelection(group);
    internal void FinishFiveGrains(NativeOwners.GroupCardHandle group) => _resolve().FinishFiveGrains(group);
    internal void ResolveHarvestSelection(NativeOwners.GroupCardHandle group, CharacterState picker, int cardId) => _resolve().ResolveHarvestSelection(group, picker, cardId);
}
