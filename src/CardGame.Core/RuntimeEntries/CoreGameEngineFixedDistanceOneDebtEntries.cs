#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime))]
internal sealed partial class CoreGameEngineFixedDistanceOneDebtEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> _resolve;
    private CoreGameEngineFixedDistanceOneDebtEntries(Func<EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedDistanceOneDebtEntries Create(Func<EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> resolve) => new(resolve);
}
