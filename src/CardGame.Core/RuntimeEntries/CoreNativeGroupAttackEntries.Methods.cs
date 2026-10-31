#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupAttackEntries
{
    internal void BeginGroupAttackResponse(NativeOwners.GroupCardHandle group) => _resolve().BeginGroupAttackResponse(group);
    internal void ContinueGroupAttackResponseAfterEffect(NativeOwners.GroupCardHandle group) => _resolve().ContinueGroupAttackResponseAfterEffect(group);
}
