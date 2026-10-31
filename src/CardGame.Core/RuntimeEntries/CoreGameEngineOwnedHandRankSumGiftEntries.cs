#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnedHandRankSumGiftRuntime))]
internal sealed partial class CoreGameEngineOwnedHandRankSumGiftEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnedHandRankSumGiftRuntime> _resolve;
    private CoreGameEngineOwnedHandRankSumGiftEntries(Func<EngineOwners.CoreGameEngineOwnedHandRankSumGiftRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedHandRankSumGiftEntries Create(Func<EngineOwners.CoreGameEngineOwnedHandRankSumGiftRuntime> resolve) => new(resolve);
}
