#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime))]
internal sealed partial class CoreGameEngineCurrentEnhancementVirtualReturnEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> _resolve;
    private CoreGameEngineCurrentEnhancementVirtualReturnEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentEnhancementVirtualReturnEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> resolve) => new(resolve);
}
