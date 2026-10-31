#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeferredCardsRuntime))]
internal sealed partial class CoreGameEngineDeferredCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeferredCardsRuntime> _resolve;
    private CoreGameEngineDeferredCardsEntries(Func<EngineOwners.CoreGameEngineDeferredCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredCardsEntries Create(Func<EngineOwners.CoreGameEngineDeferredCardsRuntime> resolve) => new(resolve);
}
