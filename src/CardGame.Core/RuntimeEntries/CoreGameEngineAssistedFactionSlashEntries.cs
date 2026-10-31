#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAssistedFactionSlashRuntime))]
internal sealed partial class CoreGameEngineAssistedFactionSlashEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> _resolve;
    private CoreGameEngineAssistedFactionSlashEntries(Func<EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAssistedFactionSlashEntries Create(Func<EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> resolve) => new(resolve);
}
