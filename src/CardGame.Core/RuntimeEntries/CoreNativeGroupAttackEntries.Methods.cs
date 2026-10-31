#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupAttackEntries
{
    internal void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().BeginGroupAttackResponse(group);
    internal void ContinueGroupAttackResponseAfterEffect(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().ContinueGroupAttackResponseAfterEffect(group);
}
