#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeReturnBridgesEntries
{
    internal void ReturnDyingNativeUse(NativeUseReturnContext context) => _resolve().ReturnDyingNativeUse(context);
    internal bool ReturnQinglongNativeAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnQinglongNativeAttack(completion);
}
