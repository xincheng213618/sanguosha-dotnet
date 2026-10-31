#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPopulationOperationsEntries
{
    private readonly Func<EngineOwners.CoreProgramPopulationOperationsRuntime> _resolve;
    private CoreProgramPopulationOperationsEntries(Func<EngineOwners.CoreProgramPopulationOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPopulationOperationsEntries Create(Func<EngineOwners.CoreProgramPopulationOperationsRuntime> resolve) => new(resolve);
}
