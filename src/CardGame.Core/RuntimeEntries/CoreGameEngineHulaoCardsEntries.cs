#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHulaoCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> _resolve;
    private CoreGameEngineHulaoCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHulaoCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> resolve) => new(resolve);
}
