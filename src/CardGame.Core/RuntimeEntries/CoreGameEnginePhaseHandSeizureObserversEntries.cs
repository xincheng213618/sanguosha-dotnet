#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandSeizureObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureObserversRuntime> _resolve;
    private CoreGameEnginePhaseHandSeizureObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandSeizureObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureObserversRuntime> resolve) => new(resolve);
}
