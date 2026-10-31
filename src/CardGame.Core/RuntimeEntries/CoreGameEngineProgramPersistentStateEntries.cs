#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramPersistentStateRuntime))]
internal sealed partial class CoreGameEngineProgramPersistentStateEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramPersistentStateRuntime> _resolve;
    private CoreGameEngineProgramPersistentStateEntries(Func<EngineOwners.CoreGameEngineProgramPersistentStateRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPersistentStateEntries Create(Func<EngineOwners.CoreGameEngineProgramPersistentStateRuntime> resolve) => new(resolve);
}
