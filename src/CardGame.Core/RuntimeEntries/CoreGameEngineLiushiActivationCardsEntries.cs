#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiushiActivationCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiActivationCardsRuntime> _resolve;
    private CoreGameEngineLiushiActivationCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiActivationCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiushiActivationCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiActivationCardsRuntime> resolve) => new(resolve);
}
