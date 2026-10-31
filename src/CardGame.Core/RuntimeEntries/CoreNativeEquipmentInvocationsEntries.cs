#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeEquipmentInvocationsRuntime))]
internal sealed partial class CoreNativeEquipmentInvocationsEntries
{
    private readonly Func<EngineOwners.CoreNativeEquipmentInvocationsRuntime> _resolve;
    private CoreNativeEquipmentInvocationsEntries(Func<EngineOwners.CoreNativeEquipmentInvocationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeEquipmentInvocationsEntries Create(Func<EngineOwners.CoreNativeEquipmentInvocationsRuntime> resolve) => new(resolve);
}
