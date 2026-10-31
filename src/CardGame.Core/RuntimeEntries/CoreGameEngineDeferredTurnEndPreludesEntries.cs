#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime))]
internal sealed partial class CoreGameEngineDeferredTurnEndPreludesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> _resolve;
    private CoreGameEngineDeferredTurnEndPreludesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredTurnEndPreludesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> resolve) => new(resolve);
}
