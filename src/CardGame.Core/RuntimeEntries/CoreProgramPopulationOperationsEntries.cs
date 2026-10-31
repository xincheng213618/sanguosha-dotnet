#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPopulationOperationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationOperationsRuntime> _resolve;
    private CoreProgramPopulationOperationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPopulationOperationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationOperationsRuntime> resolve) => new(resolve);
}
