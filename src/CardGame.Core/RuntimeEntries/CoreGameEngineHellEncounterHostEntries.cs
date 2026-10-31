#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHellEncounterHostRuntime))]
internal sealed partial class CoreGameEngineHellEncounterHostEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHellEncounterHostRuntime> _resolve;
    private CoreGameEngineHellEncounterHostEntries(Func<EngineOwners.CoreGameEngineHellEncounterHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHellEncounterHostEntries Create(Func<EngineOwners.CoreGameEngineHellEncounterHostRuntime> resolve) => new(resolve);
}
