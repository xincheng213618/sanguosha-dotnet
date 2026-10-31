#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime))]
internal sealed partial class CoreGameEngineNativePublicPileEquipmentChildrenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> _resolve;
    private CoreGameEngineNativePublicPileEquipmentChildrenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativePublicPileEquipmentChildrenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> resolve) => new(resolve);
}
