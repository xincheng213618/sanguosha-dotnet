#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramSlashRequestRuntime))]
internal sealed partial class CoreGameEngineProgramSlashRequestEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramSlashRequestRuntime> _resolve;
    private CoreGameEngineProgramSlashRequestEntries(Func<EngineOwners.CoreGameEngineProgramSlashRequestRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramSlashRequestEntries Create(Func<EngineOwners.CoreGameEngineProgramSlashRequestRuntime> resolve) => new(resolve);
}
