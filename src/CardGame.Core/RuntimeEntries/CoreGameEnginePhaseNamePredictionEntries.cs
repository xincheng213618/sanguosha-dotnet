#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseNamePredictionRuntime))]
internal sealed partial class CoreGameEnginePhaseNamePredictionEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> _resolve;
    private CoreGameEnginePhaseNamePredictionEntries(Func<EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseNamePredictionEntries Create(Func<EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> resolve) => new(resolve);
}
