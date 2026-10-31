#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed partial class CoreGameEngineFieldEquipmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFieldEquipmentRuntime> _resolve;
    private CoreGameEngineFieldEquipmentEntries(Func<EngineOwners.CoreGameEngineFieldEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFieldEquipmentEntries Create(Func<EngineOwners.CoreGameEngineFieldEquipmentRuntime> resolve) => new(resolve);
}
