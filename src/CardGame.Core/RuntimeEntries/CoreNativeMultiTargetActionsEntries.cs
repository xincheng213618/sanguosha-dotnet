#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeMultiTargetActionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> _resolve;
    private CoreNativeMultiTargetActionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeMultiTargetActionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> resolve) => new(resolve);
}
