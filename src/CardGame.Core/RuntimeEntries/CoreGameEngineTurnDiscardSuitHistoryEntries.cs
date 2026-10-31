#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime))]
internal sealed partial class CoreGameEngineTurnDiscardSuitHistoryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime> _resolve;
    private CoreGameEngineTurnDiscardSuitHistoryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDiscardSuitHistoryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDiscardSuitHistoryRuntime> resolve) => new(resolve);
}
