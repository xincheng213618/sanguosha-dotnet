#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed partial class CoreGameEngineCardSupplyCompletionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _resolve;
    private CoreGameEngineCardSupplyCompletionEntries(Func<EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardSupplyCompletionEntries Create(Func<EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> resolve) => new(resolve);
}
