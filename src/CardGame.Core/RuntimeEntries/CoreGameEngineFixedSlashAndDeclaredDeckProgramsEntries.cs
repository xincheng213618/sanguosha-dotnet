#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime))]
internal sealed partial class CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> _resolve;
    private CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries(Func<EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries Create(Func<EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> resolve) => new(resolve);
}
