#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedDistanceOneDebtEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> _resolve;
    private CoreGameEngineFixedDistanceOneDebtEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedDistanceOneDebtEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> resolve) => new(resolve);
}
