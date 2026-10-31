#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime))]
internal sealed partial class CoreProgramPopulationAndArrowBarrageOperationsEntries
{
    private readonly Func<EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> _resolve;
    private CoreProgramPopulationAndArrowBarrageOperationsEntries(Func<EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPopulationAndArrowBarrageOperationsEntries Create(Func<EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> resolve) => new(resolve);
}
