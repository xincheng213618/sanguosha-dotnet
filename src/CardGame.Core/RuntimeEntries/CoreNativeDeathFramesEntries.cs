#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDeathFramesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> _resolve;
    private CoreNativeDeathFramesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDeathFramesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> resolve) => new(resolve);
}
