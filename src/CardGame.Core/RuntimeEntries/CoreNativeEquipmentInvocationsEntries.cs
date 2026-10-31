#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime))]
internal sealed partial class CoreNativeEquipmentInvocationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime> _resolve;
    private CoreNativeEquipmentInvocationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeEquipmentInvocationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime> resolve) => new(resolve);
}
