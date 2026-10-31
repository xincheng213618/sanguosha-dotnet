#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime))]
internal sealed partial class CoreGameEngineTurnDiscardSuitHistoryEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime> _resolve;
    private CoreGameEngineTurnDiscardSuitHistoryEntries(Func<EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDiscardSuitHistoryEntries Create(Func<EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime> resolve) => new(resolve);
}
