#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime))]
internal sealed partial class CoreGameEngineEquipmentRecastsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> _resolve;
    private CoreGameEngineEquipmentRecastsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentRecastsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> resolve) => new(resolve);
}
