#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHulaoEquipmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHulaoEquipmentRuntime> _resolve;
    private CoreGameEngineHulaoEquipmentEntries(Func<EngineOwners.CoreGameEngineHulaoEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHulaoEquipmentEntries Create(Func<EngineOwners.CoreGameEngineHulaoEquipmentRuntime> resolve) => new(resolve);
}
