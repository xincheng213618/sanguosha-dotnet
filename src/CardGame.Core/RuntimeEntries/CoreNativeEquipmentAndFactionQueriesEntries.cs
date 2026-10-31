#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed partial class CoreNativeEquipmentAndFactionQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _resolve;
    private CoreNativeEquipmentAndFactionQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeEquipmentAndFactionQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> resolve) => new(resolve);
}
