#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime))]
internal sealed partial class CoreGameEngineShanheS5YimieObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> _resolve;
    private CoreGameEngineShanheS5YimieObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5YimieObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> resolve) => new(resolve);
}
