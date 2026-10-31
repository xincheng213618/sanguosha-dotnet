#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicSlashTraitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> _resolve;
    private CoreGameEnginePublicSlashTraitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicSlashTraitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> resolve) => new(resolve);
}
