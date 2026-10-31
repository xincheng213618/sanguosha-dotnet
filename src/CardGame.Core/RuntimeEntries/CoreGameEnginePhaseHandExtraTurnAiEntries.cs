#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime))]
internal sealed partial class CoreGameEnginePhaseHandExtraTurnAiEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> _resolve;
    private CoreGameEnginePhaseHandExtraTurnAiEntries(Func<EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandExtraTurnAiEntries Create(Func<EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> resolve) => new(resolve);
}
