#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime))]
internal sealed partial class CoreGameEnginePhaseStartCardControlObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime> _resolve;
    private CoreGameEnginePhaseStartCardControlObserversEntries(Func<EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseStartCardControlObserversEntries Create(Func<EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime> resolve) => new(resolve);
}
