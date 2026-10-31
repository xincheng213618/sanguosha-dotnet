#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRevealedCardHpComparisonEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealedCardHpComparisonRuntime> _resolve;
    private CoreGameEngineRevealedCardHpComparisonEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealedCardHpComparisonRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRevealedCardHpComparisonEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealedCardHpComparisonRuntime> resolve) => new(resolve);
}
