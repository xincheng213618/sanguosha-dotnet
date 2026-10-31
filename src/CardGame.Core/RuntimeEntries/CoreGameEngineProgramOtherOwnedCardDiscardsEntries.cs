#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOtherOwnedCardDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> _resolve;
    private CoreGameEngineProgramOtherOwnedCardDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOtherOwnedCardDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> resolve) => new(resolve);
}
