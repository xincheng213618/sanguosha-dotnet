#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime))]
internal sealed partial class CoreGameEngineEquipmentPairDyingCardObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> _resolve;
    private CoreGameEngineEquipmentPairDyingCardObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentPairDyingCardObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> resolve) => new(resolve);
}
