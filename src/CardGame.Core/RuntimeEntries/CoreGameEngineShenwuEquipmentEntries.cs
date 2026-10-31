#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime))]
internal sealed partial class CoreGameEngineShenwuEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> _resolve;
    private CoreGameEngineShenwuEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> resolve) => new(resolve);
}
