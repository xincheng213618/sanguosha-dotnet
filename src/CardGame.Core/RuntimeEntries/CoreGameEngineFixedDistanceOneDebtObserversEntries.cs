#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedDistanceOneDebtObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> _resolve;
    private CoreGameEngineFixedDistanceOneDebtObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedDistanceOneDebtObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> resolve) => new(resolve);
}
