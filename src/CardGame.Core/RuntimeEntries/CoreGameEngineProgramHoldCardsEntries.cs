#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHoldCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> _resolve;
    private CoreGameEngineProgramHoldCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHoldCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> resolve) => new(resolve);
}
