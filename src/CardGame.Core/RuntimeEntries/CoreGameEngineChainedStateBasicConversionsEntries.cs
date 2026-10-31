#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed partial class CoreGameEngineChainedStateBasicConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _resolve;
    private CoreGameEngineChainedStateBasicConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineChainedStateBasicConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> resolve) => new(resolve);
}
