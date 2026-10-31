#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime))]
internal sealed partial class CoreGameEngineBoundRankBonusProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime> _resolve;
    private CoreGameEngineBoundRankBonusProgramsEntries(Func<EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundRankBonusProgramsEntries Create(Func<EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime> resolve) => new(resolve);
}
