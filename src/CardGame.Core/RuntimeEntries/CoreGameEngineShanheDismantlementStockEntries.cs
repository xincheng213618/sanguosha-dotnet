#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheDismantlementStockRuntime))]
internal sealed partial class CoreGameEngineShanheDismantlementStockEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> _resolve;
    private CoreGameEngineShanheDismantlementStockEntries(Func<EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheDismantlementStockEntries Create(Func<EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> resolve) => new(resolve);
}
