#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime))]
internal sealed partial class CoreGameEnginePhaseHandSeizureReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> _resolve;
    private CoreGameEnginePhaseHandSeizureReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandSeizureReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> resolve) => new(resolve);
}
