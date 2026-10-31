#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime))]
internal sealed partial class CoreGameEngineNonBasicSlashProfileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> _resolve;
    private CoreGameEngineNonBasicSlashProfileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNonBasicSlashProfileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> resolve) => new(resolve);
}
