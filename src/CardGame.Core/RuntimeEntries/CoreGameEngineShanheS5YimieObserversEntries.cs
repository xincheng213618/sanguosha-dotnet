#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime))]
internal sealed partial class CoreGameEngineShanheS5YimieObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> _resolve;
    private CoreGameEngineShanheS5YimieObserversEntries(Func<EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5YimieObserversEntries Create(Func<EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> resolve) => new(resolve);
}
