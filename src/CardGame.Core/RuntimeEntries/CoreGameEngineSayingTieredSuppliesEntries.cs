#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSayingTieredSuppliesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSayingTieredSuppliesRuntime> _resolve;
    private CoreGameEngineSayingTieredSuppliesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSayingTieredSuppliesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSayingTieredSuppliesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSayingTieredSuppliesRuntime> resolve) => new(resolve);
}
