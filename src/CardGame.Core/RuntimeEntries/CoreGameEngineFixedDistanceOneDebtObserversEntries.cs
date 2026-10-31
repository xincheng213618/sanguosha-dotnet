#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime))]
internal sealed partial class CoreGameEngineFixedDistanceOneDebtObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> _resolve;
    private CoreGameEngineFixedDistanceOneDebtObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedDistanceOneDebtObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> resolve) => new(resolve);
}
