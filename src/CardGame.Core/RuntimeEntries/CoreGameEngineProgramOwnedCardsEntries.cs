#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnedCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> _resolve;
    private CoreGameEngineProgramOwnedCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnedCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> resolve) => new(resolve);
}
