#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativePublicPileEquipmentChildrenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> _resolve;
    private CoreGameEngineNativePublicPileEquipmentChildrenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativePublicPileEquipmentChildrenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> resolve) => new(resolve);
}
