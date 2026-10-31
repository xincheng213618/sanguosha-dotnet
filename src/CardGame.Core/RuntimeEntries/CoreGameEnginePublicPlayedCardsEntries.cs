#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPlayedCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPlayedCardsRuntime> _resolve;
    private CoreGameEnginePublicPlayedCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPlayedCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPlayedCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPlayedCardsRuntime> resolve) => new(resolve);
}
