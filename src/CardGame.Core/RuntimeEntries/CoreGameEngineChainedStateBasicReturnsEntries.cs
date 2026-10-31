#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime))]
internal sealed partial class CoreGameEngineChainedStateBasicReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _resolve;
    private CoreGameEngineChainedStateBasicReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineChainedStateBasicReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> resolve) => new(resolve);
}
