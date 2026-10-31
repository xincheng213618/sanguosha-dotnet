#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundCardsRecastsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardsRecastsRuntime> _resolve;
    private CoreGameEngineBoundCardsRecastsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardsRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundCardsRecastsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardsRecastsRuntime> resolve) => new(resolve);
}
