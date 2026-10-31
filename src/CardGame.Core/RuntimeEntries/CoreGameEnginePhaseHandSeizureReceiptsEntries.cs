#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime))]
internal sealed partial class CoreGameEnginePhaseHandSeizureReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> _resolve;
    private CoreGameEnginePhaseHandSeizureReceiptsEntries(Func<EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandSeizureReceiptsEntries Create(Func<EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> resolve) => new(resolve);
}
