#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime))]
internal sealed partial class CoreGameEngineHellEncounterHostEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> _resolve;
    private CoreGameEngineHellEncounterHostEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHellEncounterHostEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> resolve) => new(resolve);
}
