#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePreventionDrawObserversRuntime))]
internal sealed partial class CoreGameEnginePreventionDrawObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> _resolve;
    private CoreGameEnginePreventionDrawObserversEntries(Func<EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePreventionDrawObserversEntries Create(Func<EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> resolve) => new(resolve);
}
