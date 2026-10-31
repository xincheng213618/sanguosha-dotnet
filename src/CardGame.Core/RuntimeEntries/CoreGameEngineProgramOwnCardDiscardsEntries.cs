#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> _resolve;
    private CoreGameEngineProgramOwnCardDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnCardDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> resolve) => new(resolve);
}
