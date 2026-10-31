#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime))]
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> _resolve;
    private CoreGameEngineProgramOwnCardDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnCardDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> resolve) => new(resolve);
}
