#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime))]
internal sealed partial class CoreGameEnginePhaseNamePredictionObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime> _resolve;
    private CoreGameEnginePhaseNamePredictionObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseNamePredictionObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime> resolve) => new(resolve);
}
