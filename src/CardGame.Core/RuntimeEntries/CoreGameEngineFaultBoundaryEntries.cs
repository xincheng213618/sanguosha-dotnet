#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFaultBoundaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFaultBoundaryRuntime> _resolve;
    private CoreGameEngineFaultBoundaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFaultBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFaultBoundaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFaultBoundaryRuntime> resolve) => new(resolve);
}
