#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnEndingOrderRuntime))]
internal sealed partial class CoreGameEngineTurnEndingOrderEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnEndingOrderRuntime> _resolve;
    private CoreGameEngineTurnEndingOrderEntries(Func<EngineOwners.CoreGameEngineTurnEndingOrderRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnEndingOrderEntries Create(Func<EngineOwners.CoreGameEngineTurnEndingOrderRuntime> resolve) => new(resolve);
}
