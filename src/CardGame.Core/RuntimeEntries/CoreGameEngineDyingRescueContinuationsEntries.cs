#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime))]
internal sealed partial class CoreGameEngineDyingRescueContinuationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> _resolve;
    private CoreGameEngineDyingRescueContinuationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingRescueContinuationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> resolve) => new(resolve);
}
