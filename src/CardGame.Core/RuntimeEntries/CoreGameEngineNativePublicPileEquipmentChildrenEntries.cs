#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime))]
internal sealed partial class CoreGameEngineNativePublicPileEquipmentChildrenEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> _resolve;
    private CoreGameEngineNativePublicPileEquipmentChildrenEntries(Func<EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativePublicPileEquipmentChildrenEntries Create(Func<EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> resolve) => new(resolve);
}
