#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime))]
internal sealed partial class CoreGameEngineFinalTargetTurnCountsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> _resolve;
    private CoreGameEngineFinalTargetTurnCountsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetTurnCountsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> resolve) => new(resolve);
}
