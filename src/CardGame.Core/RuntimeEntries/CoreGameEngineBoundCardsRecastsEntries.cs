#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundCardsRecastsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBoundCardsRecastsRuntime> _resolve;
    private CoreGameEngineBoundCardsRecastsEntries(Func<EngineOwners.CoreGameEngineBoundCardsRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundCardsRecastsEntries Create(Func<EngineOwners.CoreGameEngineBoundCardsRecastsRuntime> resolve) => new(resolve);
}
