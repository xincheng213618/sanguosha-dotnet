#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDeckKindGrowthEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDeckKindGrowthRuntime> _resolve;
    private CoreGameEngineSequentialDeckKindGrowthEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDeckKindGrowthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDeckKindGrowthEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDeckKindGrowthRuntime> resolve) => new(resolve);
}
