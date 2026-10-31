#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime))]
internal sealed partial class CoreGameEngineAssistedFactionSlashEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> _resolve;
    private CoreGameEngineAssistedFactionSlashEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAssistedFactionSlashEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> resolve) => new(resolve);
}
