#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime))]
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> _resolve;
    private CoreGameEngineOutsidePhaseDrawDiscardObserversEntries(Func<EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOutsidePhaseDrawDiscardObserversEntries Create(Func<EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> resolve) => new(resolve);
}
