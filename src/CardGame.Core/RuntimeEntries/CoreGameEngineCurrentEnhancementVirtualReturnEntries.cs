#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentEnhancementVirtualReturnEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> _resolve;
    private CoreGameEngineCurrentEnhancementVirtualReturnEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentEnhancementVirtualReturnEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> resolve) => new(resolve);
}
