#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSlashCompletionEntries
{
    internal bool FinishAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool allowYingboGift = true) => _resolve().FinishAttack(attack, allowYingboGift);
}
