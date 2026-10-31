#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime))]
internal sealed partial class CoreGameEngineEquipmentPairDyingCardObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> _resolve;
    private CoreGameEngineEquipmentPairDyingCardObserversEntries(Func<EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentPairDyingCardObserversEntries Create(Func<EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> resolve) => new(resolve);
}
