#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeLogicalDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _resolve;
    private CoreGameEngineNativeLogicalDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeLogicalDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> resolve) => new(resolve);
}
