#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime))]
internal sealed partial class CoreGameEngineCurrentEnhancementVirtualReturnEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> _resolve;
    private CoreGameEngineCurrentEnhancementVirtualReturnEntries(Func<EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentEnhancementVirtualReturnEntries Create(Func<EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> resolve) => new(resolve);
}
