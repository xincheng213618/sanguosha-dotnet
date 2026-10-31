#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed partial class CoreNativeEquipmentAndFactionQueriesEntries
{
    private readonly Func<EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _resolve;
    private CoreNativeEquipmentAndFactionQueriesEntries(Func<EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeEquipmentAndFactionQueriesEntries Create(Func<EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> resolve) => new(resolve);
}
