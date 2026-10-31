#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePlaySlashRecastsRuntime))]
internal sealed partial class CoreGameEnginePlaySlashRecastsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> _resolve;
    private CoreGameEnginePlaySlashRecastsEntries(Func<EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlaySlashRecastsEntries Create(Func<EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> resolve) => new(resolve);
}
