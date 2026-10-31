#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandExtraTurnAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> _resolve;
    private CoreGameEnginePhaseHandExtraTurnAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandExtraTurnAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> resolve) => new(resolve);
}
