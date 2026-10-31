#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime))]
internal sealed partial class CoreGameEnginePhaseNamePredictionObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime> _resolve;
    private CoreGameEnginePhaseNamePredictionObserversEntries(Func<EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseNamePredictionObserversEntries Create(Func<EngineOwners.CoreGameEnginePhaseNamePredictionObserversRuntime> resolve) => new(resolve);
}
