#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheDismantlementStockRuntime))]
internal sealed partial class CoreGameEngineShanheDismantlementStockEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> _resolve;
    private CoreGameEngineShanheDismantlementStockEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheDismantlementStockEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDismantlementStockRuntime> resolve) => new(resolve);
}
