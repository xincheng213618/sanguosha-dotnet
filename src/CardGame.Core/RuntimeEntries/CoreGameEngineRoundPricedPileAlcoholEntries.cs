#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundPricedPileAlcoholEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> _resolve;
    private CoreGameEngineRoundPricedPileAlcoholEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundPricedPileAlcoholEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> resolve) => new(resolve);
}
