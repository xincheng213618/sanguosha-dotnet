#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupCompletionEntries
{
    internal void FinishGroupAttack(global::CardGame.Core.NativeOwners.GroupCardHandle group, bool allowYingboGift = true) => _resolve().FinishGroupAttack(group, allowYingboGift);
    internal void FinishGroupRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().FinishGroupRecovery(group);
}
