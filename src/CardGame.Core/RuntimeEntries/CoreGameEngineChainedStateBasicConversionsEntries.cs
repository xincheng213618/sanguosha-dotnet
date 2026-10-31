#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed partial class CoreGameEngineChainedStateBasicConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _resolve;
    private CoreGameEngineChainedStateBasicConversionsEntries(Func<EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineChainedStateBasicConversionsEntries Create(Func<EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> resolve) => new(resolve);
}
