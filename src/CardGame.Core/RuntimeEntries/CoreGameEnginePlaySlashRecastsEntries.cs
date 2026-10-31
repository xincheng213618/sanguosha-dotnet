#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlaySlashRecastsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> _resolve;
    private CoreGameEnginePlaySlashRecastsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlaySlashRecastsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlaySlashRecastsRuntime> resolve) => new(resolve);
}
