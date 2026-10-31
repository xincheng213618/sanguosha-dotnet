#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime))]
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> _resolve;
    private CoreGameEngineOutsidePhaseDrawDiscardObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOutsidePhaseDrawDiscardObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> resolve) => new(resolve);
}
