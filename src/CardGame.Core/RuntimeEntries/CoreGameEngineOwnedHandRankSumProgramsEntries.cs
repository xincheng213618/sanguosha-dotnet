#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime))]
internal sealed partial class CoreGameEngineOwnedHandRankSumProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> _resolve;
    private CoreGameEngineOwnedHandRankSumProgramsEntries(Func<EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedHandRankSumProgramsEntries Create(Func<EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> resolve) => new(resolve);
}
