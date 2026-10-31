#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeAttackCompletionEntries
{
    internal void CompleteAttack(NativeOwners.CardAttackHandle attack) => _resolve().CompleteAttack(attack);
    internal void CompleteAttackAfterCardResolution(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteAttackAfterCardResolution(completion);
}
