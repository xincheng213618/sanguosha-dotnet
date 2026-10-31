#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConvertedSlashResponseRuntime))]
internal sealed partial class CoreGameEngineConvertedSlashResponseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> _resolve;
    private CoreGameEngineConvertedSlashResponseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConvertedSlashResponseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> resolve) => new(resolve);
}
