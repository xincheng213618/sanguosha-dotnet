#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentSuitHandLimitEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSuitHandLimitRuntime> _resolve;
    private CoreGameEngineEquipmentSuitHandLimitEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSuitHandLimitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentSuitHandLimitEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSuitHandLimitRuntime> resolve) => new(resolve);
}
