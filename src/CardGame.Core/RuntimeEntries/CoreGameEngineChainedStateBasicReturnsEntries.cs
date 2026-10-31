#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime))]
internal sealed partial class CoreGameEngineChainedStateBasicReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _resolve;
    private CoreGameEngineChainedStateBasicReturnsEntries(Func<EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineChainedStateBasicReturnsEntries Create(Func<EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> resolve) => new(resolve);
}
