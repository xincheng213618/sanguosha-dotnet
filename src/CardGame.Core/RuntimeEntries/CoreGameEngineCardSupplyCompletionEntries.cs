#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSupplyCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _resolve;
    private CoreGameEngineCardSupplyCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardSupplyCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> resolve) => new(resolve);
}
