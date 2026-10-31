#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheDismantlementStockEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> _resolve;
    private CoreGameEngineShanheDismantlementStockEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheDismantlementStockEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> resolve) => new(resolve);
}
