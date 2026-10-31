#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed partial class CoreGameEngineFieldEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _resolve;
    private CoreGameEngineFieldEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFieldEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> resolve) => new(resolve);
}
