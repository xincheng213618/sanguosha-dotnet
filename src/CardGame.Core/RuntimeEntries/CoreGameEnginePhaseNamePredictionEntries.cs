#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime))]
internal sealed partial class CoreGameEnginePhaseNamePredictionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> _resolve;
    private CoreGameEnginePhaseNamePredictionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseNamePredictionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> resolve) => new(resolve);
}
