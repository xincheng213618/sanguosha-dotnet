#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHellEncounterHostEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> _resolve;
    private CoreGameEngineHellEncounterHostEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHellEncounterHostEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> resolve) => new(resolve);
}
