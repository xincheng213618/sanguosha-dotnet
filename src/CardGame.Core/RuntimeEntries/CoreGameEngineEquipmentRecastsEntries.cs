#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentRecastsRuntime))]
internal sealed partial class CoreGameEngineEquipmentRecastsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentRecastsRuntime> _resolve;
    private CoreGameEngineEquipmentRecastsEntries(Func<EngineOwners.CoreGameEngineEquipmentRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentRecastsEntries Create(Func<EngineOwners.CoreGameEngineEquipmentRecastsRuntime> resolve) => new(resolve);
}
