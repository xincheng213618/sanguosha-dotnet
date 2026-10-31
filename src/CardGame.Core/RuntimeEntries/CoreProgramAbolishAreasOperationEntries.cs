#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramAbolishAreasOperationRuntime))]
internal sealed partial class CoreProgramAbolishAreasOperationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAbolishAreasOperationRuntime> _resolve;
    private CoreProgramAbolishAreasOperationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAbolishAreasOperationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramAbolishAreasOperationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAbolishAreasOperationRuntime> resolve) => new(resolve);
}
