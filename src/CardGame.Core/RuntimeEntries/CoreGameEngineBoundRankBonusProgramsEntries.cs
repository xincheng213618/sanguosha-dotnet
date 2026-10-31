#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime))]
internal sealed partial class CoreGameEngineBoundRankBonusProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime> _resolve;
    private CoreGameEngineBoundRankBonusProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundRankBonusProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundRankBonusProgramsRuntime> resolve) => new(resolve);
}
