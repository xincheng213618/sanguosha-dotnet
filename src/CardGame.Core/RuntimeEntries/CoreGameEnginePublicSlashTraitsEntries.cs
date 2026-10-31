#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicSlashTraitsRuntime))]
internal sealed partial class CoreGameEnginePublicSlashTraitsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> _resolve;
    private CoreGameEnginePublicSlashTraitsEntries(Func<EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicSlashTraitsEntries Create(Func<EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> resolve) => new(resolve);
}
