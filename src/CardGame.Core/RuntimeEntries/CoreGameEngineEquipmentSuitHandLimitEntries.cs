#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentSuitHandLimitEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentSuitHandLimitRuntime> _resolve;
    private CoreGameEngineEquipmentSuitHandLimitEntries(Func<EngineOwners.CoreGameEngineEquipmentSuitHandLimitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentSuitHandLimitEntries Create(Func<EngineOwners.CoreGameEngineEquipmentSuitHandLimitRuntime> resolve) => new(resolve);
}
