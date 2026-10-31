#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime))]
internal sealed partial class CoreProgramPopulationAndArrowBarrageOperationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> _resolve;
    private CoreProgramPopulationAndArrowBarrageOperationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPopulationAndArrowBarrageOperationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> resolve) => new(resolve);
}
