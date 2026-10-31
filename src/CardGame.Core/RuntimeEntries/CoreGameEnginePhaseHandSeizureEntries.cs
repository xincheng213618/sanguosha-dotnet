#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed partial class CoreGameEnginePhaseHandSeizureEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _resolve;
    private CoreGameEnginePhaseHandSeizureEntries(Func<EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseHandSeizureEntries Create(Func<EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> resolve) => new(resolve);
}
