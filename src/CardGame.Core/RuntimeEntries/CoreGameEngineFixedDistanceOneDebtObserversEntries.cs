#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime))]
internal sealed partial class CoreGameEngineFixedDistanceOneDebtObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> _resolve;
    private CoreGameEngineFixedDistanceOneDebtObserversEntries(Func<EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedDistanceOneDebtObserversEntries Create(Func<EngineOwners.CoreGameEngineFixedDistanceOneDebtObserversRuntime> resolve) => new(resolve);
}
