#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime))]
internal sealed partial class CoreGameEngineDyingRescueContinuationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> _resolve;
    private CoreGameEngineDyingRescueContinuationsEntries(Func<EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingRescueContinuationsEntries Create(Func<EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> resolve) => new(resolve);
}
