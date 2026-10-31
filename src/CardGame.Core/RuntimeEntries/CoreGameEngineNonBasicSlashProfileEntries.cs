#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNonBasicSlashProfileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> _resolve;
    private CoreGameEngineNonBasicSlashProfileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNonBasicSlashProfileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> resolve) => new(resolve);
}
