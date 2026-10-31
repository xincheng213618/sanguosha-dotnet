#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed partial class CoreGameEnginePhaseHandSeizureEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _resolve;
    private CoreGameEnginePhaseHandSeizureEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandSeizureEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> resolve) => new(resolve);
}
