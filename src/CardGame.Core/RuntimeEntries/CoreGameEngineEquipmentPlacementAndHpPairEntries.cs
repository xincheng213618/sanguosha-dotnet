#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime))]
internal sealed partial class CoreGameEngineEquipmentPlacementAndHpPairEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> _resolve;
    private CoreGameEngineEquipmentPlacementAndHpPairEntries(Func<EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentPlacementAndHpPairEntries Create(Func<EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> resolve) => new(resolve);
}
