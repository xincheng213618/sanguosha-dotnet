#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeAttackCompletionEntries
{
    internal void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CompleteAttack(attack);
    internal void CompleteAttackAfterCardResolution(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteAttackAfterCardResolution(completion);
}
