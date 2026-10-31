#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> _resolve;
    private CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> resolve) => new(resolve);
}
