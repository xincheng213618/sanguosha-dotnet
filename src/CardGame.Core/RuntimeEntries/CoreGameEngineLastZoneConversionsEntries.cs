#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLastZoneConversionsRuntime))]
internal sealed partial class CoreGameEngineLastZoneConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLastZoneConversionsRuntime> _resolve;
    private CoreGameEngineLastZoneConversionsEntries(Func<EngineOwners.CoreGameEngineLastZoneConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLastZoneConversionsEntries Create(Func<EngineOwners.CoreGameEngineLastZoneConversionsRuntime> resolve) => new(resolve);
}
