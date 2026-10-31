#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeReturnBridgesEntries
{
    internal void ReturnDyingNativeUse(global::CardGame.Core.NativeUseReturnContext context) => _resolve().ReturnDyingNativeUse(context);
    internal bool ReturnQinglongNativeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnQinglongNativeAttack(completion);
}
