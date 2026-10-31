#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedHandRankSumGiftEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumGiftRuntime> _resolve;
    private CoreGameEngineOwnedHandRankSumGiftEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumGiftRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedHandRankSumGiftEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumGiftRuntime> resolve) => new(resolve);
}
