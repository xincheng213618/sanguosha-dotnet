#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime))]
internal sealed partial class CoreNativeEquipmentUseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> _resolve;
    private CoreNativeEquipmentUseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeEquipmentUseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> resolve) => new(resolve);
}
