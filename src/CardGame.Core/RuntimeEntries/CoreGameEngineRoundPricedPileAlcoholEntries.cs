#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime))]
internal sealed partial class CoreGameEngineRoundPricedPileAlcoholEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> _resolve;
    private CoreGameEngineRoundPricedPileAlcoholEntries(Func<EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundPricedPileAlcoholEntries Create(Func<EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> resolve) => new(resolve);
}
