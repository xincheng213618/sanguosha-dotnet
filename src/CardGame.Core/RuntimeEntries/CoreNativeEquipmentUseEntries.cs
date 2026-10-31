#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeEquipmentUseRuntime))]
internal sealed partial class CoreNativeEquipmentUseEntries
{
    private readonly Func<EngineOwners.CoreNativeEquipmentUseRuntime> _resolve;
    private CoreNativeEquipmentUseEntries(Func<EngineOwners.CoreNativeEquipmentUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeEquipmentUseEntries Create(Func<EngineOwners.CoreNativeEquipmentUseRuntime> resolve) => new(resolve);
}
