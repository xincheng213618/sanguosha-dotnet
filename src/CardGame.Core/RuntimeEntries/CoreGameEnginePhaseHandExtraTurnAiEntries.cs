#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime))]
internal sealed partial class CoreGameEnginePhaseHandExtraTurnAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> _resolve;
    private CoreGameEnginePhaseHandExtraTurnAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandExtraTurnAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> resolve) => new(resolve);
}
