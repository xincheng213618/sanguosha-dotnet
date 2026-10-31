#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime))]
internal sealed partial class CoreGameEngineProgramSlashRequestEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> _resolve;
    private CoreGameEngineProgramSlashRequestEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramSlashRequestEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> resolve) => new(resolve);
}
