#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPersistentStateEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> _resolve;
    private CoreGameEngineProgramPersistentStateEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPersistentStateEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> resolve) => new(resolve);
}
