#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedHandRankSumProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> _resolve;
    private CoreGameEngineOwnedHandRankSumProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedHandRankSumProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> resolve) => new(resolve);
}
