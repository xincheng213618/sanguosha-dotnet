#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime))]
internal sealed partial class CoreGameEngineDeferredTurnEndPreludesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> _resolve;
    private CoreGameEngineDeferredTurnEndPreludesEntries(Func<EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredTurnEndPreludesEntries Create(Func<EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> resolve) => new(resolve);
}
