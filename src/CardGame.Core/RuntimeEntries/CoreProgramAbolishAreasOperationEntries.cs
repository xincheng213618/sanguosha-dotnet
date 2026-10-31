#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramAbolishAreasOperationRuntime))]
internal sealed partial class CoreProgramAbolishAreasOperationEntries
{
    private readonly Func<EngineOwners.CoreProgramAbolishAreasOperationRuntime> _resolve;
    private CoreProgramAbolishAreasOperationEntries(Func<EngineOwners.CoreProgramAbolishAreasOperationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramAbolishAreasOperationEntries Create(Func<EngineOwners.CoreProgramAbolishAreasOperationRuntime> resolve) => new(resolve);
}
