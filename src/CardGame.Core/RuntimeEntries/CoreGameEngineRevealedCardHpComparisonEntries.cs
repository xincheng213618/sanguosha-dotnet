#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRevealedCardHpComparisonEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRevealedCardHpComparisonRuntime> _resolve;
    private CoreGameEngineRevealedCardHpComparisonEntries(Func<EngineOwners.CoreGameEngineRevealedCardHpComparisonRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRevealedCardHpComparisonEntries Create(Func<EngineOwners.CoreGameEngineRevealedCardHpComparisonRuntime> resolve) => new(resolve);
}
