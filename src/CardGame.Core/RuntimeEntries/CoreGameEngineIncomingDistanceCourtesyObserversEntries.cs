#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyObserversRuntime> _resolve;
    private CoreGameEngineIncomingDistanceCourtesyObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIncomingDistanceCourtesyObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyObserversRuntime> resolve) => new(resolve);
}
