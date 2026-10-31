#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineIncomingDistanceCourtesyObserversRuntime))]
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineIncomingDistanceCourtesyObserversRuntime> _resolve;
    private CoreGameEngineIncomingDistanceCourtesyObserversEntries(Func<EngineOwners.CoreGameEngineIncomingDistanceCourtesyObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIncomingDistanceCourtesyObserversEntries Create(Func<EngineOwners.CoreGameEngineIncomingDistanceCourtesyObserversRuntime> resolve) => new(resolve);
}
