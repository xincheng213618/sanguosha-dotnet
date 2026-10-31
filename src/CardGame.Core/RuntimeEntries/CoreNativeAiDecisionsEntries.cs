#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeAiDecisionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _resolve;
    private CoreNativeAiDecisionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeAiDecisionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> resolve) => new(resolve);
}
