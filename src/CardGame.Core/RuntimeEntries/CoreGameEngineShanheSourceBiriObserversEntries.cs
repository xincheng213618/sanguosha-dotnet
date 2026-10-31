#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime))]
internal sealed partial class CoreGameEngineShanheSourceBiriObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> _resolve;
    private CoreGameEngineShanheSourceBiriObserversEntries(Func<EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceBiriObserversEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> resolve) => new(resolve);
}
