#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineChainedStateBasicReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _resolve;
    private CoreGameEngineChainedStateBasicReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineChainedStateBasicReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> resolve) => new(resolve);
}
