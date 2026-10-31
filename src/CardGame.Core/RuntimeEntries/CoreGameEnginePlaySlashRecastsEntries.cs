#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlaySlashRecastsRuntime))]
internal sealed partial class CoreGameEnginePlaySlashRecastsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> _resolve;
    private CoreGameEnginePlaySlashRecastsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlaySlashRecastsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> resolve) => new(resolve);
}
