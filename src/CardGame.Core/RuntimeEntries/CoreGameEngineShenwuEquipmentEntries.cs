#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShenwuEquipmentRuntime))]
internal sealed partial class CoreGameEngineShenwuEquipmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShenwuEquipmentRuntime> _resolve;
    private CoreGameEngineShenwuEquipmentEntries(Func<EngineOwners.CoreGameEngineShenwuEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuEquipmentEntries Create(Func<EngineOwners.CoreGameEngineShenwuEquipmentRuntime> resolve) => new(resolve);
}
