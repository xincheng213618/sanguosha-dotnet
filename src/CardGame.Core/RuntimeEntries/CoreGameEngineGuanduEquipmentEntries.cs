#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> _resolve;
    private CoreGameEngineGuanduEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> resolve) => new(resolve);
}
