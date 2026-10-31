#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduEquipmentRuntime))]
internal sealed partial class CoreGameEngineGuanduEquipmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduEquipmentRuntime> _resolve;
    private CoreGameEngineGuanduEquipmentEntries(Func<EngineOwners.CoreGameEngineGuanduEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduEquipmentEntries Create(Func<EngineOwners.CoreGameEngineGuanduEquipmentRuntime> resolve) => new(resolve);
}
