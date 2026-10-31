#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime))]
internal sealed partial class CoreGameEngineEquipmentPlacementAndHpPairEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> _resolve;
    private CoreGameEngineEquipmentPlacementAndHpPairEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentPlacementAndHpPairEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> resolve) => new(resolve);
}
