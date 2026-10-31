#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDeckKindGrowthEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSequentialDeckKindGrowthRuntime> _resolve;
    private CoreGameEngineSequentialDeckKindGrowthEntries(Func<EngineOwners.CoreGameEngineSequentialDeckKindGrowthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDeckKindGrowthEntries Create(Func<EngineOwners.CoreGameEngineSequentialDeckKindGrowthRuntime> resolve) => new(resolve);
}
