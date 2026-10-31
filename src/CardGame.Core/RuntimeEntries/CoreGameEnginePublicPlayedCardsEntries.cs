#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPlayedCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPlayedCardsRuntime> _resolve;
    private CoreGameEnginePublicPlayedCardsEntries(Func<EngineOwners.CoreGameEnginePublicPlayedCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPlayedCardsEntries Create(Func<EngineOwners.CoreGameEnginePublicPlayedCardsRuntime> resolve) => new(resolve);
}
