#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime))]
internal sealed partial class CoreGameEnginePublicSlashTraitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> _resolve;
    private CoreGameEnginePublicSlashTraitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicSlashTraitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> resolve) => new(resolve);
}
