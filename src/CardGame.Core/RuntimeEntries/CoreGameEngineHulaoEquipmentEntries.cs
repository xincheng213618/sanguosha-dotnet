#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHulaoEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoEquipmentRuntime> _resolve;
    private CoreGameEngineHulaoEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHulaoEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoEquipmentRuntime> resolve) => new(resolve);
}
