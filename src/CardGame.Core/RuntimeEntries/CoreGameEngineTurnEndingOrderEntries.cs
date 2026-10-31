#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnEndingOrderEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndingOrderRuntime> _resolve;
    private CoreGameEngineTurnEndingOrderEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndingOrderRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnEndingOrderEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndingOrderRuntime> resolve) => new(resolve);
}
