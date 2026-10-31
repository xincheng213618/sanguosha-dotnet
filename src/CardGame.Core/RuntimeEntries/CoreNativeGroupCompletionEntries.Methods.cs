#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupCompletionEntries
{
    internal void FinishGroupAttack(NativeOwners.GroupCardHandle group, bool allowYingboGift = true) => _resolve().FinishGroupAttack(group, allowYingboGift);
    internal void FinishGroupRecovery(NativeOwners.GroupCardHandle group) => _resolve().FinishGroupRecovery(group);
}
