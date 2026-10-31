#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFaultBoundaryEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFaultBoundaryRuntime> _resolve;
    private CoreGameEngineFaultBoundaryEntries(Func<EngineOwners.CoreGameEngineFaultBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFaultBoundaryEntries Create(Func<EngineOwners.CoreGameEngineFaultBoundaryRuntime> resolve) => new(resolve);
}
